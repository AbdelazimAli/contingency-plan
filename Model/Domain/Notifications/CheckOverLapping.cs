using Model.ViewModel;
using Model.ViewModel.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Notifications
{
    public class CheckOverLapping
    {
        List<MeetingAgendaViewModel> OldRecords;
        RequestAgendaGrid grid1;
        DateTime From, To;
        public CheckOverLapping(List<MeetingAgendaViewModel> _OldRecords, RequestAgendaGrid _grid1,DateTime _From,DateTime _To)
        {
            this.OldRecords = _OldRecords;
            this.grid1 = _grid1;

            grid1.inserted = (grid1.inserted == null) ? new List<MeetingAgendaViewModel>() : grid1.inserted.ToList();
            grid1.updated = (grid1.updated == null) ? new List<MeetingAgendaViewModel>() : grid1.updated.ToList();
            grid1.deleted = (grid1.deleted == null) ? new List<MeetingAgendaViewModel>() : grid1.deleted.ToList();

            this.From = _From;
            this.To= _To;
        }
        public bool Run(out List<string> ErrorMessages, string trls ,string cult,string trlsTo,string trlsOutMeeting,string trlsInvalid)
        {
            ErrorMessages = new List<string>();
            List<MeetingAgendaViewModel> CompleteList = grid1.inserted.Concat(grid1.updated).ToList().OrderByDescending(m => m.Id).ToList();
            CheckOverLapping_Delete();
            return CheckOverLapping_FinalCheck(CompleteList, out ErrorMessages,trls,cult,trlsTo,trlsOutMeeting,trlsInvalid);
        }
        public bool CheckOverLapping_FinalCheck(List<MeetingAgendaViewModel> CompleteList, out List<string> ErrorMessages,string trls,string cult,string trlsTo,string trlsOutMeeting,string trlsInvalid)
        {
            bool IsValidAllRecords = true;
            ErrorMessages = new List<string>();
            foreach (MeetingAgendaViewModel CompleteRecord in CompleteList)
            {
                if(CompleteRecord.StartTime_DateTime == CompleteRecord.EndTime_DateTime)
                {
                    IsValidAllRecords = false;
                    ErrorMessages.Add(CompleteRecord.StartTime_DateTime.Value.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult))+" "+trls+""+ CompleteRecord.EndTime_DateTime.Value.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult))+" "+trlsInvalid );
                    continue;
                }

                if (Convert.ToDateTime(CompleteRecord.StartTime_DateTime)<From || Convert.ToDateTime(CompleteRecord.StartTime_DateTime)>To|| Convert.ToDateTime(CompleteRecord.EndTime_DateTime) < From || Convert.ToDateTime(CompleteRecord.EndTime_DateTime) > To)
                {
                    IsValidAllRecords = false;
                    ErrorMessages.Add(CompleteRecord.StartTime_DateTime.Value.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult))+" "+trlsTo+ CompleteRecord.EndTime_DateTime.Value.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) +" "+ trlsOutMeeting);
                        //string.Format("{0} to {1} Is out of Meeting time", CompleteRecord.StartTime_DateTime, CompleteRecord.EndTime_DateTime));
                    continue;
                }

                MeetingAgendaViewModel OverlappedWith = null;
                bool IsValidRecord;
                if (CompleteRecord.Id > 0)
                    IsValidRecord = IsValid_UpdateList(/*ref OldRecords,*/ CompleteRecord, false, out OverlappedWith);
                else
                    IsValidRecord = IsValid_UpdateList(/*ref OldRecords,*/ CompleteRecord, true, out OverlappedWith);

                IsValidAllRecords = IsValidAllRecords && IsValidRecord;

                if (!IsValidRecord&& OverlappedWith!=null)
                    ErrorMessages.Add(CompleteRecord.StartTime_DateTime.Value.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult))+" "+trlsTo+CompleteRecord.EndTime_DateTime.Value.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult))+" "+trls+" "+OverlappedWith.StartTime_DateTime.Value.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult))+" "+trlsTo+""+OverlappedWith.EndTime_DateTime.Value.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)));

            }

            return IsValidAllRecords;
        }
        private void CheckOverLapping_Delete()
        {
            foreach (MeetingAgendaViewModel dRow in grid1.deleted)
            {
                MeetingAgendaViewModel ExistedForDelete = OldRecords.SingleOrDefault(a => a.Id == dRow.Id);
                if (ExistedForDelete != null)
                    OldRecords.Remove(ExistedForDelete);
            }
        }

        private bool IsValid_UpdateList(/*ref List<MeetingAgendaViewModel> OldRecords,*/ MeetingAgendaViewModel CompleteRecord, bool IsAddMode, out MeetingAgendaViewModel OverlappedWith)
        {
            OverlappedWith = null;
            try
            {
                IQueryable<MeetingAgendaViewModel> Query = OldRecords.Where(m =>
                     ((m.StartTime_DateTime < CompleteRecord.StartTime_DateTime && m.EndTime_DateTime > CompleteRecord.StartTime_DateTime) ||
                    (m.StartTime_DateTime < CompleteRecord.EndTime_DateTime && m.EndTime_DateTime > CompleteRecord.EndTime_DateTime))).AsQueryable();

                if (!IsAddMode)
                    Query = Query.Where(m => m.Id != CompleteRecord.Id);

                if (Query.Any())
                    OverlappedWith = Query.FirstOrDefault();

                bool _IsValid= !Query.Any();

                if (_IsValid)
                {
                    if (IsAddMode)
                        OldRecords.Add(CompleteRecord);
                    else
                    {
                        MeetingAgendaViewModel UpdatedRecord = OldRecords.SingleOrDefault(m => m.Id == CompleteRecord.Id);
                        UpdatedRecord.StartTime = CompleteRecord.StartTime;
                        UpdatedRecord.EndTime = CompleteRecord.EndTime;
                    }
                }

                return _IsValid;
            }
            catch
            {
                return false;
            }
        }
    }
}
