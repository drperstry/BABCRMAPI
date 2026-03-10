using BabCrm.Core;
using BabCrm.Logging;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using ReportService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using ParameterValue = ReportService.ParameterValue;

namespace Bab.Jobs
{
    public class ReportsManager
    {
        private IConfiguration configuration;
        
        private string ReportExecution2005EndPointUrl;
        private string SsrsServiceAccountActiveDirectoryUserName;
        private string SsrsServiceAccountActiveDirectoryPassword;
        private string SsrsServiceAccountActiveDirectoryDomain;
        private string ReportPath;
        private string ReportWidth;
        private string ReportHeight;
        private string ReportFormat;  // Other options include WORDOPENXML and EXCELOPENXML
        private string HistoryId = null;
        private int sendTimeout;
        private int recieveTimeout;
        private int openTimeout;
        private int closeTimeout;

        private readonly ReportExecutionServiceSoapClient _reportExecutionService;

        public ReportsManager()
        {
            IConfiguration configuration = new ConfigurationBuilder()
         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .Build();

            var reportSection = configuration.GetSection("Report");

            ReportExecution2005EndPointUrl = reportSection["Url"];
            SsrsServiceAccountActiveDirectoryUserName = reportSection["Name"];
            SsrsServiceAccountActiveDirectoryPassword = reportSection["Code"];
            SsrsServiceAccountActiveDirectoryDomain = reportSection["Domain"];
            ReportPath = reportSection["Path"];
            ReportWidth = reportSection["Width"];
            ReportHeight = reportSection["Height"];
            ReportFormat = reportSection["Format"];
            sendTimeout = reportSection.GetValue<int>("SendTimeout");
            recieveTimeout = reportSection.GetValue<int>("ReceiveTimeout");
            openTimeout = reportSection.GetValue<int>("OpenTimeout");
            closeTimeout = reportSection.GetValue<int>("CloseTimeout");

            _reportExecutionService = CreateClient();

        }

