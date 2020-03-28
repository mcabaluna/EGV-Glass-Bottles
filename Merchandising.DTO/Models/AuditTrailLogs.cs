using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    /// <summary>
    /// AuditTrailLogs
    /// </summary>
    [Table("AuditTrailLogs")]
    public class AuditTrailLogs
    {
        /// <summary>
        /// AuditId
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuditId { get; set; }
        /// <summary>
        /// Document
        /// </summary>
        public string Document { get; set; }
        /// <summary>
        /// Mode
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// UpdatedBy
        /// </summary>
        public string UpdatedBy { get; set; }
        /// <summary>
        /// UpdatedTime
        /// </summary>
        public DateTime UpdatedTime { get; set; }
        /// <summary>
        /// Branch
        /// </summary>
        public string Branch { get; set; }
        /// <summary>
        /// ComputerName
        /// </summary>
        public string ComputerName { get; set; }
        /// <summary>
        /// IpAddress
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// ReferenceNo
        /// </summary>
        public string ReferenceNo { get; set; }
    }
}