namespace Merchandising.Portal.Models
{
    /// <summary>
    /// StringExtension
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// RemoveLastChars
        /// </summary>
        /// <param name="name"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RemoveLastChars(this string name, int length = 1)
        {
            name = name.Remove(name.Length - length);

            return name;
        }
    }
}