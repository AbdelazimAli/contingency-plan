using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public  class PersonnelViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string LocalCurrCode { get; set; }
        //public string  LocalCurr { get; set; }
     //   public byte? MaxAge { get; set; } 
      //  public byte? MinAge { get; set; }
        public bool CodeReuse { get; set; } 
        public byte  GenEmpCode { get; set; }
        public byte GenWorkCode { get; set; }
        public byte GenAppCode { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public short? WorkHours { get; set; }
        public string ContractTempl { get; set; }
        public byte? Frequency { get; set; }
        public bool AutoEmployment { get; set; } 
        public bool AutoTermiation { get; set; }
        public byte EmploymentDoc { get; set; } = 0; 
        public byte JobDoc { get; set; } = 0; 
        public byte AssignFlex { get; set; } = 0;
        public byte? TermSysCode { get; set; }
        public byte? WorkServMethod { get; set; } 
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedTime { get; set; }
         public byte MaxPassTrials { get; set; }
        public int WaitingInMinutes { get; set; } = 5;
        public int? TaskPeriodId { get; set; }
        public int? BudgetPeriodId { get; set; }
        public int? QualGroupId { get; set; }

    }
}
