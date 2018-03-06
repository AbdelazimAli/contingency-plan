
using Interface.Core;
using Model.Domain.Notifications;
using Model.ViewModel;
using Model.ViewModel.Notification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using WebApp.Models;
using System.Linq.Dynamic;
using System.Linq;
using System.Data;
using Model.Domain;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Routing;
using Hangfire;
using System.Threading.Tasks;
using System.Net.Mail;
using Model.ViewModel.Personnel;
using System.Globalization;
using System.Reflection;
using Db.Persistence.Services;
using Db.Persistence;

namespace WebApp.Controllers
{
    public class MeetingController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                Language = requestContext.HttpContext.User.Identity.GetLanguage();
                CompanyId = requestContext.HttpContext.User.Identity.GetDefaultCompany();
                UserName = requestContext.HttpContext.User.Identity.Name;
            }
        }
        public MeetingController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        #region Meetings
        public ActionResult MeetingIndex()
        {
            return View();
        }
        //ReadMeeting
        public ActionResult ReadMeeting(int MenuId, int pageSize, int skip, byte? Range, DateTime? Start, DateTime? End)
        {
            var EmpId = User.Identity.GetEmpId();
            var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(User.Identity.GetTimeZone()));
            var query = _hrUnitOfWork.MeetingRepository.GetMeetings(EmpId, Range ?? 10, Start, End, Language, CompanyId, currentTime);
            return ApplyFilter<MeetingViewModel>(query, null, MenuId, pageSize, skip);
        }

        public ActionResult MeetingDetails(int id = 0, byte Version = 0)
        {
            ViewBag.SiteId = _hrUnitOfWork.SiteRepository.ReadSites(Language, CompanyId).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.BranchId = _hrUnitOfWork.BranchRepository.ReadBranches(Language, CompanyId).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.Employees = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(CompanyId, Language);

            if (id == 0) return View(new MeetingViewModel());

            var Meeting = _hrUnitOfWork.MeetingRepository.ReadMeeting(id,Language);

            //ViewBag.Attendee = _hrUnitOfWork.MeetingRepository.GetMeetingAttendee(Meeting.Id, Language);
            //ViewBag.Viewer = _hrUnitOfWork.MeetingRepository.GetMeetingViewer(Meeting.Id, Language);

            return Meeting == null ? (ActionResult)HttpNotFound() : View(Meeting);
        }
        public ActionResult ReadAgenda(int MeetingId)
        {
            var query = _hrUnitOfWork.MeetingRepository.GetAgenda(MeetingId, Language);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        // Save Meeting Form 
        public ActionResult SaveMeetingForm(SaveMeetingViewModel model, OptionsViewModel moreInfo, RequestAgendaGrid grid1, bool clear)
        {
            List<Error> errors = new List<Error>();
            List<MeetNotifyAttendeeViewModel> Attendee = new List<MeetNotifyAttendeeViewModel>();
            var dbActivate = false;
            var PreviousMeetDate = DateTime.Now;
            var PreviousStartTime = DateTime.Now;
            var PreviousEndTime = DateTime.Now;
            int PreviousLocationType = 0;
            var PreviousSiteId = 0;
            var PreviousBranchId = 0;
            string PreviousLocationText = "";

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {

                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "MeetingForm",
                        TableName = "Meetings",
                        Columns = Models.Utils.GetColumnViews(ModelState).Where(s => !(new string[] { "SiteId", "BranchId", "LocationText" }).Contains(s.Name)).ToList(),
                        ParentColumn = "CompanyId",
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        foreach (var e in errors)
                        {
                            foreach (var errorMsg in e.errors)
                            {
                                ModelState.AddModelError(errorMsg.field, errorMsg.message);
                            }
                        }

                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }

                //check if not Entered  
                if (model.LocationType == 1 && model.BranchId == null)
                {
                    ModelState.AddModelError("BranchId", MsgUtils.Instance.Trls("BranchRequired"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                else if (model.LocationType == 2 && model.SiteId == null)
                {
                    ModelState.AddModelError("SiteId", MsgUtils.Instance.Trls("SiteRequired"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                else if (model.LocationType == 3 && model.LocationText == null)
                {
                   
                    ModelState.AddModelError("LocationText", MsgUtils.Instance.Trls("LocationRequired"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }

                //Check if one of the viewer are attende 
                if (model.MeetingViewer != null)
                {
                    foreach (var item in model.MeetingViewer)
                    {
                        if (model.MeetingAttendee.Contains(item) || int.Parse(item) == model.EmpId)
                        {
                            ModelState.AddModelError("", MsgUtils.Instance.Trls("Viewer Conflict"));
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                    }
                }

                // check Server Time with start Meeting 
                if (model.MeetDate.Date == DateTime.Today.Date)
                {
                    var currentime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(User.Identity.GetTimeZone()));
                    if (model.StartTime < currentime)
                    {
                        ModelState.AddModelError("StartTime", MsgUtils.Instance.Trls("Start Less than Now"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }
                // check Server date with Meeting date 
                if (model.MeetDate.Date < DateTime.Today.Date)
                {
                    ModelState.AddModelError("MeetDate", MsgUtils.Instance.Trls("Date Less than Now"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                // check over lap Updated Record
                var OldRecords = _hrUnitOfWork.MeetingRepository.GetAgenda(model.Id, Language).ToList();
                List<string> OverLappingErrorMessages = new List<string>();
                CheckOverLapping _CheckOverLapping = new CheckOverLapping(OldRecords, grid1, model.StartTime, model.EndTime);
                var trls = MsgUtils.Instance.Trls("overlapping with");
                var trlsTo = MsgUtils.Instance.Trls("To");
                var trlsOutOfMeeting = MsgUtils.Instance.Trls("Is out of Meeting time");
                var trlsInvalid = MsgUtils.Instance.Trls("Is Invalid");
                bool IsOverLapping = _CheckOverLapping.Run(out OverLappingErrorMessages, trls, Language, trlsTo, trlsOutOfMeeting, trlsInvalid);
                if (!IsOverLapping)
                {
                    foreach (var item in OverLappingErrorMessages)
                    {
                        ModelState.AddModelError("", item);
                    }
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }

                var record = _hrUnitOfWork.Repository<Meeting>().FirstOrDefault(j => j.Id == model.Id);
                var StartDateTime = model.MeetDate.Date.Add(model.StartTime.TimeOfDay);
                var EndDateTime = model.MeetDate.Date.Add(model.EndTime.TimeOfDay);
                string ConfilctMessage = string.Empty;
                if (record == null) //Add
                {
                    //Check First If Conflict in Time with Employee Schedual
                    if (model.Activate == true)
                    {
                        
                        string Ids = string.Join(",", model.MeetingAttendee);                      
                        bool IsValid = Constants.CheckDateRangAndTasksConflict.IsValid(_hrUnitOfWork, Ids, Constants.Sources.Meeting, 0, StartDateTime, EndDateTime, model.MeetDate.Date, Language,out ConfilctMessage);
                        if (!IsValid)
                        {
                            ModelState.AddModelError("", MsgUtils.Instance.Trls("Meeting Confilct"));
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                    }
                    record = new Meeting();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "MeetingForm",
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    record.CompanyId = CompanyId;
                    record.Status = 1;
                    record.IsActivate = model.Activate == true ? true : false;

                    if (model.LocationType == 1)
                    {
                        record.SiteId = null;
                        record.LocationText = null;
                    }
                    else if (model.LocationType == 2)
                    {
                        record.BranchId = null;
                        record.LocationText = null;
                    }
                    else
                    {
                        record.SiteId = null;
                        record.BranchId = null;
                    }
                    model.StartTime = StartDateTime;
                    model.EndTime = EndDateTime;
                    model.Status = record.Status;
                    model.BranchId = record.BranchId;
                    model.LocationText = record.LocationText;
                    model.SiteId = record.SiteId;
                    _hrUnitOfWork.MeetingRepository.Add(record);

                    if (model.MeetingAttendee != null)
                    {
                        foreach (var item in model.MeetingAttendee)
                        {
                            var doc = new MeetAttendee
                            {
                                Meeting = record,
                                EmpId = int.Parse(item)
                            };
                            _hrUnitOfWork.MeetingRepository.Add(doc);
                        }
                    }

                    if (model.MeetingViewer != null)
                    {
                        foreach (var item in model.MeetingViewer)
                        {
                            var viewer = new MeetViewer
                            {
                                Meeting = record,
                                EmpId = int.Parse(item)
                            };
                            _hrUnitOfWork.MeetingRepository.Add(viewer);
                        }
                    }

                }
                else //update
                {

                    var dbAttendeObjects = _hrUnitOfWork.Repository<MeetAttendee>().Where(a => a.MeetingId == record.Id).ToList();
                    var dbViewerObjects = _hrUnitOfWork.Repository<MeetViewer>().Where(a => a.MeetingId == record.Id).ToList();
                    // Add new Attendee
                    if (model.MeetingAttendee != null)
                    {
                        foreach (var item in model.MeetingAttendee)
                        {
                            if (!dbAttendeObjects.Select(a => a.EmpId).Contains(int.Parse(item)))
                            {
                                var doc = new MeetAttendee
                                {
                                    Meeting = record,
                                    EmpId = int.Parse(item)
                                };
                                _hrUnitOfWork.MeetingRepository.Add(doc);
                                Attendee.Add(new MeetNotifyAttendeeViewModel { EmpId = int.Parse(item), Procedure = "Create" });
                            }
                        }
                        if (dbAttendeObjects != null)
                        {
                            foreach (var item in dbAttendeObjects.Select(a => a.EmpId).ToList())
                            {
                                if (!model.MeetingAttendee.Contains(item.ToString()))
                                {

                                    var attendee = dbAttendeObjects.Where(a => a.EmpId == item && a.MeetingId == model.Id).FirstOrDefault();
                                    Attendee.Add(new MeetNotifyAttendeeViewModel { EmpId = item, Procedure = "Cancel" });
                                    _hrUnitOfWork.MeetingRepository.Remove(attendee);
                                }
                            }
                        }
                    }
                    // Add new viewer
                    if (model.MeetingViewer != null)
                    {
                        foreach (var item in model.MeetingViewer)
                        {
                            if (!dbViewerObjects.Select(a => a.EmpId).Contains(int.Parse(item)))
                            {
                                var viewer = new MeetViewer
                                {
                                    Meeting = record,
                                    EmpId = int.Parse(item)
                                };
                                _hrUnitOfWork.MeetingRepository.Add(viewer);
                            }
                        }
                    }
                    if (dbViewerObjects != null)
                    {
                        foreach (var item in dbViewerObjects.Select(a => a.EmpId).ToList())
                        {
                            if (model.MeetingViewer != null)
                            {
                                if (!model.MeetingViewer.Contains(item.ToString()))
                                {
                                    var viewer = dbViewerObjects.Where(a => a.EmpId == item && a.MeetingId == model.Id).FirstOrDefault();
                                    _hrUnitOfWork.MeetingRepository.Remove(viewer);
                                }
                            }
                            else
                            {
                                var viewer = dbViewerObjects.Where(a => a.EmpId == item && a.MeetingId == model.Id).FirstOrDefault();
                                _hrUnitOfWork.MeetingRepository.Remove(viewer);
                            }
                        }
                    }
                    dbActivate = record.IsActivate;
                    PreviousMeetDate = record.MeetDate;
                    PreviousLocationType = record.LocationType;
                    PreviousLocationText = record.LocationText;
                    PreviousBranchId = Convert.ToInt32(record.BranchId);
                    PreviousSiteId = Convert.ToInt32(record.SiteId);
                    PreviousStartTime = record.StartTime;
                    PreviousEndTime = record.EndTime;

                    if (record.BranchId != null)
                        PreviousLocationText = _hrUnitOfWork.Repository<Branch>().Where(a => a.Id == record.BranchId).Select(s => s.Name).FirstOrDefault();
                    else if (record.SiteId != null)
                        PreviousLocationText = _hrUnitOfWork.Repository<Site>().Where(a => a.Id == record.SiteId).Select(s => s.Name).FirstOrDefault();
                    else if (record.LocationText != null)
                        PreviousLocationText = record.LocationText;

                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "MeetingForm",
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.CompanyId = CompanyId;
                    record.Status = 2;
                    record.StartTime = StartDateTime;
                    record.EndTime = EndDateTime;
                    if (!record.IsActivate)
                        record.IsActivate = model.Activate == true ? true : false;

                    if (model.LocationType == 1)
                    {
                        record.SiteId = null;
                        record.LocationText = null;
                    }
                    else if (model.LocationType == 2)
                    {
                        record.BranchId = null;
                        record.LocationText = null;
                    }
                    else
                    {
                        record.SiteId = null;
                        record.BranchId = null;
                    }

                    model.Status = record.Status;
                    model.BranchId = record.BranchId;
                    model.SiteId = record.SiteId;
                    model.LocationText = record.LocationText;

                    if (record.IsActivate == true)
                    {

                        string Ids = string.Join(",", model.MeetingAttendee);
                        bool IsValid = Constants.CheckDateRangAndTasksConflict.IsValid(_hrUnitOfWork, Ids, Constants.Sources.Meeting, 0, StartDateTime, EndDateTime, model.MeetDate.Date, Language,out ConfilctMessage);
                        if (!IsValid)
                        {
                            ModelState.AddModelError("", MsgUtils.Instance.Trls("Meeting Confilct"));
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                    }
                    _hrUnitOfWork.MeetingRepository.Attach(record);
                    _hrUnitOfWork.MeetingRepository.Entry(record).State = EntityState.Modified;

                }

                // Save grid1
                string message = "OK";
                errors = SaveGrid1(grid1, ModelState.Where(a => a.Key.Contains("grid1")), record);
                if (errors.Count > 0) return Json(errors.First().errors.First().message);

                var trans = _hrUnitOfWork.BeginTransaction();
                errors = SaveChanges(Language);
                if (errors.Count > 0) goto Failure;
                model.Id = record.Id;

                string Changedlocation = model.LocationType == 3 ? model.LocationText : model.CompanyLocation;
                bool IsLocationType_NotChanged = CheckLocationType_NotChanged(PreviousLocationType, model.LocationType);
                bool IsLocationText_NotChanged = CheckLocationText_NotChanged(PreviousLocationText, model.LocationText);
                bool IsLocationDrop_NotChanged = CheckLocationDrop_NotChanged(PreviousSiteId, model.BranchId);


                //new record and press Activate
                if (model.Activate == true && model.Id == 0)
                {
                    // Send Notify Letters
                    errors = SendNotifyLetters(model, record.Id, Changedlocation);
                    if (errors.Count > 0) goto Failure;

                }
                // Edited record and first time to press Activate
                else if (model.Activate && model.Id > 0 && dbActivate == false)
                {
                    errors = SendNotifyLetters(model, record.Id, Changedlocation);
                    if (errors.Count > 0) goto Failure;

                }

                // Edited record is activated before 
                else if (model.Id > 0 && dbActivate)
                {
                    //check if Attendee Change To send Letters or Cancel letters
                    if (Attendee.Count > 0)
                    {
                        foreach (var item in Attendee)
                        {
                            if (item.Procedure == "Create")
                            {
                                errors = CreateNotifyLetters(model, item.EmpId, Changedlocation);
                                if (errors.Count > 0) goto Failure;
                            }
                            else if (item.Procedure == "Cancel")
                            {
                                errors = CancelNottifyLetters(model, item.EmpId, Changedlocation);
                                if (errors.Count > 0) goto Failure;
                            }
                        }
                    }
                    //check if Meeting Date Changes if Change Send Modified Notify Letters
                    if (model.MeetDate.Date != PreviousMeetDate.Date && (IsLocationType_NotChanged && IsLocationText_NotChanged && IsLocationDrop_NotChanged) && model.StartTime.TimeOfDay == PreviousStartTime.TimeOfDay && model.EndTime.TimeOfDay == PreviousEndTime.TimeOfDay)
                    {
                        foreach (var item in model.MeetingAttendee)
                        {
                            if (!Attendee.Select(a => a.EmpId).Contains(int.Parse(item)))
                            {
                                errors = EditDateNotifyLetters(model, int.Parse(item), PreviousMeetDate);
                                if (errors.Count > 0) goto Failure;
                            }
                        }
                    }
                    //check if Location Changes if Change Send Modified Notify Letters
                    else if ((!IsLocationType_NotChanged || !IsLocationText_NotChanged || !IsLocationDrop_NotChanged) && model.MeetDate.Date == PreviousMeetDate.Date && model.StartTime.TimeOfDay == PreviousStartTime.TimeOfDay && model.EndTime.TimeOfDay == PreviousEndTime.TimeOfDay)
                    {

                        foreach (var item in model.MeetingAttendee)
                        {
                            if (!Attendee.Select(a => a.EmpId).Contains(int.Parse(item)))
                            {
                                errors = EditLocationNotifyLetters(model, int.Parse(item), Changedlocation, PreviousLocationText);
                                if (errors.Count > 0) goto Failure;
                            }
                        }
                    }
                    //check if startTime Changes if Change Send Modified Notify Letters
                    else if (model.StartTime.TimeOfDay != PreviousStartTime.TimeOfDay && model.EndTime.TimeOfDay == PreviousEndTime.TimeOfDay && (IsLocationType_NotChanged && IsLocationText_NotChanged && IsLocationDrop_NotChanged) && model.MeetDate.Date == PreviousMeetDate.Date)
                    {
                        foreach (var item in model.MeetingAttendee)
                        {
                            if (!Attendee.Select(a => a.EmpId).Contains(int.Parse(item)))
                            {
                                errors = EditStartTimeNotifyLetters(model, int.Parse(item), PreviousStartTime);
                                if (errors.Count > 0) goto Failure;
                            }
                        }
                    }
                    //check if EndTime Changes if Change Send Modified Notify Letters

                    else if (model.EndTime.TimeOfDay != PreviousEndTime.TimeOfDay && model.StartTime.TimeOfDay == PreviousStartTime.TimeOfDay && (IsLocationType_NotChanged && IsLocationText_NotChanged && IsLocationDrop_NotChanged) && model.MeetDate.Date == PreviousMeetDate.Date)
                    {
                        foreach (var item in model.MeetingAttendee)
                        {
                            if (!Attendee.Select(a => a.EmpId).Contains(int.Parse(item)))
                            {
                                errors = EditEndTimeNotifyLetters(model, int.Parse(item), PreviousEndTime);
                                if (errors.Count > 0) goto Failure;
                            }
                        }
                    }
                    //check if Location Changes and MeetingDate if Change Send Modified Notify Letters
                    else if (model.MeetDate.Date != PreviousMeetDate.Date && (!IsLocationType_NotChanged || !IsLocationText_NotChanged || !IsLocationDrop_NotChanged) && model.StartTime.TimeOfDay == PreviousStartTime.TimeOfDay && model.EndTime.TimeOfDay == PreviousEndTime.TimeOfDay)
                    {
                        foreach (var item in model.MeetingAttendee)
                        {
                            if (!Attendee.Select(a => a.EmpId).Contains(int.Parse(item)))
                            {
                                errors = EditNotifyLetters(model, int.Parse(item), Changedlocation, PreviousLocationText, PreviousMeetDate);
                                if (errors.Count > 0) goto Failure;
                            }
                        }
                    }
                    //check if startTime and EndTime Changes if Change Send Modified Notify Letters
                    else if (model.MeetDate.Date == PreviousMeetDate.Date && (IsLocationType_NotChanged && IsLocationText_NotChanged && IsLocationDrop_NotChanged) && model.StartTime.TimeOfDay != PreviousStartTime.TimeOfDay && model.EndTime.TimeOfDay != PreviousEndTime.TimeOfDay)
                    {
                        foreach (var item in model.MeetingAttendee)
                        {
                            if (!Attendee.Select(a => a.EmpId).Contains(int.Parse(item)))
                            {
                                errors = EditNotifyTimesLetters(model, int.Parse(item), PreviousEndTime, PreviousStartTime);
                                if (errors.Count > 0) goto Failure;
                            }
                        }
                    }
                    //check if  MeetDate and Times changes if Change Send Modified Notify Letters
                    else if (model.MeetDate.Date.ToString("yyyy-MM-dd") != PreviousMeetDate.Date.ToString("yyyy-MM-dd") && IsLocationType_NotChanged && IsLocationText_NotChanged && IsLocationDrop_NotChanged && model.StartTime.TimeOfDay != PreviousStartTime.TimeOfDay && model.EndTime.TimeOfDay != PreviousEndTime.TimeOfDay)
                    {
                        foreach (var item in model.MeetingAttendee)
                        {
                            if (!Attendee.Select(a => a.EmpId).Contains(int.Parse(item)))
                            {
                                errors = EditDateTimesLetters(model, int.Parse(item), PreviousMeetDate, PreviousStartTime, PreviousEndTime);
                                if (errors.Count > 0) goto Failure;
                            }
                        }
                    }
                    //check if Location Changes and Times if Change Send Modified Notify Letters
                    else if (model.MeetDate.Date == PreviousMeetDate.Date && (!IsLocationType_NotChanged || !IsLocationText_NotChanged || !IsLocationDrop_NotChanged) && model.StartTime.TimeOfDay != PreviousStartTime.TimeOfDay && model.EndTime.TimeOfDay != PreviousEndTime.TimeOfDay)
                    {
                        foreach (var item in model.MeetingAttendee)
                        {
                            if (!Attendee.Select(a => a.EmpId).Contains(int.Parse(item)))
                            {
                                errors = EditLocationAndTimesNotifyLetters(model, int.Parse(item), Changedlocation, PreviousLocationText, PreviousStartTime, PreviousEndTime);
                                if (errors.Count > 0) goto Failure;
                            }
                        }
                    }
                    //check if Location Changes and MeetDate and Times if Change Send Modified Notify Letters
                    else if (model.MeetDate.Date != PreviousMeetDate.Date && (!IsLocationType_NotChanged || !IsLocationText_NotChanged || !IsLocationDrop_NotChanged) && model.StartTime.TimeOfDay != PreviousStartTime.TimeOfDay && model.EndTime.TimeOfDay != PreviousEndTime.TimeOfDay)
                    {
                        foreach (var item in model.MeetingAttendee)
                        {
                            if (!Attendee.Select(a => a.EmpId).Contains(int.Parse(item)))
                            {
                                errors = EditNotifyLetters(model, int.Parse(item), Changedlocation, PreviousLocationText, PreviousMeetDate);
                                if (errors.Count > 0) goto Failure;
                            }
                        }
                    }

                }

                trans.Commit();
                trans.Dispose();
                if (clear) model = new SaveMeetingViewModel();
                message += "," + (new JavaScriptSerializer()).Serialize(model);
                goto Exit;

                Failure:
                trans.Rollback();
                trans.Dispose();
                message = errors.First().errors.First().message;

                Exit:
                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }


        // Save Grid Agenda to meeting 
        private List<Error> SaveGrid1(RequestAgendaGrid grid1, IEnumerable<KeyValuePair<string, ModelState>> state, Meeting Meetingobj)
        {
            List<Error> errors = new List<Error>();
            // Deleted
            if (grid1.deleted != null)
            {
                foreach (MeetingAgendaViewModel model in grid1.deleted)
                {
                    var RequestAgendaGrid = new MeetSchedual
                    {
                        Id = model.Id
                    };
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Source = RequestAgendaGrid,
                        ObjectName = "MeetingAgenda",
                        Transtype = TransType.Delete
                    });
                    _hrUnitOfWork.MeetingRepository.Remove(RequestAgendaGrid);
                }
            }
            // Exclude delete models from sever side validations
            //if (ServerValidationEnabled)
            //{
            //    var modified = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted")));
            //    if (modified.Count > 0)
            //    {
            //        errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
            //        {
            //            CompanyId = CompanyId,
            //            ObjectName = "MeetingAgenda",
            //            Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
            //            Culture = Language
            //        });
            //        if (errors.Count() > 0) return errors;
            //    }
            //}
            // updated records
            if (grid1.updated != null)
            {
                foreach (MeetingAgendaViewModel model in grid1.updated)
                {

                    var requestwf = new MeetSchedual();
                    AutoMapper(new Models.AutoMapperParm { Destination = requestwf, Source = model, Transtype = TransType.Update });
                    var StarDatetime = Convert.ToDateTime(model.StartTime);
                    var EndDatetime = Convert.ToDateTime(model.EndTime);
                    requestwf.StartTime = StarDatetime.TimeOfDay;
                    requestwf.EndTime = EndDatetime.TimeOfDay;
                    requestwf.MeetingId = Meetingobj.Id;
                    requestwf.ModifiedUser = UserName;
                    requestwf.ModifiedTime = DateTime.Now;
                    _hrUnitOfWork.MeetingRepository.Attach(requestwf);
                    _hrUnitOfWork.MeetingRepository.Entry(requestwf).State = EntityState.Modified;
                }
            }

            // inserted records

            if (grid1.inserted != null)
            {
                foreach (MeetingAgendaViewModel model in grid1.inserted)
                {
                    var newAgenda = new MeetSchedual();
                    AutoMapper(new Models.AutoMapperParm { ObjectName = "MeetingAgenda", Destination = newAgenda, Source = model, Transtype = TransType.Insert });
                    var StarDatetime = Convert.ToDateTime(model.StartTime);
                    var EndDatetime = Convert.ToDateTime(model.EndTime);
                    newAgenda.StartTime = StarDatetime.TimeOfDay;
                    newAgenda.EndTime = EndDatetime.TimeOfDay;
                    newAgenda.CreatedUser = UserName;
                    newAgenda.CreatedTime = DateTime.Now;
                    newAgenda.Meeting = Meetingobj;
                    if (model.EmpId == 0) newAgenda.EmpId = null;
                    _hrUnitOfWork.MeetingRepository.Add(newAgenda);
                }
            }

            return errors;
        }
        private bool CheckLocationType_NotChanged(int PreviousLocationType, int CurrentLocationType)
        {
            return PreviousLocationType == CurrentLocationType;
        }
        private bool CheckLocationText_NotChanged(string PreviousLocationText, string CurrentLocationText)
        {
            return Convert.ToString(PreviousLocationText) == Convert.ToString(CurrentLocationText);
        }

        private bool CheckLocationDrop_NotChanged(int? PreviousLocationDropID, int? CurrentLocationDropID)
        {
            return Convert.ToInt32(PreviousLocationDropID) == Convert.ToInt32(CurrentLocationDropID);
        }
        private List<Error> SendNotifyLetters(SaveMeetingViewModel model, int Id, string DefinedLocation)
        {
            List<Error> errors = new List<Error>();
            var EmpIds = string.Join(",", model.MeetingAttendee);
            var lang = _hrUnitOfWork.MeetingRepository.GetUsersLang(EmpIds);
            foreach (var item in model.MeetingAttendee)
            {
                var chkLang = lang.Where(a => a.id == int.Parse(item)).Select(a => a.name).FirstOrDefault();
                var cult = chkLang == null ? Language : chkLang;
                NotifyLetter NL = new NotifyLetter()
                {
                    CompanyId = CompanyId,
                    EmpId = int.Parse(item),
                    NotifyDate = DateTime.Now,
                    NotifySource = MsgUtils.Instance.Trls("MeetingCreate", cult),
                    SourceId = Id.ToString(),
                    Sent = false,
                    EventDate = model.MeetDate,
                    Description = MsgUtils.Instance.Trls("intoduction", cult) + " " + model.subjectText + " " + MsgUtils.Instance.Trls("fromHours", cult) + model.StartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " +
                    MsgUtils.Instance.Trls("ToHours", cult) + " " + model.EndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " + MsgUtils.Instance.Trls("In", cult) + " " + DefinedLocation + " " + MsgUtils.Instance.Trls("Meeting Date", cult) + model.MeetDate.ToString("dddd dd/MM/yyyy") + " " +
                    MsgUtils.Instance.Trls("organiser", cult) + model.organiser
                };
                _hrUnitOfWork.MeetingRepository.AddNotifyLetter(NL);
            }
            errors = Save(Language);
            return errors;
        }
        // Create New Notify Letter To Modified Attendee 
        private List<Error> CreateNotifyLetters(SaveMeetingViewModel model, int EmpId, string DefinedLocation)
        {
            List<Error> errors = new List<Error>();
            var lang = _hrUnitOfWork.MeetingRepository.GetUsersLang(EmpId.ToString()).Select(s => s.name).FirstOrDefault();
            var cult = lang == null ? Language : lang;
            NotifyLetter NL = new NotifyLetter()
            {
                CompanyId = CompanyId,
                EmpId = EmpId,
                NotifyDate = DateTime.Now,
                NotifySource = MsgUtils.Instance.Trls("MeetingCreate", cult),
                SourceId = model.Id.ToString(),
                Sent = false,
                EventDate = model.MeetDate,
                Description = MsgUtils.Instance.Trls("intoduction", cult) + " " + model.subjectText + " " + MsgUtils.Instance.Trls("fromHours", cult) + model.StartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " +
                MsgUtils.Instance.Trls("ToHours", cult) + " " + model.EndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " + MsgUtils.Instance.Trls("In", cult) + " " + DefinedLocation + " " + MsgUtils.Instance.Trls("Meeting Date", cult) + model.MeetDate.ToString("dddd dd/MM/yyyy") + " " +
                MsgUtils.Instance.Trls("organiser", cult) + model.organiser
            };
            _hrUnitOfWork.MeetingRepository.AddNotifyLetter(NL);
            errors = Save(Language);
            return errors;
        }
        // Cancel Notify Letters To Removed Attendee
        private List<Error> CancelNottifyLetters(SaveMeetingViewModel model, int EmpId, string DefinedLocation)
        {
            List<Error> errors = new List<Error>();
            var lang = _hrUnitOfWork.MeetingRepository.GetUsersLang(EmpId.ToString()).Select(a => a.name).FirstOrDefault();
            var cult = lang == null ? Language : lang;
            NotifyLetter NL = new NotifyLetter()
            {
                CompanyId = CompanyId,
                EmpId = EmpId,
                NotifyDate = DateTime.Now,
                NotifySource = MsgUtils.Instance.Trls("MeetingCancel", cult),
                SourceId = model.Id.ToString(),
                Sent = false,
                EventDate = model.MeetDate,
                Description = MsgUtils.Instance.Trls("intoductionCancel", cult) + " " + model.subjectText + " " + MsgUtils.Instance.Trls("DefinedIn", cult) + " " + MsgUtils.Instance.Trls("fromHours", cult) + model.StartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " +
                MsgUtils.Instance.Trls("ToHours", cult) + " " + model.EndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " + MsgUtils.Instance.Trls("In", cult) + " " + DefinedLocation + " " + MsgUtils.Instance.Trls("Meeting Date", cult) + model.MeetDate.ToString("dddd dd/MM/yyyy") + " " +
                MsgUtils.Instance.Trls("organiser", cult) + model.organiser
            };
            _hrUnitOfWork.MeetingRepository.AddNotifyLetter(NL);
            errors = Save(Language);
            return errors;
        }
        //Edit Date Notify Leters To Attendee
        private List<Error> EditDateNotifyLetters(SaveMeetingViewModel model, int EmpId, DateTime PreviousMeetTime)
        {
            List<Error> errors = new List<Error>();
            var lang = _hrUnitOfWork.MeetingRepository.GetUsersLang(EmpId.ToString()).Select(s => s.name).FirstOrDefault();
            var cult = lang == null ? Language : lang;
            NotifyLetter NL = new NotifyLetter()
            {
                CompanyId = CompanyId,
                EmpId = EmpId,
                NotifyDate = DateTime.Now,
                NotifySource = MsgUtils.Instance.Trls("MeetingModified", cult),
                SourceId = model.Id.ToString(),
                Sent = false,
                EventDate = model.MeetDate,
                Description = MsgUtils.Instance.Trls("Editintroduction", cult) + " " + model.subjectText + " " + MsgUtils.Instance.Trls("fromHours", cult) + model.StartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " +
                MsgUtils.Instance.Trls("ToHours", cult) + " " + model.EndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " + MsgUtils.Instance.Trls("Meeting Date", cult) + model.MeetDate.ToString("dddd dd/MM/yyyy") + " " +
                MsgUtils.Instance.Trls("PreviousMeetingDate", cult) + PreviousMeetTime.ToString("dddd dd/MM/yyyy") + " " +
                MsgUtils.Instance.Trls("organiser", cult) + model.organiser
            };
            _hrUnitOfWork.MeetingRepository.AddNotifyLetter(NL);
            errors = Save(Language);
            return errors;

        }
        // Edit Location Notify Letters To Attendee    
        private List<Error> EditLocationNotifyLetters(SaveMeetingViewModel model, int EmpId, string ChangedLocation, string PreviousLocation)
        {
            List<Error> errors = new List<Error>();
            var lang = _hrUnitOfWork.MeetingRepository.GetUsersLang(EmpId.ToString()).Select(s => s.name).FirstOrDefault();
            var cult = lang == null ? Language : lang;
            NotifyLetter NL = new NotifyLetter()
            {
                CompanyId = CompanyId,
                EmpId = EmpId,
                NotifyDate = DateTime.Now,
                NotifySource = MsgUtils.Instance.Trls("MeetingModified", cult),
                SourceId = model.Id.ToString(),
                Sent = false,
                EventDate = model.MeetDate,
                Description = MsgUtils.Instance.Trls("EditLocationintroduction", cult) + " " + model.subjectText + " " + MsgUtils.Instance.Trls("fromLocation", cult) + " " +
                PreviousLocation + " " + MsgUtils.Instance.Trls("ToLocation", cult) + " " + ChangedLocation + " " +
                MsgUtils.Instance.Trls("organiser", cult) + model.organiser
            };
            _hrUnitOfWork.MeetingRepository.AddNotifyLetter(NL);
            errors = Save(Language);
            return errors;

        }
        private List<Error> EditStartTimeNotifyLetters(SaveMeetingViewModel model, int EmpId, DateTime PreviousStartTime)
        {
            List<Error> errors = new List<Error>();
            var lang = _hrUnitOfWork.MeetingRepository.GetUsersLang(EmpId.ToString()).Select(s => s.name).FirstOrDefault();
            var cult = lang == null ? Language : lang;
            NotifyLetter NL = new NotifyLetter()
            {
                CompanyId = CompanyId,
                EmpId = EmpId,
                NotifyDate = DateTime.Now,
                NotifySource = MsgUtils.Instance.Trls("MeetingModified", cult),
                SourceId = model.Id.ToString(),
                Sent = false,
                EventDate = model.MeetDate,
                Description = MsgUtils.Instance.Trls("EditStartTimeintroduction", cult) + " " + model.subjectText + " " + MsgUtils.Instance.Trls("fromHours", cult) + " " +
               PreviousStartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " + MsgUtils.Instance.Trls("ToHours", cult) + " " + model.StartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " + MsgUtils.Instance.Trls("EndHours", cult) + " " + model.EndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " +
                MsgUtils.Instance.Trls("organiser", cult) + model.organiser
            };
            _hrUnitOfWork.MeetingRepository.AddNotifyLetter(NL);
            errors = Save(Language);
            return errors;
        }
        private List<Error> EditEndTimeNotifyLetters(SaveMeetingViewModel model, int EmpId, DateTime PreviousEndTime)
        {
            List<Error> errors = new List<Error>();
            var lang = _hrUnitOfWork.MeetingRepository.GetUsersLang(EmpId.ToString()).Select(s => s.name).FirstOrDefault();
            var cult = lang == null ? Language : lang;
            NotifyLetter NL = new NotifyLetter()
            {
                CompanyId = CompanyId,
                EmpId = EmpId,
                NotifyDate = DateTime.Now,
                NotifySource = MsgUtils.Instance.Trls("MeetingModified", cult),
                SourceId = model.Id.ToString(),
                Sent = false,
                EventDate = model.MeetDate,
                Description = MsgUtils.Instance.Trls("EditEndTimeintroduction", cult) + " " + model.subjectText + " " + MsgUtils.Instance.Trls("fromHours", cult) + " " +
                PreviousEndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " + MsgUtils.Instance.Trls("ToHours", cult) + " " + model.EndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " + MsgUtils.Instance.Trls("StartHours", cult) + " " + model.StartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " +
                MsgUtils.Instance.Trls("organiser", cult) + model.organiser
            };
            _hrUnitOfWork.MeetingRepository.AddNotifyLetter(NL);
            errors = Save(Language);
            return errors;
        }
        //Edit StartTime && endtime 
        private List<Error> EditNotifyTimesLetters(SaveMeetingViewModel model, int EmpId, DateTime PreviousEndTime, DateTime PreviousStartTime)
        {
            List<Error> errors = new List<Error>();
            var lang = _hrUnitOfWork.MeetingRepository.GetUsersLang(EmpId.ToString()).Select(s => s.name).FirstOrDefault();
            var cult = lang == null ? Language : lang;
            NotifyLetter NL = new NotifyLetter()
            {
                CompanyId = CompanyId,
                EmpId = EmpId,
                NotifyDate = DateTime.Now,
                NotifySource = MsgUtils.Instance.Trls("MeetingModified", cult),
                SourceId = model.Id.ToString(),
                Sent = false,
                EventDate = model.MeetDate,
                Description = MsgUtils.Instance.Trls("EditEndTimeintroduction", cult) + " " +
                model.subjectText + " " + MsgUtils.Instance.Trls("fromHours", cult) + " " +
                PreviousEndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " "
                + MsgUtils.Instance.Trls("ToHours", cult) + " " +
                model.EndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " "
                + MsgUtils.Instance.Trls("EditStartTimeintroduction", cult) + " "
                + MsgUtils.Instance.Trls("fromHours", cult) + " " +
                PreviousStartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " "
                + MsgUtils.Instance.Trls("ToHours", cult) + " "
                + model.StartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " "
                +
                MsgUtils.Instance.Trls("organiser", cult) + model.organiser
            };
            _hrUnitOfWork.MeetingRepository.AddNotifyLetter(NL);
            errors = Save(Language);
            return errors;
        }
        //Send Notify MeetingDate && start && end time changes
        private List<Error> EditDateTimesLetters(SaveMeetingViewModel model, int EmpId, DateTime PreviousMeetTime, DateTime PreviousStartTime, DateTime PreviousEndTime)
        {
            List<Error> errors = new List<Error>();
            var lang = _hrUnitOfWork.MeetingRepository.GetUsersLang(EmpId.ToString()).Select(s => s.name).FirstOrDefault();
            var cult = lang == null ? Language : lang;
            NotifyLetter NL = new NotifyLetter()
            {
                CompanyId = CompanyId,
                EmpId = EmpId,
                NotifyDate = DateTime.Now,
                NotifySource = MsgUtils.Instance.Trls("MeetingModified", cult),
                SourceId = model.Id.ToString(),
                Sent = false,
                EventDate = model.MeetDate,
                Description = MsgUtils.Instance.Trls("Editintroduction", cult) + " " + model.subjectText
                + " " + MsgUtils.Instance.Trls("Meeting Date", cult) + model.MeetDate.ToString("dddd dd/MM/yyyy") + " " +
                MsgUtils.Instance.Trls("PreviousMeetingDate", cult) + PreviousMeetTime.ToString("dddd dd/MM/yyyy") + " "
                + MsgUtils.Instance.Trls("EditEndTimeintroduction", cult) + " " +
                model.subjectText + " " + MsgUtils.Instance.Trls("fromHours", cult) + " " +
                PreviousEndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " "
                + MsgUtils.Instance.Trls("ToHours", cult) + " " +
                model.EndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " "
                + MsgUtils.Instance.Trls("EditStartTimeintroduction", cult) + " "
                + MsgUtils.Instance.Trls("fromHours", cult) + " " +
                PreviousStartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " "
                + MsgUtils.Instance.Trls("ToHours", cult) + " "
                + model.StartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " "
                +
                MsgUtils.Instance.Trls("organiser", cult) + model.organiser
            };
            _hrUnitOfWork.MeetingRepository.AddNotifyLetter(NL);
            errors = Save(Language);
            return errors;
        }
        //Edit Location && start Time && end Time
        private List<Error> EditLocationAndTimesNotifyLetters(SaveMeetingViewModel model, int EmpId, string ChangedLocation, string PreviousLocation, DateTime PreviousStartTime, DateTime PreviousEndTime)
        {
            List<Error> errors = new List<Error>();
            var lang = _hrUnitOfWork.MeetingRepository.GetUsersLang(EmpId.ToString()).Select(s => s.name).FirstOrDefault();
            var cult = lang == null ? Language : lang;
            NotifyLetter NL = new NotifyLetter()
            {
                CompanyId = CompanyId,
                EmpId = EmpId,
                NotifyDate = DateTime.Now,
                NotifySource = MsgUtils.Instance.Trls("MeetingModified", cult),
                SourceId = model.Id.ToString(),
                Sent = false,
                EventDate = model.MeetDate,
                Description = MsgUtils.Instance.Trls("EditLocationintroduction", cult) + " " + model.subjectText
                + " " + MsgUtils.Instance.Trls("fromLocation", cult) + " " +
                PreviousLocation + " " + MsgUtils.Instance.Trls("ToLocation", cult) + " " + ChangedLocation + " "
                + MsgUtils.Instance.Trls("EditEndTimeintroduction", cult) + " " +
                model.subjectText + " " + MsgUtils.Instance.Trls("fromHours", cult) + " " +
                PreviousEndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " "
                + MsgUtils.Instance.Trls("ToHours", cult) + " " +
                model.EndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " "
                + MsgUtils.Instance.Trls("EditStartTimeintroduction", cult) + " "
                + MsgUtils.Instance.Trls("fromHours", cult) + " " +
                PreviousStartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " "
                + MsgUtils.Instance.Trls("ToHours", cult) + " "
                + model.StartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " "
                +
                MsgUtils.Instance.Trls("organiser", cult) + model.organiser
            };
            _hrUnitOfWork.MeetingRepository.AddNotifyLetter(NL);
            errors = Save(Language);
            return errors;
        }
        // Edit Location && Meeting Date => Notify Letters To Attendee
        private List<Error> EditNotifyLetters(SaveMeetingViewModel model, int EmpId, string ChangedLocation, string PreviousLocation, DateTime PreviousMeetTime)
        {
            List<Error> errors = new List<Error>();
            var lang = _hrUnitOfWork.MeetingRepository.GetUsersLang(EmpId.ToString()).Select(s => s.name).FirstOrDefault();
            var cult = lang == null ? Language : lang;
            NotifyLetter NL = new NotifyLetter()
            {
                CompanyId = CompanyId,
                EmpId = EmpId,
                NotifyDate = DateTime.Now,
                NotifySource = MsgUtils.Instance.Trls("MeetingModified", cult),
                SourceId = model.Id.ToString(),
                Sent = false,
                EventDate = model.MeetDate,
                Description = MsgUtils.Instance.Trls("EditLocationintroduction", cult) + " " + model.subjectText + " " + MsgUtils.Instance.Trls("fromLocation", cult) + " " +
                PreviousLocation + " " + MsgUtils.Instance.Trls("ToLocation", cult) + " " + ChangedLocation + " " +
                MsgUtils.Instance.Trls("ChangeDateAlso", cult) + " " + model.MeetDate.ToString("dddd dd/MM/yyyy") + " " +
                MsgUtils.Instance.Trls("PreviousMeetingDate", cult) + PreviousMeetTime.ToString("dddd dd/MM/yyyy") + " " +
                MsgUtils.Instance.Trls("fromHours", cult) + model.StartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " +
                MsgUtils.Instance.Trls("ToHours", cult) + " " + model.EndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " +
                MsgUtils.Instance.Trls("organiser", cult) + model.organiser
            };
            _hrUnitOfWork.MeetingRepository.AddNotifyLetter(NL);
            errors = Save(Language);
            return errors;

        }
        //Cancel Meeting
        public ActionResult CancelMeeting(int Id, string MeetSubject, string Organiser)
        {
            List<Error> errors = new List<Error>();
            string message = "OK";
            string PreviousLocationText = "";
            var MeetingObject = _hrUnitOfWork.Repository<Meeting>().Where(s => s.Id == Id).FirstOrDefault();
            MeetingObject.Status = 3;
            _hrUnitOfWork.MeetingRepository.Attach(MeetingObject);
            _hrUnitOfWork.MeetingRepository.Entry(MeetingObject).State = EntityState.Modified;
            //Create Notify Letters To Attendee to Cancel 
            var trans = _hrUnitOfWork.BeginTransaction();
            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                message = Errors.First().errors.First().message;
                trans.Rollback();
                trans.Dispose();
            }
            //Get Meeting Attendee
            var dbAttendeObjects = _hrUnitOfWork.Repository<MeetAttendee>().Where(a => a.MeetingId == Id).Select(a => a.EmpId).ToList();
            var EmpIds = string.Join(",", dbAttendeObjects);

            //Get UserProfile Culture 
            var lang = _hrUnitOfWork.MeetingRepository.GetUsersLang(EmpIds);
            if (MeetingObject.BranchId != null)
                PreviousLocationText = _hrUnitOfWork.Repository<Site>().Where(a => a.Id == MeetingObject.BranchId).Select(s => s.Name).FirstOrDefault();
            else if (MeetingObject.LocationText != null)
                PreviousLocationText = MeetingObject.LocationText;

            foreach (var item in dbAttendeObjects)
            {
                var chkLang = lang.Where(a => a.id == item).Select(a => a.name).FirstOrDefault();
                var cult = chkLang == null ? Language : chkLang;
                NotifyLetter NL = new NotifyLetter()
                {
                    CompanyId = CompanyId,
                    EmpId = item,
                    NotifyDate = DateTime.Now,
                    NotifySource = MsgUtils.Instance.Trls("MeetingCancel", cult),
                    SourceId = Id.ToString(),
                    Sent = false,
                    EventDate = MeetingObject.MeetDate,
                    Description = MsgUtils.Instance.Trls("intoductionCancel", cult) + " " + MeetSubject + " " + MsgUtils.Instance.Trls("DefinedIn", cult) + " " + MsgUtils.Instance.Trls("fromHours", cult) + MeetingObject.StartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " +
                    MsgUtils.Instance.Trls("ToHours", cult) + " " + MeetingObject.EndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo(cult)) + " " + MsgUtils.Instance.Trls("In", cult) + " " + PreviousLocationText + " " + MsgUtils.Instance.Trls("Meeting Date", cult) + MeetingObject.MeetDate.ToString("dddd dd/MM/yyyy") + " " +
                    MsgUtils.Instance.Trls("organiser", cult) + Organiser
                };

                _hrUnitOfWork.MeetingRepository.AddNotifyLetter(NL);
            }

            errors = Save(Language);
            if (errors.Count > 0) return Json(errors.First().errors.First().message);
            trans.Commit();
            trans.Dispose();
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        //Get Attendee Count 
        public ActionResult GetAttendeeCount(int Id)
        {
            var count = _hrUnitOfWork.Repository<MeetAttendee>().Where(a => a.MeetingId == Id).Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        // fill Kendo Drop Down List Agenda Speakers
        public ActionResult FillSpeakers(string Ids)
        {
            var Emps = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Where(a => Ids.Contains(a.Id.ToString())).Select(a => new { value = a.Id, text = a.Employee }).ToList();
            return Json(Emps, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteMeeting(int Id)
        {
            List<Error> errors = new List<Error>();
            DataSource<MeetingViewModel> Source = new DataSource<MeetingViewModel>();
            Meeting meeting = _hrUnitOfWork.MeetingRepository.GetMeeting(Id);
            if (meeting != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = meeting,
                    ObjectName = "Meeting",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.MeetingRepository.Remove(meeting);

            }
            string message = "OK";

            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else

                return Json(message, JsonRequestBehavior.AllowGet);
        }
    }
    #endregion
}