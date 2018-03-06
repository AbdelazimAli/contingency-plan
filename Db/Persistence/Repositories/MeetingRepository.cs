using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel.Notification;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Persistence.Repositories
{


    public class MeetingRepository : Repository<Meeting>, IMeetingRepository
    {

        public MeetingRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }


        #region Meetings
        /// <summary>
        /// return schedual for input employees search in Meeting "M", Approved Errands "E", and Approved leave Request "L"
        /// Source    EmpId   StartTime     EndTime
        /// M, E, L   15      15:00         17:00
        /// if time is zero or null means employee is busy overall  day
        /// </summary>
        /// <param name="commaSeperatedEmpIds">15,1050,1051  comma seperated employees Id</param>
        /// <param name="startime">Input date auto truncate time</param>
        /// <returns>IQueryable<EmpSchedual> can be extended</returns>
        public IList<Schedual> GetEmployeeSchedual(string commaSeperatedEmpIds, DateTime startime, DateTime endtime, string source, int sourceid, string lang)
        {
            string fromtime = startime.ToString("yyyy-MM-dd HH:mm");
            string totime = (endtime.Hour == 0 ? endtime.AddDays(1).AddMinutes(-1) : endtime).ToString("yyyy-MM-dd HH:mm");
            if (string.IsNullOrEmpty(commaSeperatedEmpIds)) return new List<Schedual>();
            string meeting = (source == "Meeting" ? " M.Id <> " + sourceid + " And" : "");
            string errand = (source == "Errand" ? " ER.Id <> " + sourceid + " And" : "");
            string leave = (source == "Leave" ? " LR.Id <> " + sourceid + " And" : "");
            string assignorder = (source == "AssignOrder" ? " AO.Id <> " + sourceid + " And" : "");
            var query = context.Database.SqlQuery<EmpSchedual>($"Select T.[Source] Source, P.Id EmpId, (CASE WHEN T.Organizer IS NOT NULL THEN (Select dbo.fn_TrlsName(IsNull(E.Title,'')+' '+E.FirstName+' '+E.Familyname, '{lang}') From People E Where E.Id = T.Organizer) END) Organizer, dbo.fn_TrlsName(IsNull(P.Title,'')+' '+P.FirstName+' '+P.Familyname, '{lang}') EmpName, T.StartTime StartTime, T.EndTime EndTime, SourceName, SourceType, LocatName, T.MultiDays  From People P left outer join (Select 'Meeting' [Source], MA.EmpId, M.EmpId Organizer ,M.StartTime StartTime, M.EndTime EndTime, SubjectDesc SourceName, dbo.fn_GetLookUpCode('MeetSubject', MeetSubject, '{lang}') SourceType, ISNULL(LocationText, (CASE WHEN SiteId IS NULL THEN (Select dbo.fn_TrlsName(B.Name, '{lang}') from Branches B Where B.Id = BranchId) ELSE (Select dbo.fn_TrlsName(S.Name, '{lang}') from Sites S Where S.Id = SiteId) END)) LocatName, 0 MultiDays From Meetings M, MeetAttendees MA Where M.Id <> 0 And M.Id = MA.MeetingId And ((M.StartTime Between '{fromtime}' And '{totime}') OR (M.EndTime Between '{fromtime}' And '{totime}')) And MA.EmpId In ({commaSeperatedEmpIds}) And M.IsActivate = 1 And M.Status !=3 UNION ALL Select 'Errand', ER.EmpId, null, ER.StartDate StartTime, ER.EndDate EndTime,[Subject] SourceName, null, (Select dbo.fn_TrlsName(LS.Name, '{lang}') from Sites LS Where LS.Id = SiteId) LocatName, ER.MultiDays From ErrandRequests ER Where ER.EmpId In ({commaSeperatedEmpIds}) And ((ER.StartDate Between '{fromtime}' And '{totime}') OR (ER.EndDate Between '{fromtime}' And '{totime}')) And ER.ApprovalStatus = 6 UNION ALL Select 'Leave', LR.EmpId, null, LR.ActualStartDate, LR.ActualEndDate,(Select dbo.fn_TrlsName(LT.Name, '{lang}') from LeaveTypes LT where LT.Id = TypeId) SourceName, null,null,0 From LeaveRequests LR Where LR.EmpId In ({commaSeperatedEmpIds}) And ((LR.ActualStartDate Between '{fromtime}' And '{totime}') OR (LR.ActualEndDate Between '{fromtime}' And '{totime}')) And LR.ApprovalStatus = 6 And LR.DayFraction = 0 UNION ALL Select 'AssignOrder', AO.EmpId, null, AO.AssignDate StartTime, AO.AssignDate EndTime,null,null,null,0 From AssignOrders AO Where  AO.EmpId In ({commaSeperatedEmpIds}) And (AO.AssignDate Between '{fromtime}' And '{totime}') And AO.ApprovalStatus = 6 And AO.Duration = 1) T on T.EmpId = P.Id Where P.Id In ({commaSeperatedEmpIds})").ToList();
            if (query.Count == 0) return new List<Schedual>();

            return query.Select(a => new { id = a.EmpId, name = a.EmpName }).Distinct().Select(b => new Schedual { EmpName = b.name, Tasks = query.Where(a => a.EmpId == b.id).ToList() }).ToList();
        }

        public IQueryable<MeetingViewModel> GetMeetings(int EmpId, byte Range, DateTime? Start, DateTime? End, string culture, int CompanyId, DateTime currentTime)
        {
            //10- All, 0-Custom
            if (Range != 10 && Range != 0) RequestRangeFilter(Range, CompanyId, out Start, out End);

            // Organizer
            var query = (from m in context.Meetings
                         where m.CompanyId == CompanyId && m.EmpId == EmpId && m.Status != 3
                         join P in context.People on m.EmpId equals P.Id
                         select new MeetingViewModel
                         {
                             Id = m.Id,
                             MeetDate = m.MeetDate,
                             Status = m.Status,
                             StartTime = m.StartTime.ToString(),
                             EndTime = m.EndTime.ToString(),
                             MeetSubject = HrContext.GetLookUpCode("MeetSubject", m.MeetSubject, culture),
                             EmpId = m.EmpId,
                             organiser = HrContext.TrlsName(P.Title + " " + P.FirstName + " " + P.Familyname, culture),
                             IsActivate = m.IsActivate == true ? "active" : "inactive",
                             SubjectDesc = m.SubjectDesc,
                             AttUrl = HrContext.GetDoc("Meeting", m.Id),
                             IsOrganiser = true,
                         }).Union(from m in context.Meetings
                                  where m.CompanyId == CompanyId && m.Status != 3
                                  join ma in context.MeetAttendees on m.Id equals ma.MeetingId
                                  where ma.EmpId == EmpId && m.EmpId != EmpId
                                  join P in context.People on m.EmpId equals P.Id
                                  select new MeetingViewModel
                                  {
                                      Id = m.Id,
                                      MeetDate = m.MeetDate,
                                      Status = m.Status,
                                      StartTime = m.StartTime.ToString(),
                                      EndTime = m.EndTime.ToString(),
                                      MeetSubject = HrContext.GetLookUpCode("MeetSubject", m.MeetSubject, culture),
                                      EmpId = m.EmpId,
                                      organiser = HrContext.TrlsName(P.Title + " " + P.FirstName + " " + P.Familyname, culture),
                                      IsActivate = m.IsActivate == true ? "active" : "inactive",
                                      SubjectDesc = m.SubjectDesc,
                                      AttUrl = null,
                                      IsOrganiser = false,
                                  }).Union(from m in context.Meetings
                                           where m.CompanyId == CompanyId && m.Status != 3
                                           join ma in context.MeetViewers on m.Id equals ma.MeetingId
                                           where ma.EmpId == EmpId && m.EmpId != EmpId
                                           join P in context.People on m.EmpId equals P.Id
                                           select new MeetingViewModel
                                           {
                                               Id = m.Id,
                                               MeetDate = m.MeetDate,
                                               Status = m.Status,
                                               StartTime = m.StartTime.ToString(),
                                               EndTime = m.EndTime.ToString(),
                                               MeetSubject = HrContext.GetLookUpCode("MeetSubject", m.MeetSubject, culture),
                                               EmpId = m.EmpId,
                                               organiser = HrContext.TrlsName(P.Title + " " + P.FirstName + " " + P.Familyname, culture),
                                               IsActivate = m.IsActivate == true ? "active" : "inactive",
                                               SubjectDesc = m.SubjectDesc,
                                               AttUrl = null,
                                               IsOrganiser = false,
                                           }).ToList().Select(l => new MeetingViewModel
                                           {
                                               Id = l.Id,
                                               StartTime = Convert.ToDateTime(l.StartTime).ToString("hh:mm tt"),
                                               EndTime = Convert.ToDateTime(l.EndTime).ToString("hh:mm tt"),
                                               Status = l.Status,
                                               MeetDate = l.MeetDate,
                                               SubjectDesc = l.SubjectDesc,
                                               MeetSubject = l.MeetSubject,
                                               Time = (Convert.ToDateTime(l.EndTime) - Convert.ToDateTime(l.StartTime)).ToString(@"hh\:mm"),
                                               EmpId = l.EmpId,
                                               organiser = l.organiser,
                                               IsActivate = l.IsActivate,
                                               AttUrl = l.AttUrl,
                                               Attendes = l.Attendes,
                                               IsOrganiser = l.IsOrganiser,
                                               CanUpload = (currentTime.Date > l.MeetDate.Date || (currentTime.Date == l.MeetDate.Date && Convert.ToDateTime(l.EndTime).TimeOfDay < currentTime.TimeOfDay)) ? true : false,
                                               CanCancel = (currentTime.Date > l.MeetDate.Date || (currentTime.Date == l.MeetDate.Date && Convert.ToDateTime(l.StartTime).TimeOfDay <= currentTime.TimeOfDay)) ? true : false
                                           });
            if (Range != 10)
                query = query.Where(c => Start <= c.MeetDate && End >= c.MeetDate);

            return query.OrderByDescending(a => a.MeetDate).AsQueryable();
        }

        public MeetingFormViewModel ReadMeeting(int Id,string culture)
        {
            var record = (from m in context.Meetings
                          where m.Id == Id
                          select new MeetingFormViewModel
                          {
                              Id = m.Id,
                              MeetDate = m.MeetDate,
                              SubjectDesc = m.SubjectDesc,
                              StartTime = m.StartTime.ToString(),
                              EndTime = m.EndTime.ToString(),
                              MeetSubject = m.MeetSubject.ToString(),
                              MeetingAttendee = context.MeetAttendees.Where(a => a.MeetingId == Id).Select(s => s.EmpId).ToList(),
                              MeetingAttendeeNames = context.MeetAttendees.Where(a => a.MeetingId == m.Id).Select(s => HrContext.TrlsName(s.Attendee.Title + " " + s.Attendee.FirstName + " " + s.Attendee.Familyname, culture)).ToList(),
                              MeetingViewer = context.MeetViewers.Where(a => a.MeetingId == Id).Select(s => s.EmpId).ToList(),
                              MeetingViewerNames = context.MeetViewers.Where(a => a.MeetingId == m.Id).Select(s => HrContext.TrlsName(s.Viewer.Title + " " + s.Viewer.FirstName + " " + s.Viewer.Familyname, culture)).ToList(),
                              EmpId = m.EmpId,
                              LocationText = m.LocationText,
                              BranchId = m.BranchId,
                              LocationType = m.LocationType,
                              IsActivate = m.IsActivate,
                              SiteId = m.SiteId
                          }).FirstOrDefault();

            record.StartTime = DateTime.Parse(record.StartTime).ToString("HH:mm");
            record.EndTime = DateTime.Parse(record.EndTime).ToString("HH:mm");
            return record;
        }
        //GetMeetingAttendee
        public List<FormList> GetMeetingAttendee(int Id, string culture)
        {
            var query = (from m in context.MeetAttendees
                         where m.MeetingId == Id
                         select new FormList
                         {
                             id = m.EmpId,
                             name = HrContext.TrlsName(m.Attendee.Title + " " + m.Attendee.FirstName + " " + m.Attendee.Familyname, culture)
                         }).ToList();

            return query;

        }
        public List<FormList> GetMeetingViewer(int Id, string culture)
        {
            var query = (from m in context.MeetViewers
                         where m.MeetingId == Id
                         select new FormList
                         {
                             id = m.EmpId,
                             name = HrContext.TrlsName(m.Viewer.Title + " " + m.Viewer.FirstName + " " + m.Viewer.Familyname, culture)
                         }).ToList();

            return query;

        }

        public IQueryable<MeetingAgendaViewModel> GetAgenda(int Id, string culture)
        {
            var query = (from m in context.MeetScheduals
                         where m.MeetingId == Id
                         join p in context.People on m.EmpId equals p.Id into j
                         from x in j.DefaultIfEmpty()
                         select new MeetingAgendaViewModel
                         {
                             Id = m.Id,
                             StartTime = m.StartTime.ToString(),
                             EndTime = m.EndTime.ToString(),
                             Description = m.Description,
                             EmpId = m.EmpId,
                             EmpName = HrContext.TrlsName(x.Title + " " + x.FirstName + " " + x.Familyname, culture),
                         }).ToList().Select(l => new MeetingAgendaViewModel
                         {
                             Id = l.Id,
                             EmpId = l.EmpId,
                             StartTime = Convert.ToDateTime(l.StartTime).ToString("hh:mm tt"),
                             EndTime = Convert.ToDateTime(l.EndTime).ToString("hh:mm tt"),
                             Description = l.Description,
                             Time = (Convert.ToDateTime(l.EndTime) - Convert.ToDateTime(l.StartTime)).ToString(@"hh\:mm")
                         });
            return query.AsQueryable();
        }

        public List<FormDropDown> GetUsersLang(string EmpIds)
        {
            try
            {
                return context.Database.SqlQuery<FormDropDown>("select DISTINCT EmpId id, [Language] name from AspNetUsers a where  a.EmpId in (" + EmpIds + ")").ToList();
            }
            catch
            {
                return new List<FormDropDown>();
            }
        }

        #endregion


        public void Add(MeetViewer viewer)
        {
            context.MeetViewers.Add(viewer);
        }
        public void Remove(MeetViewer viewer)
        {
            if (Context.Entry(viewer).State == EntityState.Detached)
            {
                context.MeetViewers.Attach(viewer);
            }
            context.MeetViewers.Remove(viewer);
        }
      
        public void Add(MeetAttendee meetAttendee)
        {
            context.MeetAttendees.Add(meetAttendee);
        }
       
        public DbEntityEntry<MeetSchedual> Entry(MeetSchedual Agenda)
        {
            return Context.Entry(Agenda);
        }
        public void Add(MeetSchedual Agenda)
        {
            context.MeetScheduals.Add(Agenda);
        }
        public void Attach(MeetSchedual Agenda)
        {
            context.MeetScheduals.Attach(Agenda);
        }
        public void Remove(MeetSchedual Agenda)
        {
            if (Context.Entry(Agenda).State == EntityState.Detached)
            {
                context.MeetScheduals.Attach(Agenda);
            }
            context.MeetScheduals.Remove(Agenda);
        }
     
        public void Remove(MeetAttendee attendee)
        {
            if (Context.Entry(attendee).State == EntityState.Detached)
            {
                context.MeetAttendees.Attach(attendee);
            }
            context.MeetAttendees.Remove(attendee);
        }
        public DbEntityEntry<Meeting> Entry(Meeting meeting)
        {
            return Context.Entry(meeting);
        }
        public void AddNotifyLetter(NotifyLetter notify)
        {
            context.NotifyLetters.Add(notify);
        }
        public void AttachNotifyLetter(NotifyLetter notify)
        {
            context.NotifyLetters.Attach(notify);
        }
        public void Attach(NotifyLetter notify)
        {
            context.NotifyLetters.Attach(notify);
        }
        public DbEntityEntry<NotifyLetter> Entry(NotifyLetter notify)
        {
            return Context.Entry(notify);
        }
        public void Remove(NotifyLetter notify)
        {
            if (Context.Entry(notify).State == EntityState.Detached)
            {
                context.NotifyLetters.Attach(notify);
            }
            context.NotifyLetters.Remove(notify);
        }
        public Meeting GetMeeting(int? id)
        {
            return Context.Set<Meeting>().Find(id);
        }

    }
}
