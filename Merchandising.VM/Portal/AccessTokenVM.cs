using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.VM.Portal
{
   public  class AccessTokenVM
    {
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
        public long expires_in { get; set; }
    }
}
