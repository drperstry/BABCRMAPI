namespace BabCrm.Crm
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;

    public class ApplicationHttpContent : ByteArrayContent
    {
        private const string DefaultMediaType = "application/http";

        public ApplicationHttpContent(string content)
            : this(content, null, null)
        {
        }

        public ApplicationHttpContent(string content, Encoding encoding)
            : this(content, encoding, null)
        {
        }

        public ApplicationHttpContent(string content, Encoding encoding, string mediaType)
            : base(GetContentByteArray(content, encoding))
        {
            // Initialize the 'Content-Type' header with information provided by parameters. 
            MediaTypeHeaderValue headerValue = new MediaTypeHeaderValue((mediaType == null) ? DefaultMediaType : mediaType);

            Headers.ContentType = headerValue;
            Headers.Add("Content-Transfer-Encoding", "binary");
        }

        // A StringContent is essentially a ByteArrayContent. We serialize the string into a byte-array in the 
        // constructor using encoding information provided by the caller (if any). When this content is sent, the
        // Content-Length can be retrieved easily (length of the array).
        private static byte[] GetContentByteArray(string content, Encoding encoding)
        {
            // In this case we treat 'null' strings different from string.Empty in order to be consistent with our 
            // other *Content constructors: 'null' throws, empty values are allowed.
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            return encoding.GetBytes(content);
        }
    }
}
