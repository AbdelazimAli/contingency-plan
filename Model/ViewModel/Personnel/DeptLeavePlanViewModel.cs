using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class DeptLeavePlanViewModel
    {
        public int Id { get; set; }
        public int DeptId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public float MinAllowPercent { get; set; }
        public byte Stars { get; set; }
        public int Dept { get; set; }
        public int JobId { get; set; }
    }

    public class DeptLeavePlVM
    {
        public IEnumerable<DeptLeavePlanViewModel> inserted { get; set; }
        public IEnumerable<DeptLeavePlanViewModel> updated { get; set; }
        public IEnumerable<DeptLeavePlanViewModel> deleted { get; set; }
    }
    public class DeptJobLvPlanViewModel
    {
        public int Id { get; set; }
        public int DeptPlanId { get; set; }
        public int JobId { get; set; }
        public float? MinAllowPercent { get; set; }
        public int DeptId { get; set; }
        public string Job { get; set; }
    }
    public class DeptPlanFormDaysViewModel
    {
        public int Id { get; set; }
        public byte Day { get; set; }
        public int Every { get; set; }
        public float? MinAllowPercent { get; set; }
        public byte? Stars { get; set; }
    }
}
