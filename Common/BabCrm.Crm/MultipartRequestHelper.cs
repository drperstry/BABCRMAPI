namespace BabCrm.Crm
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http.Headers;

    internal static class MultipartRequestHelper
    {
        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"

        // The spec says 70 characters is a reasonable limit.
        internal static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = contentType.Parameters.FirstOrDefault(p => string.Equals(p.Name, "boundary"))?.Value;

            // boundary = Microsoft.Net.Http.Headers.HeaderUtilities.RemoveQuotes(boundary);

            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException($"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }

        internal static bool IsMultipartContentType(this string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
