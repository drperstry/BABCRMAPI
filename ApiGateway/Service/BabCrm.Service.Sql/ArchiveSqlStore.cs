using BabCrm.Core;
using BabCrm.Core.Caching;
using BabCrm.Logging;
using BabCrm.Service.ArchiveDataModels;
using BabCrm.Service.Sql.SqlConnectionFactory;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BabCrm.Service.Sql
{
    public class ArchiveSqlStore : IArchiveSqlStore
    {
        private readonly BabArchiveDataContext _dbContext;
        private readonly Dictionary<string, string> _tablesDictionary;
        private readonly ICacheManager _cacheManager;
        private readonly ISqlConnectionFactory _connectionFactory;

        public ArchiveSqlStore(BabArchiveDataContext dbContext, ICacheManager inMemoryCacheManager, ISqlConnectionFactory connectionFactory)
        {
            _dbContext = dbContext;
            _cacheManager = inMemoryCacheManager;
            _connectionFactory = connectionFactory;
            _tablesDictionary = new Dictionary<string, string>
            {
            {"Case_Request", "CaseRequests"},
            {"CASE_PROCESS_HISTORY", "CaseProcessHistories"},
            {"UPDATE_ID_EXPIRY_DATE", "UpdateIdExpiryDates"},
            {"ONLINE_ACCOUNT_ACTIVATION", "OnlineAccountActivations"},
            {"Cheque_Book_Request", "ChequeBookRequests"},
            {"Stop_Cheque", "StopCheques"},
            {"Beneficiary_Activation", "BeneficiaryActivations"},
            {"Delete_Beneficiary", "DeleteBeneficiaries"},
            {"Update_Beneficiary_Local_Bank", "UpdateBeneficiaryLocalBanks"},
            {"BR_UB_International_Bank", "BrUbInternationalBanks"},
            {"BR_UB_WU", "BrUbWus"},
            {"TransFast_EnjazEasy_Charity", "TransFastEnjazEasyCharities"},
            {"Card_Request_Replacement", "CardRequestReplacements"},
            {"Card_Request_Renewal", "CardRequestRenewals"},
            {"Internet_Purchase_Stat_Change", "InternetPurchaseStatChanges"},
            {"Card_Request_Whitelist", "CardRequestWhitelists"},
            {"Card_Request_Stop_Card", "CardRequestStopCards"},
            {"Card_Payment_Transfer_From_Acc", "CardPaymentTransferFromAccs"},
            {"CR_Cash_Adva_Transfer_To_Acc", "CrCashAdvaTransferToAccs"},
            {"Card_Request_Limit_Maintenance", "CardRequestLimitMaintenances"},
            {"CR_ATM_Pin_Unblocked_ATM_Pin", "CrAtmPinUnblockedAtmPins"},
            {"CR_LM_Change_POS_Daily_Limit", "CrLmChangePosDailyLimits"},
            {"Channels_IVR_IVR_PIN_Blocking", "ChannelsIvrIvrPinBlockings"},
            {"Channels_IVR_Unbloc_IVR_regist", "ChannelsIvrUnblocIvrRegists"},
            {"CSR_Beneficiary_Maintenance", "CsrBeneficiaryMaintenances"},
            {"Enjaz_Request_SMS_Notification", "EnjazRequestSmsNotifications"},
            {"Enjaz_Req_MFA_Blocked_Removal", "EnjazReqMfaBlockedRemovals"},
            {"Fund_Transfer_Request", "FundTransferRequests"},
            {"FTR_Bank_AlBilad", "FtrBankAlBilads"},
            {"FTR_Local_Bank", "FtrLocalBanks"},
            {"FTR_WU_TF_EE_C", "FtrWuTfEeCs"},
            {"FTR_International_Bank", "FtrInternationalBanks"},
            {"Token_Request_Registration", "TokenRequestRegistrations"},
            {"Token_Request_Cancelation", "TokenRequestCancelations"},
            {"Token_Request_Resend_tkn_link", "TokenRequestResendTknLinks"},
            {"Token_Request_Activating", "TokenRequestActivatings"},
            {"IPO_Request", "IpoRequests"},
            {"IPO_Processing_IPO_Subscr", "IpoProcessingIpoSubscrs"},
            {"Retail_Request", "RetailRequests"},
            {"Retail_Request_SMS_Notific", "RetailRequestSmsNotifics"},
            {"Retail_MFA_MFA_Blocked_Removal", "RetailMfaMfaBlockedRemovals"},
            {"Sadad_Request_Bill_Payment", "SadadRequestBillPayments"},
            {"Sadad_Req_MOI_Payment_Refund", "SadadReqMoiPaymentRefunds"},
            {"Sadad_Req_Update_Favorite_Bill", "SadadReqUpdateFavoriteBills"},
            {"Sadad_Req_Add_Favorite_Bill", "SadadReqAddFavoriteBills"},
            {"Sadad_Req_Delete_Favorite_Bill", "SadadReqDeleteFavoriteBills"},
            {"Account_Activation", "AccountActivations"},
            {"Other_Request", "OtherRequests"},
            {"Enjaz_Request", "EnjazRequests"},
            {"Channels_Req_Unblock_Authent", "ChannelsReqUnblockAuthents"},
            {"FTR_OWN_ACCOUNT", "FtrOwnAccounts"},
            {"Request_Status", "RequestStatuses"},
            {"CRM_Request_Category", "CrmRequestCategorys"},
            {"CRM_Request_Type", "CrmRequestTypes"},
            {"CRM_Request_Sub_Type", "CrmRequestSubTypes"},
            {"CRM_Product", "CrmProducts"},
            {"CRM_Request_Status", "CrmRequestStatuses"}
            };
        }

        public async Task<IEnumerable<CaseRequest>> GetArchivedCaseRequests(string cif, DateTime? startDate, DateTime? endDate, string requestNumber = "")
        {
            var archivedCaseRequests = await _dbContext.CaseRequests.Include(t => t.CrmStatus).Include(t => t.CrmType).Include(t => t.CrmSubType).Include(t => t.CrmProduct).Include(t => t.CrmCategory)
                .Where(c => string.IsNullOrWhiteSpace(cif) || c.Cif == cif)
                .Where(c => string.IsNullOrWhiteSpace(requestNumber) || c.SrNum == requestNumber)
                .Where(c => startDate == null || c.SubmissionDate >= startDate)
                .Where(c => endDate == null || c.SubmissionDate <= endDate).ToListAsync();

            return archivedCaseRequests;


        }

        public async Task<IEnumerable<CaseRequest>> GetArchivedCases(string cif, DateTime? startDate, DateTime? endDate, string mobileNumber)
        {
            var archivedCaseRequests = await _dbContext.CaseRequests.Include(t => t.CrmStatus).Include(t => t.CrmType).Include(t => t.CrmSubType).Include(t => t.CrmProduct).Include(t => t.CrmCategory)
                .Where(c => string.IsNullOrWhiteSpace(cif) || c.Cif == cif)
                .Where(c => string.IsNullOrWhiteSpace(mobileNumber) || c.MobileNumber == mobileNumber)
                .Where(c => startDate == null || c.SubmissionDate >= startDate)
                .Where(c => endDate == null || c.SubmissionDate <= endDate).ToListAsync();

            return archivedCaseRequests;


        }

        public async Task<SubmissionResponse> GetArchivedData(string tableName, CommonFilter filter)
        {
            try
            {
                if (tableName.Equals("Case_Request"))
                {
                    var caseRequests = await GetArchivedCaseRequests(filter?.Cif, filter?.StartDate, filter?.EndDate, filter?.RequestNumber);
                    return SubmissionResponse.Ok(caseRequests);
                }

                var queryResult = await ExecuteQueryAsync(tableName, filter);

                return SubmissionResponse.Ok(queryResult);
            }
            catch (Exception ex)
            {
                Logger.Log(ex, $@"Method = {nameof(GetArchivedData)}, TableName = {tableName}, filter = {JsonConvert.SerializeObject(filter)}");
                return SubmissionResponse.Error($"Method = {nameof(GetArchivedData)}, TableName = {tableName}, filter = {JsonConvert.SerializeObject(filter)}, EXCEPTION = {ex}");
            }
        }

        private async Task<List<Dictionary<string, object>>> ExecuteQueryAsync(string tableName, CommonFilter filter)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                var query = BuildQueryString(tableName, filter, out List<SqlParameter> parameters);
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        return ConvertDataTableToList(reader);
                    }
                }
            }
        }

        private string BuildQueryString(string tableName, CommonFilter filter, out List<SqlParameter> parameters)
        {
            var queryBuilder = new StringBuilder($"SELECT * FROM {tableName} WHERE 1=1");
            parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(filter?.Cif))
            {
                queryBuilder.Append(" AND [CIF] = @cif");
                parameters.Add(new SqlParameter("@cif", filter.Cif));
            }

            if (!string.IsNullOrWhiteSpace(filter?.RequestNumber))
            {
                queryBuilder.Append(" AND [SR_NUM] = @requestnumber");
                parameters.Add(new SqlParameter("@requestnumber", filter.RequestNumber));
            }

            if (tableName == "S_SRV_REQ_OTHER")
            {
                if (filter != null && filter.StartDate.HasValue)
                {
                    queryBuilder.Append(" AND [CREATED] >= @startdate");
                    parameters.Add(new SqlParameter("@startdate", filter.StartDate.Value));
                }

                if (filter != null && filter.EndDate.HasValue)
                {
                    queryBuilder.Append(" AND [CREATED] <= @enddate");
                    parameters.Add(new SqlParameter("@enddate", filter.EndDate.Value));
                }
            }
            else
            {
                if (filter != null && filter.StartDate.HasValue)
                {
                    queryBuilder.Append(" AND [Submission Date] >= @startdate");
                    parameters.Add(new SqlParameter("@startdate", filter.StartDate.Value));
                }

                if (filter != null && filter.EndDate.HasValue)
                {
                    queryBuilder.Append(" AND [Submission Date] <= @enddate");
                    parameters.Add(new SqlParameter("@enddate", filter.EndDate.Value));
                }
            }

            return queryBuilder.ToString();
        }

        private List<Dictionary<string, object>> ConvertDataTableToList(SqlDataReader reader)
        {
            var dataTable = new DataTable();
            dataTable.Load(reader);
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in dataTable.Columns)
                {
                    dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                }
                list.Add(dict);
            }

            return list;
        }


        private string GetDbSetTypeByTableName(Dictionary<string, string> dictionary, string key)
        {
            if (dictionary.TryGetValue(key, out string value))
            {
                return value;
            }
            else
            {
                return "Key not found";
            }
        }
    }
}