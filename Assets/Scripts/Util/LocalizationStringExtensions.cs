namespace Gamepackage
{
    public static class LocalizationStringExtensions
    {
        public static string Localize(this string input)
        {
            return Context.Localizer.LookupValueForLanguage(input);
        }
    }
}
