using Interface.Core;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApp.Extensions;
using Model.Domain;
using Model.ViewModel;
using System.Data.Entity;
using System.Web.Script.Serialization;
using System.Linq.Dynamic;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class CustodyController : BaseController
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
        public CustodyController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        #region The Permenant Custody
        public ActionResult Index(bool isEmp = false)
        {
            ViewBag.isEmp = isEmp;
            return View();
        }
        //ReadCustody
        public ActionResult ReadCustody(int MenuId, byte? Range, DateTime? Start, DateTime? End)
        {
            var query = _hrUnitOfWork.CustodyRepository.ReadCustody(Range ?? 10, Start, End, Language, CompanyId);
            string whereclause = GetWhereClause(MenuId);
            if (whereclause.Length > 0)
            {
                try
                {
                    query = query.Where(whereclause);
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
        // GET: Convenant
        public ActionResult Details(int id = 0)
        {
            bool Disposal = false;
            var Custody = _hrUnitOfWork.CustodyRepository.ReadCustObject(id, Language);
            ViewBag.Currency = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            ViewBag.Names = _hrUnitOfWork.CustodyRepository.CustodyNames(false);
            ViewBag.jobs = _hrUnitOfWork.JobRepository.GetAllJobs(CompanyId, Language, 0).Select(a => new { id = a.Id, name = a.LocalName }).ToList();
            ViewBag.Branches = _hrUnitOfWork.Repository<Branch>().Where(a => a.CompanyId == CompanyId).Select(a => new { id = a.Id, name = a.Name }).ToList();
            ViewBag.CustodyCategory = _hrUnitOfWork.CustodyRepository.fillCatCustody(Disposal, Language);
            if (id == 0)
                return View(new CustodyFormViewModel());
            return Custody == null ? (ActionResult)HttpNotFound() : View(Custody);
        }
        public ActionResult SaveCustody(CustodyFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Custody",
                        TableName = "Custody",
                        Columns = Models.Utils.GetColumnViews(ModelState),
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
                var CustodyCategory = _hrUnitOfWork.Repository<CustodyCat>().Where(a => a.Id == model.CustodyCatId).Select(s => new { prefix = s.Prefix, CodeLen = s.CodeLength == 0 ? 4 : s.CodeLength }).FirstOrDefault();
                var record = _hrUnitOfWork.Repository<Custody>().FirstOrDefault(j => j.Id == model.Id);
                if (record == null) //Add
                {
                    record = new Custody();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "Custody",
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    //_hrUnitOfWork.CustodyRepository.AddTrail(new AddTrailViewModel { ValueBefore = record.CustodyCatId.ToString(), ValueAfter = model.CustodyCat.ToString(), CompanyId = CompanyId, ObjectName = "Custody",  Transtype = (byte)TransType.Insert, SourceId = record.Id.ToString(), ColumnName = "CustodyCatId", UserName = UserName });

                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    record.CompanyId = CompanyId;
                    record.Qty = 1;
                    record.InUse = false;
                    if (CustodyCategory.prefix == null)
                    {
                        var GenralSeq = _hrUnitOfWork.Repository<Custody>().Where(a => a.CompanyId == CompanyId).Select(a => a.Sequence).DefaultIfEmpty(0).Max();
                        var sequence = GenralSeq != 0 ? GenralSeq + 1 : 1;
                        record.Sequence = sequence;
                        record.Code = sequence.ToString();
                    }
                    else
                    {
                        var CategorySeq = _hrUnitOfWork.Repository<Custody>().Where(a => a.CustodyCatId == model.CustodyCatId).Select(a=>a.Sequence).DefaultIfEmpty(0).Max();
                        var sequence = CategorySeq != 0 ? CategorySeq + 1 : 1;
                        record.Sequence = sequence;
                        record.Code = CustodyCategory.prefix + sequence.ToString().PadLeft((int)CustodyCategory.CodeLen, '0');
                    }
                    _hrUnitOfWork.CustodyRepository.Add(record);

                }
                else //update
                {
                    var ExitCode = record.Code;
              
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "Custody",
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    //if(record.CustodyCatId != model.CustodyCat)
                    //_hrUnitOfWork.CustodyRepository.AddTrail(new AddTrailViewModel { ValueBefore = record.CustodyCatId.ToString(), ValueAfter = model.CustodyCat.ToString(),CompanyId=CompanyId,ObjectName= "Custody", Transtype =(byte)TransType.Update,SourceId=record.Id.ToString(),ColumnName = "CustodyCatId",UserName = UserName });
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.Code = ExitCode;
                    record.CompanyId = CompanyId;
                    _hrUnitOfWork.CustodyRepository.Attach(record);
                    _hrUnitOfWork.CustodyRepository.Entry(record).State = EntityState.Modified;

                }

                if (errors.Count > 0) return Json(errors.First().errors.First().message);
                var Errors = SaveChanges(Language);
                model.Id = record.Id;
                model.Code = record.Code;               
                string message = "OK," + ((new JavaScriptSerializer()).Serialize(model));
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }
        public ActionResult DeleteCustody(int id)
        {
            List<Error> errors = new List<Error>();
            DataSource<CustodyViewModel> Source = new DataSource<CustodyViewModel>();
            Custody custody = _hrUnitOfWork.CustodyRepository.GetCustody(id);
            if (custody != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = custody,
                    ObjectName = "Custody",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.CustodyRepository.Remove(custody);

            }
            string message = "OK";

            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else

                return Json(message);
        }
        //ReadEmpCustody
        public ActionResult ReadEmpCustody()
        {
            return Json(_hrUnitOfWork.CustodyRepository.ReadEmpCustody(CompanyId, Language),JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Button Recieve
        public ActionResult GetCurrentBranch(int EmpId)
        {
            var branchId = _hrUnitOfWork.Repository<Assignment>().Where(a => a.EmpId == EmpId && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today).Select(a => a.BranchId).FirstOrDefault();
            return Json(branchId, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RecieveDetails(int id = 0, byte Version = 0,bool edit = false,int EmpCustodyId = 0)
        {
            var RecieveCustody = _hrUnitOfWork.CustodyRepository.ReadRecievedCustody(id, Language);
            if (edit == true)
            {
                RecieveCustody = _hrUnitOfWork.CustodyRepository.ReadEditRecievedCustody(EmpCustodyId, Language);
            }
                     
            ViewBag.Branches = _hrUnitOfWork.Repository<Branch>().Where(a => a.CompanyId == CompanyId).Select(a => new { id = a.Id, name = a.Name });
            ViewBag.CustodyCategory = _hrUnitOfWork.CustodyRepository.GetCatCustody(Language);
         
            if (!_hrUnitOfWork.LeaveRepository.CheckAutoCompleteColumn("RecieveCustody", CompanyId, Version, "EmpId"))
                ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { id = a.Id, name = a.Employee });

            return View(RecieveCustody);
        }      
        public ActionResult SaveRecieveCustody(RecievedCustodyForm model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "RecieveCustody",
                        TableName = "EmpCustodies",
                        Columns = Models.Utils.GetColumnViews(ModelState),
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
                var custodyRecord = _hrUnitOfWork.Repository<Custody>().Where(a => a.Id == model.CustodyId).FirstOrDefault();
                var EmpCustody = _hrUnitOfWork.Repository<EmpCustody>().Where(a => a.Id == model.Id).FirstOrDefault();

                if (EmpCustody == null) //Add
                {
                    EmpCustody = new EmpCustody();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = EmpCustody,
                        Source = model,
                        ObjectName = "RecieveCustody",
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    EmpCustody.CreatedTime = DateTime.Now;
                    EmpCustody.CreatedUser = UserName;
                    EmpCustody.CompanyId = CompanyId;
                    EmpCustody.Qty = 1;
                    // EmpCustody.InUse = false;
                    if (custodyRecord.PurchaseDate > EmpCustody.RecvDate)
                    {
                        ModelState.AddModelError("RecvDate", MsgUtils.Instance.Trls("PurchaseMustlessthanRec")+" "+ custodyRecord.PurchaseDate.Value.ToString("d"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }

                    var DeleverDate = _hrUnitOfWork.Repository<EmpCustody>().Where(a => a.CustodyId == model.CustodyId).Select(a => a.delvryDate).DefaultIfEmpty(new DateTime(2099, 12, 31)).Max();
                    if (DeleverDate != new DateTime(2099, 12, 31))
                    {
                        if (EmpCustody.RecvDate < DeleverDate)
                        {
                            ModelState.AddModelError("RecvDate", MsgUtils.Instance.Trls("RecieveMustGrThanDelver")+" "+DeleverDate.Value.ToString("d"));
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                    }
                    _hrUnitOfWork.CustodyRepository.Add(EmpCustody);
                    custodyRecord.InUse = true;
                    _hrUnitOfWork.CustodyRepository.Attach(custodyRecord);
                    _hrUnitOfWork.CustodyRepository.Entry(custodyRecord).State = EntityState.Modified;
                }
                else // Update
                {
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = EmpCustody,
                        Source = model,
                        ObjectName = "RecieveCustody",
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    if (custodyRecord.PurchaseDate > EmpCustody.RecvDate)
                    {
                        ModelState.AddModelError("RecvDate", MsgUtils.Instance.Trls("PurchaseMustlessthanRec") + " " + custodyRecord.PurchaseDate.Value.ToString("d"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }

                    var DeleverDate = _hrUnitOfWork.Repository<EmpCustody>().Where(a => a.CustodyId == model.CustodyId).Select(a => a.delvryDate).DefaultIfEmpty(new DateTime(2099, 12, 31)).Max();
                    if (DeleverDate != new DateTime(2099, 12, 31))
                    {
                        if (EmpCustody.RecvDate < DeleverDate)
                        {
                            ModelState.AddModelError("RecvDate", MsgUtils.Instance.Trls("RecieveMustGrThanDelver") + " " + DeleverDate.Value.ToString("d"));
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                    }
                    _hrUnitOfWork.CustodyRepository.Attach(EmpCustody);
                    _hrUnitOfWork.CustodyRepository.Entry(EmpCustody).State = EntityState.Modified;
                    custodyRecord.Status = model.RecvStatus;
                    _hrUnitOfWork.CustodyRepository.Attach(custodyRecord);
                    _hrUnitOfWork.CustodyRepository.Entry(custodyRecord).State = EntityState.Modified;
                }
                if (errors.Count > 0) return Json(errors.First().errors.First().message);
                var Errors = SaveChanges(Language);
                model.Id = EmpCustody.Id;
                string message = "OK," + ((new JavaScriptSerializer()).Serialize(model));
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;
                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }
        #endregion

        #region Button Delever
        public ActionResult DeleverDetails(int id = 0, byte Version = 0, int EmpId = 0, int EmpCustodyId = 0)
        {
            var DeleverCustody = _hrUnitOfWork.CustodyRepository.ReadDeleverCustody(id, EmpId, EmpCustodyId, CompanyId, Language);
            ViewBag.Branches = _hrUnitOfWork.Repository<Branch>().Where(a => a.CompanyId == CompanyId).Select(a => new { id = a.Id, name = a.Name });
            ViewBag.CustodyCategory = _hrUnitOfWork.CustodyRepository.GetCatCustody(Language);
            
            if (!_hrUnitOfWork.LeaveRepository.CheckAutoCompleteColumn("DeleverCustody", CompanyId, Version, "EmpId"))
                ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { id = a.Id, name = a.Employee });

            return View(DeleverCustody);
        }
        public ActionResult SaveDeleverCustody(DeleverCustodyFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "DeleverCustody",
                        TableName = "EmpCustodies",
                        Columns = Models.Utils.GetColumnViews(ModelState),
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
                if (model.delvryDate == null)
                {
                    ModelState.AddModelError("delvryDate", MsgUtils.Instance.Trls("Required"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                var custodyRecord = _hrUnitOfWork.Repository<Custody>().Where(a => a.Id == model.CustodyId).FirstOrDefault();
                var RcvDate = _hrUnitOfWork.Repository<EmpCustody>().Where(a => a.Id == model.EmpCustodyId && a.EmpId == model.EmpId).Select(s => s.RecvDate).Max();
                var EmpCustodyRecord = _hrUnitOfWork.Repository<EmpCustody>().Where(a => a.Id == model.Id).FirstOrDefault();
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = EmpCustodyRecord,
                    Source = model,
                    ObjectName = "DeleverCustody",
                    Options = moreInfo,
                    Transtype = TransType.Update
                });
                EmpCustodyRecord.ModifiedTime = DateTime.Now;
                EmpCustodyRecord.ModifiedUser = UserName;
                EmpCustodyRecord.CompanyId = CompanyId;
                EmpCustodyRecord.Qty = 1;
                if (custodyRecord.PurchaseDate >= EmpCustodyRecord.delvryDate)
                {
                    ModelState.AddModelError("", MsgUtils.Instance.Trls("PurchaseMustlessthanDel"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                _hrUnitOfWork.CustodyRepository.Attach(EmpCustodyRecord);
                _hrUnitOfWork.CustodyRepository.Entry(EmpCustodyRecord).State = EntityState.Modified;
                custodyRecord.InUse = false;
                custodyRecord.Status = (byte)model.delvryStatus;
                _hrUnitOfWork.CustodyRepository.Attach(custodyRecord);
                _hrUnitOfWork.CustodyRepository.Entry(custodyRecord).State = EntityState.Modified;
                if (errors.Count > 0) return Json(errors.First().errors.First().message);
                var Errors = SaveChanges(Language);
                string message = "OK," + ((new JavaScriptSerializer()).Serialize(EmpCustodyRecord));
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;
                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }

        #endregion

        #region End of the Covenant
        public ActionResult EndCovenantIndex()
        {
            return View();
        }
        public ActionResult EndDetails(int id = 0)
        {
            var Custody = _hrUnitOfWork.CustodyRepository.ReadCustObject(id, Language);
            ViewBag.jobs = _hrUnitOfWork.JobRepository.GetAllJobs(CompanyId, Language, 0).Select(a => new { id = a.Id, name = a.LocalName }).ToList();
            ViewBag.Branches = _hrUnitOfWork.Repository<Branch>().Where(a => a.CompanyId == CompanyId).Select(a => new { id = a.Id, name = a.Name }).ToList();
            ViewBag.CustodyCategory = _hrUnitOfWork.CustodyRepository.GetCatCustody(Language);
            ViewBag.Currency = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            if (id == 0)
                return View(new CustodyFormViewModel());
            return Custody == null ? (ActionResult)HttpNotFound() : View(Custody);
        }

        #endregion

        #region Consuming Covenant
        public ActionResult ConsumeIndex()
        {
            return View();
        }
        public ActionResult ReadConsumeCustody(byte? Range, DateTime? Start, DateTime? End)
        {
            var query = _hrUnitOfWork.CustodyRepository.ReadConsumeCustody(Range ?? 10, Start, End, Language, CompanyId);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsumeDetails(int id = 0)
        {
            var Disposal = true;
            var Custody = _hrUnitOfWork.CustodyRepository.ReadCustObject(id, Language);
            ViewBag.Currency = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            ViewBag.Names = _hrUnitOfWork.CustodyRepository.CustodyNames(true);
            ViewBag.CustodyCategory = _hrUnitOfWork.CustodyRepository.fillCatCustody(Disposal, Language);
            ViewBag.jobs = _hrUnitOfWork.JobRepository.GetAllJobs(CompanyId, Language, 0).Select(a => new { id = a.Id, name = a.LocalName }).ToList();
            ViewBag.Branches = _hrUnitOfWork.Repository<Branch>().Where(a => a.CompanyId == CompanyId).Select(a => new { id = a.Id, name = a.Name }).ToList();
            if (id == 0)
                return View(new CustodyFormViewModel());
            return Custody == null ? (ActionResult)HttpNotFound() : View(Custody);
        }
        public ActionResult SaveConsumeCustody(CustodyFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "ConsumeFormCustody",
                        TableName = "Custody",
                        Columns = Models.Utils.GetColumnViews(ModelState),
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
                var CustodyCategory = _hrUnitOfWork.Repository<CustodyCat>().Where(a => a.Id == model.CustodyCatId).Select(s => new { prefix = s.Prefix, CodeLen = s.CodeLength == 0 ? 4 : s.CodeLength }).FirstOrDefault();
                var record = _hrUnitOfWork.Repository<Custody>().FirstOrDefault(j => j.Id == model.Id);
                if (record == null) //Add
                {
                    record = new Custody();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "ConsumeFormCustody",
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    record.CompanyId = CompanyId;
                    record.InUse = false;
                    if (CustodyCategory.prefix == null)
                    {
                        var GenralSeq = _hrUnitOfWork.Repository<Custody>().Where(a => a.CompanyId == CompanyId).Select(a => a.Sequence).DefaultIfEmpty(0).Max();
                        var sequence = GenralSeq != 0 ? GenralSeq + 1 : 1;
                        record.Sequence = sequence;
                        record.Code =sequence.ToString();
                    }
                    else
                    {
                        var CategorySeq = _hrUnitOfWork.Repository<Custody>().Where(a => a.CustodyCatId == model.CustodyCatId).Select(a => a.Sequence).DefaultIfEmpty(0).Max();
                        var sequence = CategorySeq != 0 ? CategorySeq + 1 : 1;
                        record.Sequence = sequence;
                        record.Code = CustodyCategory.prefix + sequence.ToString().PadLeft((int)CustodyCategory.CodeLen, '0');
                    }
                    _hrUnitOfWork.CustodyRepository.Add(record);

                }
                else //update
                {
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "ConsumeFormCustody",
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                     record.CompanyId = CompanyId;
                    _hrUnitOfWork.CustodyRepository.Attach(record);
                    _hrUnitOfWork.CustodyRepository.Entry(record).State = EntityState.Modified;

                }
                if (errors.Count > 0) return Json(errors.First().errors.First().message);
                var Errors = SaveChanges(Language);
                string message = "OK," + ((new JavaScriptSerializer()).Serialize(record));
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }
        public ActionResult RecieveConsumedCustody(float RestQty, int id = 0, byte Version = 0)
        {
            var RecieveCustody = _hrUnitOfWork.CustodyRepository.ReadRecievedCustody(id, Language);
            ViewBag.RestQty = RestQty;
            ViewBag.Branches = _hrUnitOfWork.Repository<Branch>().Where(a => a.CompanyId == CompanyId).Select(a => new { id = a.Id, name = a.Name });
            ViewBag.CustodyCategory = _hrUnitOfWork.CustodyRepository.GetCatCustody(Language);
            
            if (!_hrUnitOfWork.LeaveRepository.CheckAutoCompleteColumn("RecieveCustody", CompanyId, Version, "EmpId"))
                ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { id = a.Id, name = a.Employee });

            return View(RecieveCustody);
        }
        public ActionResult SaveRecievedConsumeCustody(RecievedCustodyForm model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "RecieveCustody",
                        TableName = "EmpCustodies",
                        Columns = Models.Utils.GetColumnViews(ModelState),
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
                var custodyRecord = _hrUnitOfWork.Repository<Custody>().Where(a => a.Id == model.CustodyId).FirstOrDefault();
                var record = new EmpCustody();
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = record,
                    Source = model,
                    ObjectName = "RecieveCustody",
                    Options = moreInfo,
                    Transtype = TransType.Insert
                });
                record.CreatedTime = DateTime.Now;
                record.CreatedUser = UserName;
                record.CompanyId = CompanyId;
                record.RecvStatus = 100;
                // record.InUse = false;
                if (custodyRecord.PurchaseDate >= record.RecvDate)
                {
                    ModelState.AddModelError("", MsgUtils.Instance.Trls("PurchaseMustlessthanRec"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                _hrUnitOfWork.CustodyRepository.Add(record);
                if (errors.Count > 0) return Json(errors.First().errors.First().message);
                var Errors = SaveChanges(Language);
                string message = "OK," + ((new JavaScriptSerializer()).Serialize(record));
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;
                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }
        public ActionResult DeleteConsumedCustody(int id)
        {
            List<Error> errors = new List<Error>();
            DataSource<CustodyViewModel> Source = new DataSource<CustodyViewModel>();
            Custody custody = _hrUnitOfWork.CustodyRepository.GetCustody(id);
            if (custody != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = custody,
                    ObjectName = "ConsumeCustody",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.CustodyRepository.Remove(custody);

            }
            string message = "OK";
            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else
                return Json(message);
        }
        public ActionResult ReadEmpConsumeCustody()
        {
            return Json(_hrUnitOfWork.CustodyRepository.ReadEmpConsumeCustody(CompanyId, Language), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteEmpConsumedCustody(int id)
        {
            List<Error> errors = new List<Error>();
            DataSource<CustodyViewModel> Source = new DataSource<CustodyViewModel>();
            EmpCustody EmpCustody = _hrUnitOfWork.CustodyRepository.GetEmpCustody(id);
            if (EmpCustody != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = EmpCustody,
                    ObjectName = "Custodies",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.CustodyRepository.Remove(EmpCustody);

            }
            string message = "OK";
            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else
                return Json(message);
        }
        #endregion

        #region Custody Category
        public ActionResult CustCategoryIndex()
        {
            return View();
        }
        //ReadCustCategory
        public ActionResult ReadCustCategory()
        {
            var query = _hrUnitOfWork.CustodyRepository.ReadCustCategory(Language);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        //CreateCustCategory
        public ActionResult CreateCustCategory(IEnumerable<CustodyCategoryViewModel> models)
        {
            var result = new List<CustodyCat>();
            var datasource = new DataSource<CustodyCategoryViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "CustodyCategory",
                        TableName = "CustodyCats",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                
                for (var i = 0; i < models.Count(); i++)
                {
                    // Create
                    if (models.ElementAtOrDefault(i).Id == 0)
                    {
                        var custCategory = new CustodyCat();
                        AutoMapper(new Models.AutoMapperParm
                        {
                            Destination = custCategory,
                            Source = models.ElementAtOrDefault(i),
                            ObjectName = "CustodyCategory",
                            Transtype = TransType.Insert
                        });
                        result.Add(custCategory);
                        custCategory.CreatedTime = DateTime.Now;
                        custCategory.CreatedUser = UserName;
                        _hrUnitOfWork.CustodyRepository.Add(custCategory);
                        _hrUnitOfWork.PositionRepository.AddLName(Language, null, models.ElementAtOrDefault(i).Name, models.ElementAtOrDefault(i).Title);
                    }

                }
                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from p in models
                               join r in result on p.Name equals r.Name
                               select new CustodyCategoryViewModel
                               {
                                   Id = r.Id,
                                   CodeLength = p.CodeLength,
                                   Prefix = p.Prefix,
                                   Disposal = p.Disposal,
                                   Title =p.Title,
                                   Name = p.Name
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateCustCategory(IEnumerable<CustodyCategoryViewModel> models)
        {
            var datasource = new DataSource<CustodyCategoryViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "CustodyCategory",
                        TableName = "CustodyCats",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var ids = models.Select(a => a.Id);
                var db_CustodyCat = _hrUnitOfWork.Repository<CustodyCat>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    var custodyCat = db_CustodyCat.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                        AutoMapper(new Models.AutoMapperParm
                        {
                            Destination = custodyCat,
                            Source = models.ElementAtOrDefault(i),
                            ObjectName = "CustodyCategory",
                            Transtype = TransType.Update
                        });
                        custodyCat.ModifiedTime = DateTime.Now;
                        custodyCat.ModifiedUser = UserName;
                        _hrUnitOfWork.CustodyRepository.Attach(custodyCat);
                        _hrUnitOfWork.CustodyRepository.Entry(custodyCat).State = EntityState.Modified;
                        _hrUnitOfWork.PositionRepository.AddLName(Language, models.ElementAtOrDefault(i).Name, models.ElementAtOrDefault(i).Name, models.ElementAtOrDefault(i).Title);
                    
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
            {
                return Json(datasource.Data);
            }
        }
        //Delete
        public ActionResult DeleteCustCategory(int id)
        {
            var datasource = new DataSource<CustodyCategoryViewModel>();
            CustodyCat CustodyCat = _hrUnitOfWork.CustodyRepository.GetCustodyCat(id);
            if (ModelState.IsValid)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = CustodyCat,
                    ObjectName = "CustodyCategory",
                    Transtype = TransType.Delete
                });
             
                _hrUnitOfWork.CustodyRepository.Remove(CustodyCat);
                _hrUnitOfWork.BenefitsRepository.RemoveLName(Language, CustodyCat.Name);
                datasource.Errors = SaveChanges(Language);
            }
            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
        #endregion

        #region Custody Report Link
        public ActionResult CustodyReport (int Id)
        {
            ViewBag.CustodyId = Id;
            return View();
        }
        public ActionResult GetCustodyReport(int Id)
        {
            var query = _hrUnitOfWork.CustodyRepository.GetCustodyReport(Id,Language);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Employee Document Borrow
        public ActionResult BorrowIndex()
        {
            return View();
        }
        //ReadDocBorrow
        public ActionResult ReadDocBorrow(int MenuId,byte? Range, DateTime? Start, DateTime? End)
        {
            var query = _hrUnitOfWork.CustodyRepository.ReadDocBorrow(Range ?? 10, Start, End, CompanyId,Language).AsQueryable();
            string whereclause = GetWhereClause(MenuId);
            if (whereclause.Length > 0)
            {
                try
                {
                    query = query.Where(whereclause);
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
 
        //Details Of Employee Recieve Form Document Borrow
        public ActionResult DetailsDocBorrow(int id = 0, byte Version = 0)
        {                  
            if (!_hrUnitOfWork.LeaveRepository.CheckAutoCompleteColumn("BorrowPapers", CompanyId, Version, "EmpId"))
                ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { id = a.Id, name = a.Employee });
            
            if (id == 0)
                return View(new EmpDocBorrowViewModel());

            var EmpDocBorrow = _hrUnitOfWork.CustodyRepository.ReadEmpDocBorrow(id, Language);
            ViewBag.EmpDocs = GetEmpDocType(EmpDocBorrow.EmpId);

            return EmpDocBorrow == null ? (ActionResult)HttpNotFound() : View(EmpDocBorrow);
        }
        //Get Employee Documents 
        public ActionResult GetDocType(int EmpId)
        {
            var empDocs = _hrUnitOfWork.CompanyRepository.GetDocsViews("People", EmpId);
            var BorrowIds = _hrUnitOfWork.Repository<EmpDocBorrow>().Where(a => a.EmpId == EmpId).Select(s => s.Id).ToList();
            var dbListOfInt = _hrUnitOfWork.Repository<DocBorrowList>().Where(a => BorrowIds.Contains(a.DocBorrowId)&& a.DocBorrow.delvryDate == null ).Select(s => s.DocId).ToList();
            var AllEmpDoc = empDocs.Where(w => w.TypeId != null).Select(s => new { id = s.TypeId, name = s.DocType.Name }).ToList();
            var EmpDocsList = AllEmpDoc.Where(s => !dbListOfInt.Contains(s.id.Value)).ToList();
            return Json(EmpDocsList,JsonRequestBehavior.AllowGet);
        }
        public object GetEmpDocType(int EmpId)
        {
            var empDocs = _hrUnitOfWork.CompanyRepository.GetDocsViews("People", EmpId);
            var EmpDocsList = empDocs.Where(w => w.TypeId != null).Select(a => new { id = a.TypeId, name = a.DocType.Name }).ToList();
            return EmpDocsList;
        }

        public ActionResult GetEditDocType(string DocumentIds,int EmpId)
        { 
            var dbListOfInt = new List<int>();
            var empDocs = _hrUnitOfWork.CompanyRepository.GetDocsViews("People", EmpId);
            var BorrowIds = _hrUnitOfWork.Repository<EmpDocBorrow>().Where(a => a.EmpId == EmpId).Select(s => s.Id).ToList();
            var x= _hrUnitOfWork.Repository<DocBorrowList>().Where(a => DocumentIds.Contains(a.DocId.ToString())).Select(s=>s.DocBorrowId).ToList();
             dbListOfInt = _hrUnitOfWork.Repository<DocBorrowList>().Where(a => BorrowIds.Contains(a.DocBorrowId) && a.DocBorrow.delvryDate == null).Select(s => s.DocId).ToList();
            foreach (var item in DocumentIds.Split(','))
            {
                if (dbListOfInt.Contains(int.Parse(item)))
                    dbListOfInt.Remove(int.Parse(item));
            }
            var AllEmpDoc = empDocs.Where(w => w.TypeId != null).Select(s => new { id = s.TypeId, name = s.DocType.Name }).ToList();
            var EmpDocsList = AllEmpDoc.Where(s => !dbListOfInt.Contains(s.id.Value)).ToList();
            return Json(EmpDocsList, JsonRequestBehavior.AllowGet);
        }

        // Save Borrow Document
        public ActionResult SaveBorrowPapers(EmpDocBorrowViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "BorrowPapers",
                        TableName = "EmpDocBorrows",
                        Columns = Models.Utils.GetColumnViews(ModelState),
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
                var ListOfIDs = new List<int>();
                foreach (var item in model.Document)
                {
                    ListOfIDs.Add(int.Parse(item));
                }
                var EmpBorrow = _hrUnitOfWork.Repository<EmpDocBorrow>().Where(a => a.Id == model.Id).FirstOrDefault();
                var LastDelveryDate = _hrUnitOfWork.Repository<DocBorrowList>().Where(a=>a.DocBorrow.EmpId == model.EmpId && ListOfIDs.Contains(a.DocId)).Select(s=>s.DocBorrow.delvryDate).DefaultIfEmpty(new DateTime(2099, 12, 31)).Max();
                if(LastDelveryDate != new DateTime(2099, 12, 31))
                {
                    if(model.RecvDate < LastDelveryDate)
                    {
                        ModelState.AddModelError("RecvDate", MsgUtils.Instance.Trls("RecieveMustGrThanDelvDoc") + " " + LastDelveryDate.Value.ToString("d"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }
                if (EmpBorrow == null) //Add
                {
                    EmpBorrow = new EmpDocBorrow();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = EmpBorrow,
                        Source = model,
                        ObjectName = "BorrowPapers",
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    EmpBorrow.CreatedTime = DateTime.Now;
                    EmpBorrow.CreatedUser = UserName;
                    EmpBorrow.CompanyId = CompanyId;                  
                    _hrUnitOfWork.CustodyRepository.Add(EmpBorrow);

                    if (model.Document != null)
                    {
                        foreach (var item in model.Document)
                        {
                            var doc = new DocBorrowList
                            {
                                DocBorrow = EmpBorrow,
                                DocId = int.Parse(item)
                            };
                            _hrUnitOfWork.CustodyRepository.Add(doc);
                        }
                    }
                }
                else // Update
                {
                 
                    var dbListOfInt = _hrUnitOfWork.Repository<DocBorrowList>().Where(a => a.DocBorrowId == EmpBorrow.Id).Select(s => s.DocId).ToList();
                    var dbBorrowObjects = _hrUnitOfWork.Repository<DocBorrowList>().Where(a => a.DocBorrowId == EmpBorrow.Id).ToList();
                    // Add new DocType
                    if (model.Document != null)
                    {
                        foreach (var item in model.Document)
                        {
                            if (!dbListOfInt.Contains(int.Parse(item)))
                            {
                                  var doc = new DocBorrowList
                                {
                                    DocBorrow = EmpBorrow,
                                    DocId = int.Parse(item)
                                };
                                _hrUnitOfWork.CustodyRepository.Add(doc);
                            }
                        }
                    }
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = EmpBorrow,
                        Source = model,
                        ObjectName = "BorrowPapers",
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    EmpBorrow.CompanyId = CompanyId;
                    _hrUnitOfWork.CustodyRepository.Attach(EmpBorrow);
                    _hrUnitOfWork.CustodyRepository.Entry(EmpBorrow).State = EntityState.Modified;
                }
                if (errors.Count > 0) return Json(errors.First().errors.First().message);
                var Errors = SaveChanges(Language);
                model.Id = EmpBorrow.Id;
                string message = "OK," + ((new JavaScriptSerializer()).Serialize(model));
                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }
        //Delever Docs Borrow 
        public ActionResult DeleverDocBorrow(int Id = 0, byte Version = 0)
        {
            var DeleverDoc = _hrUnitOfWork.CustodyRepository.ReadDeleverDocBorrow(Id);
            ViewBag.EmpDocs = GetEmpDocType(DeleverDoc.EmpId);
            
            if (!_hrUnitOfWork.LeaveRepository.CheckAutoCompleteColumn("DeleverCustody", CompanyId, Version, "EmpId"))
                ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { id = a.Id, name = a.Employee });

            return View(DeleverDoc);
        }

        //Save Delver Docs 
        public ActionResult SaveDeleverDocs(EmpDocBorrowViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "DeleverDocs",
                        TableName = "EmpDocBorrows",
                        Columns = Models.Utils.GetColumnViews(ModelState),
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
                if (model.delvryDate == null)
                {
                    ModelState.AddModelError("delvryDate", MsgUtils.Instance.Trls("Required"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                var EmpBorrowRecord = _hrUnitOfWork.Repository<EmpDocBorrow>().Where(a => a.Id == model.Id).FirstOrDefault();
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = EmpBorrowRecord,
                    Source = model,
                    ObjectName = "DeleverDocs",
                    Options = moreInfo,
                    Transtype = TransType.Update
                });
                EmpBorrowRecord.ModifiedTime = DateTime.Now;
                EmpBorrowRecord.ModifiedUser = UserName;
                EmpBorrowRecord.CompanyId = CompanyId;
                _hrUnitOfWork.CustodyRepository.Attach(EmpBorrowRecord);
                _hrUnitOfWork.CustodyRepository.Entry(EmpBorrowRecord).State = EntityState.Modified;
                //var dbBorrowObjects = _hrUnitOfWork.Repository<DocBorrowList>().Where(a => a.DocBorrowId == model.Id).ToList();
                //foreach (var item in model.Document)
                //{                   
                //        var doc = dbBorrowObjects.Where(a => a.DocId == int.Parse(item)&& a.DocBorrowId == model.Id).FirstOrDefault();
                //        _hrUnitOfWork.CustodyRepository.Remove(doc);                  
                //}
                if (errors.Count > 0) return Json(errors.First().errors.First().message);
                var Errors = SaveChanges(Language);
                string message = "OK," + ((new JavaScriptSerializer()).Serialize(EmpBorrowRecord));
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;
                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }

        #endregion
    }
}