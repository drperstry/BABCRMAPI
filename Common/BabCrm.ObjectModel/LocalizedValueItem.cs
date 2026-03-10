using System;
using System.Text;

namespace BabCrm.ObjectModel
{
    public sealed class LocalizedValueItem<T>
    {
        public LocalizedValueItem(string lang, T value)
        {
            Lang = lang ?? throw new System.ArgumentNullException(nameof(lang));
            Value = value;
        }

        public string Lang { get; }

        public T Value { get; }
    }
}
