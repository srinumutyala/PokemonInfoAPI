using System.Text.RegularExpressions;

namespace PokemonInfo.Services
{
    public static class TextSanitizer
    {
        public static string  Sanitize(this string text)
        {
            return !string.IsNullOrEmpty(text) ? Regex.Replace(text, @"\t|\n|\r|\f", " ") : text;
        }
    }
}