        public async Task<string> GetReport(IEnumerable<string> caseIds)
        {
            //ReportExecutionServiceSoapClient rs = CreateClient();
            try
            {
                var trustedHeader = new TrustedUserHeader();

                LoadReportResponse loadReponse = await LoadReport(_reportExecutionService, trustedHeader);

                await AddParametersToTheReport(_reportExecutionService, loadReponse.ExecutionHeader, trustedHeader, caseIds);

                RenderResponse response = await RenderReportByteArrayAsync(loadReponse.ExecutionHeader, trustedHeader, _reportExecutionService, ReportFormat, ReportWidth, ReportHeight);

                if (!response.Result.IsEmpty())
                {
                    var result = Convert.ToBase64String(response.Result);
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }

        private async Task<LoadReportResponse> LoadReport(ReportExecutionServiceSoapClient reportExecutionService, TrustedUserHeader trustedHeader)
        {
            try
            {
                // Get the report and set the execution header.
                // Failure to set the execution header will result in this error: "The session identifier is missing. A session identifier is required for this operation."
                // See https://social.msdn.microsoft.com/Forums/sqlserver/en-US/17199edb-5c63-4815-8f86-917f09809504/executionheadervalue-missing-from-reportexecutionservicesoapclient
                LoadReportResponse loadReponse = await reportExecutionService.LoadReportAsync(trustedHeader, ReportPath, HistoryId);

                return loadReponse;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }


        public async Task<string> GetReport(string reportPath)
        {
            try
            {
                //ReportExecutionServiceSoapClient rs = CreateClient();
                var trustedHeader = new TrustedUserHeader();

                LoadReportResponse loadReponse = await LoadReport(_reportExecutionService, trustedHeader, reportPath);

                RenderResponse response = await RenderReportByteArrayAsync(loadReponse.ExecutionHeader, trustedHeader, _reportExecutionService, ReportFormat, ReportWidth, ReportHeight);

                if (!response.Result.IsEmpty())
                {
                    var result = Convert.ToBase64String(response.Result);
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }

        private async Task<LoadReportResponse> LoadReport(ReportExecutionServiceSoapClient reportExecutionService, TrustedUserHeader trustedHeader, string reportPath)
        {
            try
            {
                // Get the report and set the execution header.
                // Failure to set the execution header will result in this error: "The session identifier is missing. A session identifier is required for this operation."
                // See https://social.msdn.microsoft.com/Forums/sqlserver/en-US/17199edb-5c63-4815-8f86-917f09809504/executionheadervalue-missing-from-reportexecutionservicesoapclient
                LoadReportResponse loadReponse = await reportExecutionService.LoadReportAsync(trustedHeader, reportPath, HistoryId);

                return loadReponse;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }

        private async Task<SetExecutionParametersResponse> AddParametersToTheReport(ReportExecutionServiceSoapClient reportExecutionService, ExecutionHeader executionHeader, TrustedUserHeader trustedHeader, IEnumerable<string> caseIds)
        {
            try
            {
                //var caseIdsCondition =
                //caseIds.IsEmpty() ? string.Empty :
                //caseIds.Select(r => $@"<value>{{{r}}}</value>")
                //         .Aggregate((m1, m2) => m1 + m2);

                var caseIdsCondition = caseIds.IsEmpty() ? string.Empty : string.Join(", ", caseIds.Select(r => $@"''{r}''"));

                var caseIdsString = $@"'{caseIdsCondition}'";

                Logger.Log(caseIdsString);
                // Add parameters to the report
                var reportParameters = new List<ParameterValue>();
                reportParameters.Add(new ParameterValue() { Name = "CRM_FilteredIncident", Value = caseIdsString });
                //reportParameters.Add(new ParameterValue() { Name = "GeographyID", Value = "6071" });


                SetExecutionParametersResponse setParamsResponse = await reportExecutionService.SetExecutionParametersAsync(executionHeader, trustedHeader, reportParameters.ToArray(), "en-US");

                return setParamsResponse;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }

        private async Task<RenderResponse> RenderReportByteArrayAsync(ExecutionHeader execHeader, TrustedUserHeader trustedHeader,
           ReportExecutionServiceSoapClient reportExecutionService, string format, string width, string height)
        {
            try
            {
                string deviceInfo = String.Format("<DeviceInfo><PageHeight>{0}</PageHeight><PageWidth>{1}</PageWidth><PrintDpiX>300</PrintDpiX><PrintDpiY>300</PrintDpiY></DeviceInfo>", height, width);

                var renderRequest = new RenderRequest(execHeader, trustedHeader, format, deviceInfo);

                //get report bytes
                RenderResponse response = await reportExecutionService.RenderAsync(renderRequest);

                return response;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }


        private ReportExecutionServiceSoapClient CreateClient()
        {
            try
            {
                //var rsBinding = new BasicHttpBinding();
                //rsBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                //rsBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;

                //var rsBinding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
                var rsBinding = new BasicHttpBinding();
                rsBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
                rsBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                rsBinding.UseDefaultWebProxy = true;

                // So we can download reports bigger than 64 KBytes
                // See https://stackoverflow.com/questions/884235/wcf-how-to-increase-message-size-quota
                rsBinding.MaxBufferPoolSize = 20000000;
                rsBinding.MaxBufferSize = 20000000;
                rsBinding.MaxReceivedMessageSize = 20000000;

                //change the timeouts for the connection
                rsBinding.SendTimeout = TimeSpan.FromMinutes(sendTimeout);
                rsBinding.ReceiveTimeout = TimeSpan.FromMinutes(recieveTimeout);
                rsBinding.OpenTimeout = TimeSpan.FromMinutes(openTimeout);
                rsBinding.CloseTimeout = TimeSpan.FromMinutes(closeTimeout);

                var rsEndpointAddress = new EndpointAddress(ReportExecution2005EndPointUrl);
                var rsClient = new ReportExecutionServiceSoapClient(rsBinding, rsEndpointAddress);
                rsClient.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                rsClient.ClientCredentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
                // Set user name and password
                //rsClient.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential(
                //    SsrsServiceAccountActiveDirectoryUserName.Decrypt(),
                //    SsrsServiceAccountActiveDirectoryPassword.Decrypt(),
                //    SsrsServiceAccountActiveDirectoryDomain);


                return rsClient;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }
    }
}
