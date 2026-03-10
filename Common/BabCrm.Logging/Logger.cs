using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace BabCrm.Logging
{
    public static class Logger
    {
        static IConfiguration configuration;
        private static string logPath;
        static Logger()
        {
            IConfiguration configuration = new ConfigurationBuilder()
         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .Build();

            logPath = configuration.GetSection("LogsPath").Value;
        }
        public static bool LastLogPassed;

        public static void Log(Exception exception, string note = "")
        {
            LastLogPassed = false;

            try
            {
                var logPath = GetLogFilePath();

                var logText = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: ERROR
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Note: {note}
Message: {exception?.Message}
StackTrace: {exception?.StackTrace}";

                File.AppendAllText(logPath, logText);

                LastLogPassed = true;
            }
            catch
            {
                // Ignored
            }
        }

        public static void Log(Exception exception, string query, params System.Data.SqlClient.SqlParameter[] sqlParameters)
        {
            LastLogPassed = false;

            try
            {
                var logPath = GetLogFilePath();

                var logText = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: ERROR
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Note: {LogAggregator(query, sqlParameters)}
Message: {exception?.Message}
StackTrace: {exception?.StackTrace}";

                File.AppendAllText(logPath, logText);

                LastLogPassed = true;
            }
            catch
            {
                // Ignored
            }
        }

        public static void Log(string logText, string type = "INFO")
        {
            LastLogPassed = false;

            try
            {
                var logPath = GetLogFilePath();

                var logTextInfo = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: {type}
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Message: {logText}";

                File.AppendAllText(logPath, logTextInfo);

                LastLogPassed = true;
            }
            catch
            {
                // Ignored
            }
        }

        public static void LogInfo(string logText)
        {
            LastLogPassed = false;

            try
            {
                var logPath = GetLogFilePath("Info");

                var logTextInfo = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: INFO
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Message: {logText}";

                File.AppendAllText(logPath, logTextInfo);

                LastLogPassed = true;
            }
            catch
            {
                // Ignored
            }
        }

        public static void LogArchiveDataError(string logText)
        {
            LastLogPassed = false;

            try
            {
                var logPath = GetLogFilePath("ArchiveDataError");

                var logTextInfo = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: INFO
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Message: {logText}";

                File.AppendAllText(logPath, logTextInfo);

                LastLogPassed = true;
            }
            catch
            {
                // Ignored
            }
        }

        public static void Log(ExceptionContext context)
        {
            LastLogPassed = false;

            try
            {
                var logPath = GetLogFilePath();

                var logText = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: ERROR
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Url: {context?.HttpContext?.Request?.Method} {context?.HttpContext?.Request?.GetUri()}
Payload: {GetBody(context)}
Message: {context?.Exception?.Message}
StackTrace: {context?.Exception?.StackTrace}
InnerMessage: {context?.Exception?.InnerException?.Message}
InnerStackTrace: {context?.Exception?.InnerException?.StackTrace}";

                File.AppendAllText(logPath, logText);

                LastLogPassed = false;
            }
            catch
            {
                // Ignored
            }
        }

        private static string GetLogFilePath(string prefix = "")
        {
            try
            {
                var folder = logPath + "\\logs";

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                return Path.Combine(folder, $"{prefix}Log_{DateTime.Now:yyyyMMdd}.txt");
            }
            catch
            {
                // ignored
            }
            return string.Empty;
        }

        private static string GetBody(ExceptionContext context)
        {
            var result = "";
            try
            {
                var req = context.HttpContext.Request;

                // Allows using several time the stream in ASP.Net Core
                req.EnableBuffering();

                // Arguments: Stream, Encoding, detect encoding, buffer size 
                // AND, the most important: keep stream opened
                if (req.Body != null)
                {
                    using (StreamReader reader
                        = new StreamReader(req.Body, System.Text.Encoding.UTF8, true, 1024, true))
                    {
                        result = reader.ReadToEnd();
                    }

                    // Rewind, so the core is not lost when it looks the body for the request
                    req.Body.Position = 0;
                }

                // Do your work with bodyStr
            }
            catch
            {
                // Ignored
            }
            return result;
        }

        private static string LogAggregator(string query, params SqlParameter[] sqlParameters)
        {
            var result = string.Empty;

            result += $"{(string.IsNullOrWhiteSpace(query) ? "No query" : query)}";

            if (sqlParameters?.Any() ?? false)
            {
                result += Environment.NewLine + "Params: " +
                    sqlParameters
                        .Select(m => $"{m.ParameterName}: {m.Value}")
                        .Aggregate((m1, m2) => m1 + Environment.NewLine + m2);
            }

            return result + Environment.NewLine;
        }

        public static void ApiLog(object payload, Exception ex, string method, string route = "")
        {
            LastLogPassed = false;

            try
            {
                var logPath = GetLogFilePath("api_");

                var logText = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: ERROR
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Method: {method}
Payload: {JsonConvert.SerializeObject(payload)}
Route: {route}
Message: {ex?.Message}
StackTrace: {ex?.StackTrace}
InnerMessage: {ex?.InnerException?.Message}
InnerStackTrace: {ex?.InnerException?.StackTrace}";

                File.AppendAllText(logPath, logText);

                LastLogPassed = false;
            }
            catch
            {
                // Ignored
            }
        }

        public static void RequestLog(object payload, string method, string url, string header, string query, string requestId)
        {
            LastLogPassed = false;

            try
            {
                var logPath = GetLogFilePath("request_");

                var logText = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: INFO
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Method: {method}
Payload: {JsonConvert.SerializeObject(payload)}
Url: {url}
Header: {header}
Query: {query}
RequestId : {requestId}";

                File.AppendAllText(logPath, logText);

                LastLogPassed = false;
            }
            catch
            {
                // Ignored
            }
        }

        public static void InfoLog(string message, string requestId, string url,string languageHeader, object data, string verb,string authHeader,string apiVersion = "")
        {
            Serilog.Log.Information(message + " with requestId {RequestId}, info details => URL : {Url},languageHeader : {languageHeader} object : {Data}, Verb : {Verb}, AuthHeader : {AuthHeader}, ApiVersion : {ApiVersion}", requestId, url,languageHeader, data,verb,authHeader,apiVersion);
        }
    }
}
