namespace Merchandising.Portal.Models
{
    using Merchandising.DTO;

    /// <summary>
    /// Token
    /// </summary>
    public class Token
    {
        /// <summary>
        /// error
        /// </summary>
        public string error { get; set; }

        /// <summary>
        /// error_description
        /// </summary>
        public string error_description { get; set; }

        /// <summary>
        /// access_token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// token_type
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        /// expires_in
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// CurrentUser
        /// </summary>
        //public CurrentUser CurrentUser { get; set; }
    }
}