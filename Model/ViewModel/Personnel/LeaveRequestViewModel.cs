using Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class LeaveRequestViewModel
    {
        public int Id { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Today;
        public string Employee { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; } // Apply Eligiblity criteria & Conditions
        public byte ReqStatus { get; set; } // 1- Planned   2-Confirmed   from hr always 2
        public byte ApprovalStatus { get; set; } // 1- New  2-Cancelled 4-Rejected  5-Approved
        public string ApprovalName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public float NofDays { get; set; }
        public string RoleId { get; set; }
        public int? PositionId { get; set; }
        public int? DeptId { get; set; }
        public int? EmpId { get; set; }
        public string FlowWork { get; set; }
        public int CompanyId { get; set; }
        public int? BranchId { get; set; } // Branch = 2
        public int? SectorId { get; set; } // Sector = 3
        public int? AuthBranch { get; set; } // Branch = 2
        public int? AuthDept { get; set; }
        public int? AuthPosition { get; set; }
        public int? AuthEmp { get; set; }
        public string AuthEmpName { get; set; }
        public string AuthPosName { get; set; }
        public string AuthDeptName { get; set; }
        public bool HasImage { get; set; }
        public DateTime EndDate { get; set; }
        public int? ReplaceEmpId { get; set; }
        //last Add Property by Abdelazim 
        public short? ReqReason { get; set; } // lookup code LeaveReason
        [MaxLength(250)]
        public string ReasonDesc { get; set; }
        public short? RejectReason { get; set; } // lookup code RejectReason
        [MaxLength(250)]
        public string RejectDesc { get; set; }
        public short? CancelReason { get; set; } // lookup code CancelReason
        public string CancelDesc { get; set; }

        public string Notes { get; set; }
        public DateTime? WorkflowTime { get; set; }

        public DateTime? ActualEndDate { get; set; }
        public float? ActualNofDays { get; set; }
        public byte DayFraction { get; set; } = 0;
        public string AttUrl { get; set; }
    }

    public class LeaveReqViewModel
    {
        public int Id { get; set; }
        public DateTime? RequestDate { get; set; } = DateTime.Today;
        public int EmpId { get; set; }
        public string Employee { get; set; }
        public string LeaveType { get; set; }
        public int TypeId { get; set; }
        public int PeriodId { get; set; }
        public short AbsenceType { get; set; }

        public byte ReqStatus { get; set; } = 2; // 1- Planned   2-Confirmed   from hr always 2
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Review 4-Cancel before accepted 5-Rejected  6-Accepted  7-Cancel after accepted
        public string Approval { get; set; }
        public bool IsDeleted { get; set; } = false;
        public short? ReqReason { get; set; } // lookup code LeaveReason

        [MaxLength(250)]
        public string ReasonDesc { get; set; }

        public short? RejectReason { get; set; } // lookup code RejectReason

        [MaxLength(250)]
        public string RejectDesc { get; set; }
        public short? CancelReason { get; set; } // lookup code CancelReason
        public string CancelDesc { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? ReturnTime { get; set; }

        public DateTime? ActualStartDate { get; set; }

        public DateTime? ActualEndDate { get; set; }

        public DateTime? ReturnDate { get; set; }
        public float NofDays { get; set; } = 0;
        public byte DayFraction { get; set; } = 0;
        public float? BalanceBefore { get; set; }
        public float? ActualNofDays { get; set; }
        public int? ReplaceEmpId { get; set; }
        public int? AuthbyEmpId { get; set; }
        //public short? StartTolerance { get; set; } = 0; // leave request start tolerance

        //Validation 
        public float? AllowedDays { get; set; }
        public float? ReservedDays { get; set; }
        public float? BalBefore { get; set; }
        public float? BalAfter { get; set; }

        public bool submit { get; set; }
        public bool AllowNegBal { get; set; } = false;
        public float? Percentage { get; set; }
        public short? MaxDaysInPeriod { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int Attachments { get; set; }
        public string Period { get; set; }
        public byte? Stars { get; set; }
        public string Departments { get; set; }
        public int CompanyId { get; set; }
    }

    public class ReqDaysParamVM
    {
        public int EmpId { get; set; }
        public int RequestId { get; set; }
        public LeaveType type { get; set; }
        public DateTime StartDate { get; set; }
        public string culture { get; set; }
    }

    public class RequestValidationViewModel
    {
        public byte? WaitingMonth { get; set; }
        public byte? MaxDays { get; set; }
        public short? MaxDaysInPeriod { get; set; }
        public bool ExDayOff { get; set; }
        public bool ExHolidays { get; set; }

        public int EmpStars { get; set; }
        public int Stars { get; set; } = 0;

        public float? AllowedDays { get; set; }
        public float? ReservedDays { get; set; }
        public float? BalBefore { get; set; }
        public float BalAfter { get; set; }

        public string Message { get; set; }
        public string Warning { get; set; }
        public int EmpId { get; set; }
        public byte AbsenceType { get; set; }
    }

    public class LeavePlanStarsVM
    {
        public byte Stars { get; set; }
        public int EmpStars { get; set; }
        public int DeptId { get; set; }
        public int JobId { get; set; }
        public List<DeptLeavePlanViewModel> Plans { get; set; }

    }

    public class CheckLeavePlanVM
    {
        List<string> errorMsg { get; set; }
        public int Stars { get; set; }
        public int EmpStars { get; set; }
        public float MinAllowPercent { get; set; }
    }

    public class GetStarsParamVM {
        public int EmpId { get; set; }
        public int RequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ExHolidays { get; set; }
        public bool ExDayOff { get; set; }
        public int ComapnyId { get; set; }
        public int PeriodId { get; set; }
    }


    public class GroupLeaveViewModel
    {
        public int Id { get; set; }
        public string Employee { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public bool Approve { get; set; }
        public int NofErrors { get; set; }
        public byte ReasonCode { get; set; } //0. success, 1.have Request, 2.balance, 3.balance & have Request
        public bool Success { get; set; }
        public int EmpId { get; set; }
    }

    public class LeaveOpViewModel
    {
        public int Id { get; set; }
        public DateTime? RequestDate { get; set; } = DateTime.Today;
        public int EmpId { get; set; }
        public string Employee { get; set; }
        public string LeaveType { get; set; }
        public int TypeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public float? NofDays { get; set; }

        public byte btn { get; set; }

        public int Attachments { get; set; }
        public byte? Stars { get; set; }
        public int CompanyId { get; set; }
        public int PeriodId { get; set; }


        //Edit
        public DateTime? EditedStartDate { get; set; }
        public DateTime? EditedEndDate { get; set; }

        //Cancel
        public short? CancelReason { get; set; } // lookup code CancelReason
        public string CancelDesc { get; set; }
        public float? BalAfterCancel { get; set; }

        //Break
        public DateTime? BreakEndDate { get; set; }
        public DateTime? BreakReturnDate { get; set; }
        public float? BreakNofDays { get; set; }
        public float? BreakBalDays { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public bool AllowNegBal { get; set; }
        public short AbsenceType { get; set; }
        public short? MaxDaysInPeriod { get; set; }
        public float? Percentage { get; set; }
        public float Balance { get; set; }
        //public short? StartTolerance { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public Period Period { get; set; }
    }

    #region API
    public class ValidationMessages
    {
        public string AssignError { get; set; }
        public string IsReplacementError { get; set; }
        public string ReplacementError { get; set; }
        public string WaitingError { get; set; }
        public DateTime WaitingMonth { get; set; }
        public string HasRequestError { get; set; }
        public string StarsError { get; set; }
        public int Stars { get; set; }
        public string DeptPercentError { get; set; }
        public float Percentage { get; set; }
        public bool IsError { get; set; }
    }
    #endregion
}