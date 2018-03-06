using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class ErrandRequest
    {
        public int Id { get; set; }
        [Index("IX_ErrandRequest", Order = 1)]
        public int EmpId { get; set; }
        public Person Emp { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int SiteId { get; set; }
        public Site Site { get; set; }

        [MaxLength(500)]
        public string Subject { get; set; }
        public bool MultiDays { get; set; } // Multiple Days

        //[DataType(DataType.Date), Column(TypeName = "Date")]
        [Index("IX_ErrandRequest", Order = 2)]
        public DateTime StartDate { get; set; }

        //[DataType(DataType.Date), Column(TypeName = "Date")]
        [Index("IX_ErrandRequest", Order = 3)]
        public DateTime EndDate { get; set; }
        public int? ManagerId { get; set; }
        public Person Manager { get; set; }
        public byte ErrandType { get; set; }

        [MaxLength(500)]
        public string Reason { get; set; }

        [Index("IX_ErrandRequest", Order = 4)]
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected 

        public decimal Expenses { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
