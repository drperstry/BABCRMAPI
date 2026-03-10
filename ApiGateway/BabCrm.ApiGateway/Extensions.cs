using BabCrm.Service.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Globalization;

namespace BabCrm.ApiGateway
{
    public static class Extensions
    {
        public static LookupModel ToModel(this BaseLookupModel lookup, CultureInfo cultureInfo)
        {
            return new LookupModel()
            {
                Id = lookup.Id,
                Name = lookup.Name[cultureInfo].Value,
                Code = lookup.Code
            };
        }
    }
}
