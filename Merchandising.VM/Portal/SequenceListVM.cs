using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.VM.Portal
{
    public class SequenceListVM
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ObjectCode
        /// </summary>
        public int ObjectCode { get; set; }
        /// <summary>
        /// Document
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// DocumentType
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// DocSubType
        /// </summary>
        public string DocSubType { get; set; }
        /// <summary>
        /// Series
        /// </summary>
        public int Series { get; set; }
        /// <summary>
        /// SeriesName
        /// </summary>
        public string DefaultSeries { get; set; }      
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
    }
}
