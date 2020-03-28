using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.VM.Portal
{
    public class SequenceLinesListVM
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// SeriesName
        /// </summary>
        public string SeriesName { get; set; }
        /// <summary>
        /// InitialNum
        /// </summary>
        public string InitialNum { get; set; }
        /// <summary>
        /// NextNumber
        /// </summary>
        public string NextNumber { get; set; }
        /// <summary>
        /// LastNumber
        /// </summary>
        public string LastNum { get; set; }
        /// <summary>
        /// BeginStr
        /// </summary>
        public string BeginStr { get; set; }
        /// <summary>
        /// EndStr
        /// </summary>
        public string EndStr { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; } 
        /// <summary>
        /// NumSize
        /// </summary>
        public int NumSize { get; set; }
        /// <summary>
        /// Lock
        /// </summary>
        public bool Lock { get; set; }
        /// <summary>
        /// BranchCode
        /// </summary>
        public string BranchCode { get; set; }
    }
}
