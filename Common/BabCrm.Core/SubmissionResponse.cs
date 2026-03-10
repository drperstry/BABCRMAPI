namespace BabCrm.Core
{
    public sealed class SubmissionResponse
    {
        /// <summary>
        /// Gets or sets success
        /// </summary>
        public bool Success { get; set; }

        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets extra data
        /// </summary>
        public object ExtraData { get; set; }

        /// <summary>
        /// Returns a success response
        /// </summary>
        public static SubmissionResponse Ok(object extraData = null)
        {
            return new SubmissionResponse()
            {
                Success = true,
                ExtraData = extraData
            };
        }

        public static SubmissionResponse Of(bool success, string errorCodeIfNeeded = "")
        {
            var identitySubmissionResponse = new SubmissionResponse()
            {
                Success = success,
                ErrorCode = success ? string.Empty : errorCodeIfNeeded
            };

            return identitySubmissionResponse;
        }

        /// <summary>
        /// Returns an error response
        /// </summary>
        public static SubmissionResponse Error(string errorCode, object extraData = null)
        {
            return new SubmissionResponse
            {
                Success = false,
                ErrorCode = errorCode,
                ExtraData = extraData
            };
        }

        /// <summary>
        /// Returns an error response
        /// </summary>
        public static SubmissionResponse Error(object extraData = null)
        {
            return new SubmissionResponse
            {
                Success = false,
                ExtraData = extraData
            };
        }
    }
}
