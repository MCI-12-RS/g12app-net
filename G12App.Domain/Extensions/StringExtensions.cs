using System.Linq;

namespace G12App.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string FormatFirstLastName(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            var names = name.Trim().Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
            return string.Concat(names.FirstOrDefault(), " ", names.LastOrDefault());
        }
    }
}
