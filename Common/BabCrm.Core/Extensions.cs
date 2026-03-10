

namespace BabCrm.Core
{
    using BabCrm.Core.Configurations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Security.Cryptography;
    using System.Text;

    public static class Extensions
    {
        public static bool IsEmpty(this string text) => string.IsNullOrWhiteSpace(text);

        public static bool IsEmpty<T>(this IEnumerable<T> list) => !(list?.Any() ?? false);

        public static bool IsNullOrEmpty(this string text) => string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(text);

        public static bool NotEmpty(this string text) => !string.IsNullOrWhiteSpace(text);

        public static Guid AsGuid(this string guidString)
        {
            Guid guid = Guid.Empty;

            if (Guid.TryParse(guidString, out guid))
            {
                return guid;
            }

            return Guid.Empty;
        }

        public static string SafeSubstring(this string text, int length)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return text.Length > length
                ? text.Substring(0, length)
                : text;
        }

        public static string GetHash(this string input)
        {
            var hash = SHA256.Create();
            byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes("s1iodi$" + input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static T GetConfigurationValue<T>(this IConfiguration configuration, string key, T defaultValue = default(T)) where T : IConvertible
        {
            var stringValue = configuration[key];

            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                return (T)Convert.ChangeType(stringValue, typeof(T));
            }

            return defaultValue;
        }

        public static bool ContainsIgnoreCase(this string value, string text)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                    !string.IsNullOrWhiteSpace(text) &&
                    value.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        public static void AddAsConfigSingleton<TService, TImplementation>(this IServiceCollection services, IConfiguration configuration)
           where TService : class, IBabCrmConfig
           where TImplementation : class, TService
        {
            services.AddSingleton<TService, TImplementation>(serviceProvider =>
            {
                var implementation = Activator.CreateInstance<TImplementation>();
                implementation.LoadConfiguration(configuration);
                return implementation;
            });
        }

        public static string RemoveExtension(this string value)
        {
            string[] parts = value.Split('.');
            string result = parts[0];
            return result;
        }

        public static bool EqualsIgnorCase(this string value, string Comparator) => value.Equals(Comparator, StringComparison.OrdinalIgnoreCase);

        public static IEnumerable<T> SafeConcat<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first == null || !first.Any())
            {
                return second ?? Enumerable.Empty<T>();
            }

            if (second == null || !second.Any())
            {
                return first;
            }

            return first.Concat(second);
        }

        public static string Decrypt(this string value)
        {
            if (value.IsEmpty())
            {
                return null;
            }

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("#8X$1(d*0&W_2@^+");
                aes.IV = new byte[16] { 145, 249, 126, 218, 15, 17, 5, 94, 92, 84, 10, 34, 14, 86, 197, 132 }; // IV should match the one used during encryption

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] encryptedPasswordBytes = Convert.FromBase64String(value);

                using (var ms = new System.IO.MemoryStream())
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(encryptedPasswordBytes, 0, encryptedPasswordBytes.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        public static string ConvertDateTimeToTime(this DateTime date) => date.ToString("HH:mm");
    }
}
