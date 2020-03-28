﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.VM.Portal
{
    public class PaymentTermsListVM
    {
        /// <summary>
        /// TermId
        /// </summary>
        public string TermId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// NoOfDays
        /// </summary>
        public int NoOfDays { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public bool Status { get; set; }
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
