using Model.Domain;
using Model.ViewModel.Notification;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
    public interface IMeetingRepository : IRepository<Meeting>
    {
        IList<Schedual> GetEmployeeSchedual(string commaSeperatedEmpIds, DateTime starttime, DateTime endtime, string source, int id, string lang);
        IQueryable<MeetingViewModel> GetMeetings(int EmpId, byte Range, DateTime? Start, DateTime? End, string culture, int CompanyId, DateTime currentTime);
        MeetingFormViewModel ReadMeeting(int Id ,string culture);
        List<FormList> GetMeetingAttendee(int Id, string culture);
        IQueryable<MeetingAgendaViewModel> GetAgenda(int Id, string culture);
        List<FormDropDown> GetUsersLang(string EmpIds);
        void Add(MeetViewer viewer);
        void Remove(MeetViewer viewer);
        void Add(MeetAttendee meetAttendee);
        DbEntityEntry<MeetSchedual> Entry(MeetSchedual Agenda);
        void Add(MeetSchedual Agenda);
        void Attach(MeetSchedual Agenda);
        void Remove(MeetSchedual Agenda);
        void Remove(MeetAttendee attendee);
        void AddNotifyLetter(NotifyLetter notify);
        void AttachNotifyLetter(NotifyLetter notify);
        void Attach(NotifyLetter notify);
        DbEntityEntry<NotifyLetter> Entry(NotifyLetter notify);
        void Remove(NotifyLetter notify);
        Meeting GetMeeting(int? id);

    }
}
