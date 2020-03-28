using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.VM.Portal
{
    public class VatListVM
    {
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Percentage
        /// </summary>
        public decimal Percentage { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// EffectiveFrom
        /// </summary>
        public DateTime EffectiveFrom { get; set; }
        /// <summary>
        /// EffectiveTo
        /// </summary>
        public DateTime EffectiveTo { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// CreatedById
        /// </summary>
        public string CreatedById { get; set; }
        /// <summary>
        /// ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }
        /// <summary>
        /// ModifiedById
        /// </summary>
        public string ModifiedById { get; set; }
    }
}
