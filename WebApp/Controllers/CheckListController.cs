using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Interface.Core;
using Model.Domain;
using Model.ViewModel.Personnel;
using System.Data.Entity;
using WebApp.Extensions;
using Model.ViewModel;
using System.Linq.Dynamic;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class CheckListController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        private string UserName { get; set; }
        private string Language { get; set; }
        private int CompanyId { get; set; }
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

        public CheckListController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        public CheckListController()
        {

        }
        #region CheckList
        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult CheckDefault(int? Id,int ListTypeId)
        {
            var Def = _hrUnitOfWork.Repository<CheckList>().Where(a => a.ListType == ListTypeId&& a.Default==true).Select(d=>d).FirstOrDefault();
            if (Def == null)
            {
                Def = new CheckList();
                Def.Default = true;

            }
            else if (Def.Id == Id)
                Def.Default = true;
            else if (Id != Def.Id)
                Def.Default = false;

            else
                Def.Default = false;
            return Json(Def, JsonRequestBehavior.AllowGet);

          
        }
        public ActionResult ReadCheckList(int MenuId)
        {

            var query = _hrUnitOfWork.CheckListRepository.GetCheckLists(Language,CompanyId);
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {

                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {

                    TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadCheckListTask(int ListId)
        {
            return Json(_hrUnitOfWork.CheckListRepository.ReadCheckListTask(ListId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int Id = 0)
        {
            //  List<int> EmpChecList = _hrUnitOfWork.CheckListRepository.ReadCheckListTask(Id).Select(a => a.EmpId.Value).ToList();
            // ViewBag.EmpId = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(Language,EmpChecList).Distinct().Select(a => new { value = a.Id, text = a.Employee }).Distinct().ToList();
            ViewBag.EmpId = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(CompanyId, Language).Select(a=> new {value=a.id,text=a.name });
            ViewBag.TaskCat= _hrUnitOfWork.LookUpRepository.GetLookUpCodes("EmpTaskCat", Language).Select(a => new { value = a.CodeId, text = a.Title }).ToList();
            if (Id == 0)
            {
                return View(new CheckListFormViewModel());

            }

            var CheckList = _hrUnitOfWork.CheckListRepository.ReadCheckList(Id, Language);
            return CheckList == null ? (ActionResult)HttpNotFound() : View(CheckList);

        }
        [HttpPost]
        public ActionResult Details(CheckListFormViewModel model, OptionsViewModel moreInfo, CheckListTaskVM grid1)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.LocationRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "CheckList",
                        TableName = "CheckLists",
                        ParentColumn = "CompanyId",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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

                CheckList record;
                //insert
                if (model.Id == 0)
                {
                    record = new CheckList();
                    _hrUnitOfWork.JobRepository.AddLName(Language, record.Name, model.Name, model.LocalName);
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "CheckList",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    record.CreatedUser = UserName;
                    record.CreatedTime = DateTime.Now;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    if (record.StartDate > record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndDateGthanStartDate"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    _hrUnitOfWork.CheckListRepository.Add(record);
                }
                //update
                else
                {
                    record = _hrUnitOfWork.Repository<CheckList>().FirstOrDefault(a => a.Id == model.Id);
                    _hrUnitOfWork.JobRepository.AddLName(Language, record.Name, model.Name, model.LocalName);

                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "CheckList",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    if (record.StartDate > record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndDateGthanStartDate"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    _hrUnitOfWork.CheckListRepository.Attach(record);
                    _hrUnitOfWork.CheckListRepository.Entry(record).State = EntityState.Modified;
                }

                // Save grid1
                errors = SaveGrid(grid1, ModelState.Where(a => a.Key.Contains("grid")), record);
                if (errors.Count > 0) return Json(errors.First().errors.First().message);

                errors = SaveChanges(Language);

                var message = "OK";
                if (errors.Count > 0) message = errors.First().errors.First().message;

                return Json(message);
            }

            return Json(Models.Utils.ParseFormErrors(ModelState));

        }

        private List<Error> SaveGrid(CheckListTaskVM grid1, IEnumerable<KeyValuePair<string, ModelState>> state, CheckList checkList)
        {
            List<Error> errors = new List<Error>();

            // Deleted
            if (grid1.deleted != null)
            {
                foreach (CheckListTaskViewModel model in grid1.deleted)
                {
                    var chlistTask = new ChecklistTask
                    {
                        Id = model.Id
                    };

                    _hrUnitOfWork.CheckListRepository.Remove(chlistTask);
                }
            }

            // Exclude delete models from sever side validations
            if (ServerValidationEnabled)
            {
                var modified = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted")));
                if (modified.Count > 0)
                {
                    errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "ChecklistTasks",
                        TableName= "ChecklistTasks",
                        Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0) return errors;
                }
            }

            // updated records
            if (grid1.updated != null)
            {
                foreach (CheckListTaskViewModel model in grid1.updated)
                {
                    var chlistTask = new ChecklistTask();
                    AutoMapper(new Models.AutoMapperParm { Destination = chlistTask, Source = model });
                    chlistTask.ModifiedTime = DateTime.Now;
                    chlistTask.ModifiedUser = UserName;
                    _hrUnitOfWork.CheckListRepository.Attach(chlistTask);
                    _hrUnitOfWork.CheckListRepository.Entry(chlistTask).State = EntityState.Modified;
                }
            }

            // inserted records
            if (grid1.inserted != null)
            {
                foreach (CheckListTaskViewModel model in grid1.inserted)
                {
                    var chlistTask = new ChecklistTask();
                    AutoMapper(new Models.AutoMapperParm { Destination = chlistTask, Source = model});
                    chlistTask.Checklist = checkList;
                    chlistTask.CreatedTime = DateTime.Now;
                    chlistTask.CreatedUser = UserName;
                    _hrUnitOfWork.CheckListRepository.Add(chlistTask);
                }
            }

            return errors;
        }
        public ActionResult Delete(int id)
        {
            var message = "OK";
            DataSource<CheckListViewModel> Source = new DataSource<CheckListViewModel>();

            CheckList checkListObj = _hrUnitOfWork.CheckListRepository.Get(id);
            if (checkListObj != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = checkListObj,
                    ObjectName = "ChecklistTasks",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.CheckListRepository.Remove(checkListObj);
                _hrUnitOfWork.CheckListRepository.RemoveLName(Language,checkListObj.Name);
            }
            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);


        }
        #endregion


        #region EmpList
        private void FillViewBag(int EmpId)
        {
            var Emp = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, EmpId, CompanyId).Distinct();
            ViewBag.EmpId = Emp != null ? Emp.Select(a => new { id = a.Id, name = a.Employee }).ToList() : null;
            ViewBag.EmpIdGrid = Emp != null ? Emp.Where(a => a.Id != EmpId).Select(a => new { value = a.Id, text = a.Employee }).ToList() : null;
            ViewBag.TaskCat = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("EmpTaskCat", Language).Select(a => new { value = a.CodeId, text = a.Title }).ToList();
            ViewBag.CheckList = _hrUnitOfWork.CheckListRepository.GetCheckLists(Language, CompanyId).Select(a => new { id = a.Id, name = a.Name }).ToList();
            if (Session["MenuId"] != null && Session["RoleId"] != null)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(Session["RoleId"].ToString(), int.Parse(Session["MenuId"].ToString()));
          
        }
        public ActionResult EmpListIndex()
        {
            ViewBag.checkList = _hrUnitOfWork.CheckListRepository.GetCheckLists(Language, CompanyId).Select(a => new { id = a.Id, name = a.Name }).ToList();
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult CopyEmplistDetails(int checkList)
        {
           
            var checklist = _hrUnitOfWork.CheckListRepository.Find(a => a.Id == checkList).FirstOrDefault();

            EmpChkList EmpList = _hrUnitOfWork.CheckListRepository.AddEmpChlst(checklist, UserName, null,CompanyId);
            _hrUnitOfWork.CheckListRepository.Add(EmpList);
            var checkTask = _hrUnitOfWork.CheckListRepository.ReadCheckListTask(checklist.Id).ToList();
            if (checkTask.Count > 0)
            {
                _hrUnitOfWork.CheckListRepository.AddEmpTask(checkTask, UserName, EmpList);
            }
            List<Error> errors = new List<Error>();
            errors = SaveChanges(Language);

            if (errors.Count > 0)
            {
                var message = errors.First().errors.First().message;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            var EmpLists = _hrUnitOfWork.CheckListRepository.ReadEmpList(EmpList.Id);
            FillViewBag(EmpList.EmpId == null ?0 : EmpList.EmpId.Value);

            return View("EmplistDetails",EmpLists);


        }
        public ActionResult EmplistDetails(int Id=0)
        {

            if (Id == 0)
            {
                FillViewBag(0);

                return View(new EmpChkListViewModel());

            }

            var EmpList = _hrUnitOfWork.CheckListRepository.ReadEmpList(Id);
            FillViewBag(EmpList.EmpId==null ? 0 : EmpList.EmpId.Value);

            return EmpList == null ? (ActionResult)HttpNotFound() : View(EmpList);
        }
        public ActionResult ReadEmpCheckList(int MenuId)
        {
            var query = _hrUnitOfWork.CheckListRepository.GetEmpCheckLists(Language, CompanyId);
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {

                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {

                    TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadEmpListTask(int ListId)
        {
            return Json(_hrUnitOfWork.CheckListRepository.ReadEmpListTask(ListId),JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmplistDelete(int id)
        {
            var message = "OK";
            DataSource<EmpChkListViewModel> Source = new DataSource<EmpChkListViewModel>();

            EmpChkList EmpcheckList = _hrUnitOfWork.Repository<EmpChkList>().Where(a => a.Id == id).FirstOrDefault();
            var EmpTaskslis = _hrUnitOfWork.Repository<EmpTask>().Where(a => a.EmpListId == id).ToList();
           if(EmpTaskslis.Count > 0)
            {
                foreach (var item in EmpTaskslis)
                {
                    _hrUnitOfWork.CheckListRepository.Remove(item);
                }
            }

            _hrUnitOfWork.CheckListRepository.Remove(EmpcheckList);
            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);
        }    
        [HttpPost]
        public ActionResult EmplistDetails(EmpChkListViewModel model, OptionsViewModel moreInfo, EmpTaskVM grid1)
        {
            List<Error> errors = new List<Error>();
            if(model.ManagerId == model.EmpId)
            {
                ModelState.AddModelError("EmpId", MsgUtils.Instance.Trls("ManagerCan'tbeequalEmp"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.LocationRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmpListForm",
                        TableName = "EmpChkLists",
                        ParentColumn = "ListId",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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

                EmpChkList record;
                //insert
                if (model.Id == 0)
                {
                    record = new EmpChkList();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "EmpListForm",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    record.CreatedUser = UserName;
                    record.CreatedTime = DateTime.Now;
                    record.CompanyId = CompanyId;
                    if (record.ListStartDate > record.ListEndDate)
                    {
                        ModelState.AddModelError("ListStartDate", MsgUtils.Instance.Trls("End must greater than start"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    _hrUnitOfWork.CheckListRepository.Add(record);
                }
                //update
                else
                {
                    record = _hrUnitOfWork.Repository<EmpChkList>().FirstOrDefault(a => a.Id == model.Id);
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "EmpListForm",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.CompanyId =  CompanyId;
                    if (record.ListStartDate > record.ListEndDate)
                    {
                        ModelState.AddModelError("ListEndDate", MsgUtils.Instance.Trls("End must greater than start"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }

                    if(model.Status == 2)
                    {
                        var EmpTasks = _hrUnitOfWork.Repository<EmpTask>().Where(a => a.EmpListId == model.Id).ToList();
                        if(EmpTasks.Count > 0)
                        {
                            foreach (var item in EmpTasks)
                            {
                                if(item.Status != 2)
                                {
                                    item.Status = 3;
                                    _hrUnitOfWork.CheckListRepository.Attach(item);
                                    _hrUnitOfWork.CheckListRepository.Entry(item).State = EntityState.Modified;
                                }
                            }
                        }
                    }
                    _hrUnitOfWork.CheckListRepository.Attach(record);
                    _hrUnitOfWork.CheckListRepository.Entry(record).State = EntityState.Modified;
                }

                // Save grid1
                errors = EmpTasksSaveGrid(grid1, ModelState.Where(a => a.Key.Contains("grid")), record);
                if (errors.Count > 0) return Json(errors.First().errors.First().message);

                errors = SaveChanges(Language);

                var message = "OK";
                if (errors.Count > 0) message = errors.First().errors.First().message;

                return Json(message);
            }

            return Json(Models.Utils.ParseFormErrors(ModelState));

        }
        private List<Error> EmpTasksSaveGrid(EmpTaskVM grid1, IEnumerable<KeyValuePair<string, ModelState>> state, EmpChkList checkList)
        {
            List<Error> errors = new List<Error>();
            DataSource<EmpTaskViewModel> Err = new DataSource<EmpTaskViewModel>();
            Err.Total = 0;
            // Exclude delete models from sever side validations
            if (ServerValidationEnabled)
            {
                var modified = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted")));
                if (modified.Count > 0)
                {
                    errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmplistTasks",
                        Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0) return errors;
                }
            }

            // Deleted
            if (grid1.deleted != null)
            {
                foreach (EmpTaskViewModel model in grid1.deleted)
                {
                    var chlistTask = new EmpTask
                    {
                        Id = model.Id
                    };

                    _hrUnitOfWork.CheckListRepository.Remove(chlistTask);
                }
            }



            // updated records
            if (grid1.updated != null)
            {
                short count = 0;
                foreach (EmpTaskViewModel model in grid1.updated)
                {
                    if (model.AssignedTime != null)
                    {
                         Err = _hrUnitOfWork.CheckListRepository.AddSubperiods(CompanyId, model.AssignedTime, model.Id, count, Language);
                    }

                    if (Err.Errors == null || Err.Errors.Count == 0)
                    {
                        model.SubPeriodId = Err.Total == 0 ? (int?)null : Err.Total;
                        var chlistTask = _hrUnitOfWork.Repository<EmpTask>().Where(a => a.Id == model.Id).FirstOrDefault();

                        AutoMapper(new Models.AutoMapperParm
                        {
                            Destination = chlistTask,
                            Source = model
                        });

                        chlistTask.ModifiedTime = DateTime.Now;
                        chlistTask.ExpectDur = model.ExpectDur;
                        chlistTask.ModifiedUser = UserName;
                        _hrUnitOfWork.CheckListRepository.Attach(chlistTask);
                        _hrUnitOfWork.CheckListRepository.Entry(chlistTask).State = EntityState.Modified;

                    }
                    else
                        errors.Add(Err.Errors.FirstOrDefault());

                    count++;
                }
            }

            // inserted records
            if (grid1.inserted != null)
            {
                short count = 0;
                foreach (EmpTaskViewModel model in grid1.inserted)
                {
                    if (model.AssignedTime != null)
                    {
                        if (model.AssignedTime != null)
                        {
                            Err = _hrUnitOfWork.CheckListRepository.AddSubperiods(CompanyId, model.AssignedTime, model.Id, count, Language);
                        }
                    }

                    if (Err.Errors == null || Err.Errors.Count == 0)
                    {
                        model.SubPeriodId = Err.Total == 0 ? (int?)null : Err.Total;
                        var chlistTask = new EmpTask();
                        AutoMapper(new Models.AutoMapperParm
                        {
                            Destination = chlistTask,
                            Source = model,
                            Transtype = TransType.Insert
                        });
                        if (model.EmpId == 0)
                            chlistTask.EmpId = null;

                        if (model.ManagerId == 0)
                            chlistTask.ManagerId = null;
                        chlistTask.EmpChklist = checkList;
                        chlistTask.CreatedTime = DateTime.Now;
                        chlistTask.CreatedUser = UserName;
                        chlistTask.ExpectDur = model.ExpectDur;
                        _hrUnitOfWork.CheckListRepository.Add(chlistTask);
                    }
                    else
                        errors.Add(Err.Errors.FirstOrDefault());

                    count++;
                }
            }
            return errors;
        }
        public ActionResult Updatestatus(int id)
        {
         //   RecurringJob.AddOrUpdate(() => ExcuteUpdate(id), Cron.Minutely());
            return Json(ExcuteUpdate(id), JsonRequestBehavior.AllowGet);
        }      
        public string ExcuteUpdate(int id)
        {

            var message = "Ok";
            List<Error> errors = new List<Error>();
            
            int EmpListstatus = _hrUnitOfWork.CheckListRepository.ReadEmpList(id).Status;
            int ImpCount = 0, NotImpCount = 0;
            var Tasks = _hrUnitOfWork.Repository<EmpTask>().Where(a => a.EmpListId == id).ToList();
            if ((Tasks.Count > 0) && (EmpListstatus != 2))
            {
                foreach (var Task in Tasks.Where(a => a.Status == 1))
                {
                    bool Expiered;
                    switch (Task.Unit)
                    {
                        case 1:
                            if (Task.Required)
                            {
                                if (Task.ExpectDur != null && Task.ExpectDur != 0)
                                {
                                    Expiered = Task.AssignedTime != null ? (Task.AssignedTime.Value.AddMinutes(Task.ExpectDur.Value) > DateTime.Now ? false : true) : true;
                                    if (Expiered)
                                    {
                                        Task.Status = 4;
                                        _hrUnitOfWork.CheckListRepository.Attach(Task);
                                        _hrUnitOfWork.CheckListRepository.Entry(Task).State = EntityState.Modified;
                                    }
                                    else
                                        ImpCount++;
                                }
                                else
                                    ImpCount++;

                            }
                            else
                            {
                                if (Task.ExpectDur != null && Task.ExpectDur != 0)
                                {
                                    Expiered = Task.AssignedTime != null ? (Task.AssignedTime.Value.AddMinutes(Task.ExpectDur.Value) > DateTime.Now ? false : true) : true;
                                    if (Expiered)
                                    {
                                        Task.Status = 4;
                                        _hrUnitOfWork.CheckListRepository.Attach(Task);
                                        _hrUnitOfWork.CheckListRepository.Entry(Task).State = EntityState.Modified;
                                    }
                                    else
                                        NotImpCount++;
                                }
                                else
                                    NotImpCount++;

                            }
                            break;
                        case 2:
                            if (Task.Required)
                            {
                                if (Task.ExpectDur != null && Task.ExpectDur != 0)
                                {
                                    Expiered = Task.AssignedTime != null ? (Task.AssignedTime.Value.AddMinutes(Task.ExpectDur.Value) > DateTime.Now ? false : true) : true;
                                    if (Expiered)
                                    {
                                        Task.Status = 4;
                                        _hrUnitOfWork.CheckListRepository.Attach(Task);
                                        _hrUnitOfWork.CheckListRepository.Entry(Task).State = EntityState.Modified;
                                    }
                                    else
                                        ImpCount++;
                                }
                                else
                                    ImpCount++;

                            }
                            else
                            {
                                if (Task.ExpectDur != null && Task.ExpectDur != 0)
                                {
                                    Expiered = Task.AssignedTime != null ? (Task.AssignedTime.Value.AddHours(Task.ExpectDur.Value) > DateTime.Now ? false : true) : true;
                                    if (Expiered)
                                    {
                                        Task.Status = 4;
                                        _hrUnitOfWork.CheckListRepository.Attach(Task);
                                        _hrUnitOfWork.CheckListRepository.Entry(Task).State = EntityState.Modified;
                                    }
                                    else
                                        NotImpCount++;
                                }
                                else
                                    NotImpCount++;

                            }
                            break;
                        case 4:
                            if (Task.Required)
                            {
                                if (Task.ExpectDur != null && Task.ExpectDur != 0)
                                {
                                    Expiered = Task.AssignedTime != null ? (Task.AssignedTime.Value.AddMinutes(Task.ExpectDur.Value) > DateTime.Now ? false : true) : true;
                                    if (Expiered)
                                    {
                                        Task.Status = 4;
                                        _hrUnitOfWork.CheckListRepository.Attach(Task);
                                        _hrUnitOfWork.CheckListRepository.Entry(Task).State = EntityState.Modified;
                                    }
                                    else
                                        ImpCount++;
                                }
                                else
                                    ImpCount++;

                            }
                            else
                            {
                                if (Task.ExpectDur != null && Task.ExpectDur != 0)
                                {
                                    Expiered = Task.AssignedTime != null ? (Task.AssignedTime.Value.AddDays(Task.ExpectDur.Value * 7) > DateTime.Now ? false : true) : true;
                                    if (Expiered)
                                    {
                                        Task.Status = 4;
                                        _hrUnitOfWork.CheckListRepository.Attach(Task);
                                        _hrUnitOfWork.CheckListRepository.Entry(Task).State = EntityState.Modified;
                                    }
                                    else
                                        NotImpCount++;
                                }
                                else
                                    NotImpCount++;

                            }
                            break;
                        case 5:
                            if (Task.Required)
                            {
                                if (Task.ExpectDur != null && Task.ExpectDur != 0)
                                {
                                    Expiered = Task.AssignedTime != null ? (Task.AssignedTime.Value.AddMinutes(Task.ExpectDur.Value) > DateTime.Now ? false : true) : true;
                                    if (Expiered)
                                    {
                                        Task.Status = 4;
                                        _hrUnitOfWork.CheckListRepository.Attach(Task);
                                        _hrUnitOfWork.CheckListRepository.Entry(Task).State = EntityState.Modified;
                                    }
                                    else
                                        ImpCount++;
                                }
                                else
                                    ImpCount++;

                            }
                            else
                            {
                                if (Task.ExpectDur != null && Task.ExpectDur != 0)
                                {
                                    Expiered = Task.AssignedTime != null ? (Task.AssignedTime.Value.AddMonths(Task.ExpectDur.Value) > DateTime.Now ? false : true) : true;
                                    if (Expiered)
                                    {
                                        Task.Status = 4;
                                        _hrUnitOfWork.CheckListRepository.Attach(Task);
                                        _hrUnitOfWork.CheckListRepository.Entry(Task).State = EntityState.Modified;
                                    }
                                    else
                                        NotImpCount++;
                                }
                                else
                                    NotImpCount++;

                            }
                            break;
                        default:
                            if (Task.Required)
                            {
                                if (Task.ExpectDur != null && Task.ExpectDur != 0)
                                {
                                    Expiered = Task.AssignedTime != null ? (Task.AssignedTime.Value.AddMinutes(Task.ExpectDur.Value) > DateTime.Now ? false : true) : true;
                                    if (Expiered)
                                    {
                                        Task.Status = 4;
                                        _hrUnitOfWork.CheckListRepository.Attach(Task);
                                        _hrUnitOfWork.CheckListRepository.Entry(Task).State = EntityState.Modified;
                                    }
                                    else
                                        ImpCount++;
                                }
                                else
                                    ImpCount++;

                            }
                            else
                            {
                                if (Task.ExpectDur != null && Task.ExpectDur != 0)
                                {
                                    Expiered = Task.AssignedTime != null ? (Task.AssignedTime.Value.AddDays(Task.ExpectDur.Value) > DateTime.Now ? false : true) : true;
                                    if (Expiered)
                                    {
                                        Task.Status = 4;
                                        _hrUnitOfWork.CheckListRepository.Attach(Task);
                                        _hrUnitOfWork.CheckListRepository.Entry(Task).State = EntityState.Modified;
                                    }
                                    else
                                        NotImpCount++;
                                }
                                else
                                    NotImpCount++;

                            }
                            break;
                    }

                }

                if (ImpCount + NotImpCount + Tasks.Where(a => a.Status == 0).Count() == 0)
                {
                    EmpChkList Emplist = _hrUnitOfWork.Repository<EmpChkList>().FirstOrDefault(a => a.Id == id && a.CompanyId == CompanyId);

                    Emplist.Status = 2;
                    _hrUnitOfWork.CheckListRepository.Attach(Emplist);
                    _hrUnitOfWork.CheckListRepository.Entry(Emplist).State = EntityState.Modified;
                    message += "Canceled";
                }
                else
                {
                    if (ImpCount == 0 && Tasks.Where(a => a.Status == 0).Count() > 0)
                    {
                        var res = Tasks.Where(b => b.Status == 0).OrderBy(a => a.Priority).FirstOrDefault();
                        foreach (var item in Tasks.Where(b => b.Status == 0 && b.Priority == res.Priority).ToList())
                        {
                            item.Status = 1;
                            _hrUnitOfWork.CheckListRepository.Attach(item);
                            _hrUnitOfWork.CheckListRepository.Entry(item).State = EntityState.Modified;
                        }
                        message = "Updated";

                    }
                    else
                        message = (ImpCount + NotImpCount).ToString() + " AlreadyAssigned";
                }

                if (message.Contains("Updated"))
                {
                    errors = SaveChanges(Language);
                    if (errors.Count > 0)
                        message = errors.First().errors.First().message;
                }
            }
            else
                return message + "NoTask";
            return message;

        }

        #endregion
    }
}