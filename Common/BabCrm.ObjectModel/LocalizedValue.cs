using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace BabCrm.ObjectModel
{
    public sealed class LocalizedValue<T> : IEnumerable<LocalizedValueItem<T>>
    {
        private readonly Dictionary<string, LocalizedValueItem<T>> items = new Dictionary<string, LocalizedValueItem<T>>();

        public LocalizedValue()
        { }

        public IEnumerator<LocalizedValueItem<T>> GetEnumerator()
        {
            return this.items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.items.Values.GetEnumerator();
        }

        public LocalizedValueItem<T> this[string lang]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(lang))
                {
                    throw new System.ArgumentException($"'{nameof(lang)}' cannot be null or whitespace.", nameof(lang));
                }

                return this.items[lang];
            }
        }

        public LocalizedValueItem<T> this[CultureInfo cultureInfo]
        {
            get
            {
                if (cultureInfo == null)
                {
                    throw new System.ArgumentNullException(nameof(cultureInfo));
                }

                if (this.items.TryGetValue(cultureInfo.TwoLetterISOLanguageName, out var item))
                {
                    return item;
                }

                return null;
            }
        }

        public LocalizedValue<T> AddValueAr(T value)
        {
            return this.AddValue("ar", value);
        }

        public LocalizedValue<T> AddValueEn(T value)
        {
            return this.AddValue("en", value);
        }

        public LocalizedValue<T> AddValue(string lang, T value)
        {
            if (string.IsNullOrWhiteSpace(lang))
            {
                throw new System.ArgumentException($"'{nameof(lang)}' cannot be null or whitespace.", nameof(lang));
            }

            if (this.items.ContainsKey(lang))
            {
                throw new System.ArgumentException("Duplicate key", nameof(lang));
            }

            this.items[lang] = new LocalizedValueItem<T>(lang, value);

            return this;
        }
    }
}
