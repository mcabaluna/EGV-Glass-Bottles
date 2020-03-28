using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.DTO.Models
{
    [Table("IncomingLines")]
    public class IncomingLines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LineID { get; set; }
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public int DocNum { get; set; }
        public string InvType { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0.00}")]
        //public decimal GrossTotal { get; set; }
        public decimal DocTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:0,0.00}")]
        public decimal Collections { get; set; }
        [DisplayFormat(DataFormatString = "{0:0,0.00}")]
        public decimal Balance { get; set; }
        [DisplayFormat(DataFormatString = "{0:0,0.00}")]
        public decimal SumApplied { get; set; }
        public string InvoiceNo { get; set; }

    }
}
