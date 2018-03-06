using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class LoanInstall
    {
        public int Id { get; set; }
        public LoanRequest Request { get; set; }
        public int RequestId { get; set; }
        public short InstallNo { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime InstallDate { get; set; }
        public decimal InstallAmt { get; set; }
        public decimal? PaidAmt { get; set; }
        public bool IsPaid { get; set; }
    }
}
