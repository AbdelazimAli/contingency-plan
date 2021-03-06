﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Notification
{
   public class MeetingViewModel
    {
        public int Id { get; set; }

        [MaxLength(500)]
        public string SubjectDesc { get; set; }
        public string MeetSubject { get; set; }
        public string Time { get; set; }
        public DateTime MeetDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        [MaxLength(250)]
        public string LocationText { get; set; }
        public int EmpId { get; set; } // Owner
        public byte Status { get; set; } // 1-Created  2-Modified  3-Canceled
        public string subjectText { get; set; }
        public string organiser { get; set; }
        public string IsActivate { get; set; }
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public List<int> MeetingAttendee { get; set; }
        public List<int> MeetingViewer { get; set; }
        public string AttUrl { get; set; }
        public int Attendes { get; set; }
        public bool IsOrganiser { get; set; }
        public bool CanUpload { get; set; }
        public bool CanCancel { get; set; }
    }
    public class MeetingAgendaViewModel
    {
        public int Id { get; set; }

        public string StartTime { get; set; }
        
        public string EndTime { get; set; }

        public DateTime? StartTime_DateTime
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(this.StartTime);
                }
                catch
                {
                    return null;
                }
            }
        }

        public DateTime? EndTime_DateTime
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(this.EndTime);
                }
                catch
                {
                    return null;
                }
            }
        }

        public string Time { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public int? EmpId { get; set; } // Owner
        public string EmpName { get; set; }
    }
    public class MeetingFormAgendaViewModel
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime? StartTime_DateTime
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(this.StartTime);
                }
                catch
                {
                    return null;
                }
            }
        }

        public DateTime? EndTime_DateTime
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(this.EndTime);
                }
                catch
                {
                    return null;
                }
            }
        }

        public string Time { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public int? EmpId { get; set; } // Owner
        public string EmpName { get; set; }
    }

    public class MeetingFormViewModel
    {

        public int Id { get; set; }
        [MaxLength(500)]
        public string SubjectDesc { get; set; }
        public string MeetSubject { get; set; }
        public string Time { get; set; }
        public DateTime MeetDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        [MaxLength(250)]
        public string LocationText { get; set; }
        public int EmpId { get; set; } // Owner
        public byte Status { get; set; } // 1-Created  2-Modified  3-Canceled
        public List<int> MeetingAttendee { get; set; }
        public List<string> MeetingAttendeeNames { get; set; }
        public List<int> MeetingViewer { get; set; }
        public List<string> MeetingViewerNames { get; set; }
        public int? BranchId { get; set; }
        public int? SiteId { get; set; }
        public byte LocationType { get; set; }
        public bool IsActivate { get; set; }
    }
    public class SaveMeetingViewModel
    {

        public int Id { get; set; }
        [MaxLength(500)]
        public string SubjectDesc { get; set; }
        public short MeetSubject { get; set; }
        public string Time { get; set; }
        public DateTime MeetDate { get; set; }
        public DateTime StartTime { get; set; }
        public int MyProperty { get; set; }
        public DateTime EndTime { get; set; }
        [MaxLength(250)]
        public string LocationText { get; set; }
        public int EmpId { get; set; } // Owner
        public byte Status { get; set; } // 1-Created  2-Modified  3-Canceled                                 
        public IEnumerable<string> MeetingAttendee { get; set; }
        public IEnumerable<string> MeetingViewer { get; set; }
        public int? BranchId { get; set; }
        public int? SiteId { get; set; }
        public byte LocationType { get; set; }
        public bool Activate { get; set; }
        public bool IsActivate { get; set; }
        public string subjectText { get; set; }
        public string organiser { get; set; }
        public string CompanyLocation { get; set; }
        public string SiteLocation { get; set; }
    }
}
