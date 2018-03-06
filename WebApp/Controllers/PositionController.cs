using Interface.Core;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model.Domain;
using Model.ViewModel;
using WebApp.Extensions;
using System.Data.Entity;
using System.Linq.Dynamic;
using Model.Domain.Payroll;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class PositionController : BaseController
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
        public PositionController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        // GET: Position
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetPositions(int MenuId)
        {
            var query = _hrUnitOfWork.PositionRepository.GetPositions(Language, CompanyId);
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
        public ActionResult Details(int id = 0)
        {
            var Position = _hrUnitOfWork.PositionRepository.ReadPosition(id, Language) ?? new PositionViewModel();
            if (id != 0)
            {
                ViewBag.related = _hrUnitOfWork.EmployeeRepository.GetAssignments(Language).Where(a => a.PositionId == Position.Id).Count() > 0;
                ViewBag.job = _hrUnitOfWork.JobRepository.GetAllJobs(CompanyId, Language,Position.JobId).Select(a => new { id = a.Id, name = a.LocalName });
                ViewBag.Dept = _hrUnitOfWork.CompanyStructureRepository.GetAllDepartments(CompanyId, Position.DeptId, Language);
            }
            else
            {
                ViewBag.job = _hrUnitOfWork.JobRepository.GetAllJobs(CompanyId, Language,0).Select(a => new { id = a.Id, name = a.LocalName });
                ViewBag.Dept = _hrUnitOfWork.CompanyStructureRepository.GetAllDepartments(CompanyId,null, Language);
            }
            var Array = _hrUnitOfWork.PositionRepository.GetPositions(Language, CompanyId).Where(a => a.HiringStatus == 2).Select(a => a).ToList();
            Array.Remove(Position);
            RemoveChildren(Array, Position.Id);
            ViewBag.Sup = Array.Where(b => b.Id != Position.Id).Select(a => new { id = a.Id, name = a.Name }).ToList();
            ViewBag.Successor = _hrUnitOfWork.PositionRepository.GetPositions(Language, CompanyId).Where(a => a.HiringStatus == 2).Select(a => new { id = a.Id, name = a.Name }).ToList();
            ViewBag.Budget = _hrUnitOfWork.Repository<Budget>().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.Payroll = _hrUnitOfWork.Repository<Payrolls>().Select(a => new { id = a.Id, name = a.Name });
            return View(Position);
        }
        private void RemoveChildren(List<PositionViewModel> array, int id)
        {
            var child = array.Where(a => a.Supervisor == id).ToList();
            if (child.Count != 0)
            {
                for (int i = 0; i < child.Count; i++)
                {
                    array.Remove(child[i]);
                    RemoveChildren(array, child[i].Id);
                }
            }
        }

        [HttpPost]
        public ActionResult Details(PositionViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            var Pos = _hrUnitOfWork.PositionRepository.Get(model.Id);
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.PositionRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PositionForm",
                        TableName = "Positions",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
            var Sequence = _hrUnitOfWork.Repository<Position>().Select(a => a.Code).DefaultIfEmpty(0).Max();
            var MaxCode = Sequence == 0 ? 1 : Sequence + 1;
            if (Pos == null) // New
            {
                Pos = new Position();
                _hrUnitOfWork.PositionRepository.AddLName(Language, Pos.Name, model.Name, model.LName);
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = Pos,
                    Source = model,
                    ObjectName = "PositionForm",
                    Options = moreInfo,
                    Transtype = TransType.Insert
                });


                if (model.Seasonal != true)
                {
                    Pos.SeasonDay = null;
                    Pos.SeasonMonth = null;
                }
                Pos.Code = MaxCode;
                Pos.CompanyId = CompanyId;
                Pos.CreatedTime = DateTime.Now;
                Pos.CreatedUser = UserName;
                _hrUnitOfWork.PositionRepository.Add(Pos);

            }
            else // Edit
            {
                var Assignments = _hrUnitOfWork.Repository<Assignment>().Where(a => (a.PositionId == Pos.Id) && a.CompanyId == CompanyId).ToList();
                if (Pos.HiringStatus != model.HiringStatus)
                {
                    if (model.HiringStatus == 1 && Assignments.Count > 0)
                    {
                        ModelState.AddModelError("HiringStatus", MsgUtils.Instance.Trls("HiringStatusChange"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    else if ((model.HiringStatus == 4) && Assignments.Where(a => (a.EndDate > DateTime.Now) && (a.PositionId == Pos.Id) && (a.CompanyId == CompanyId)).Count() > 0)
                    {
                        ModelState.AddModelError("HiringStatus", MsgUtils.Instance.Trls("HiringStatusChange"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }
                _hrUnitOfWork.PositionRepository.AddLName(Language, Pos.Name, model.Name, model.LName);
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = Pos,
                    Source = model,
                    ObjectName = "PositionForm",
                    Options = moreInfo,
                    Transtype = TransType.Update
                });
                Pos.ModifiedTime = DateTime.Now;
                Pos.ModifiedUser = UserName;

                if (model.Seasonal != true)
                {
                    Pos.SeasonDay = null;
                    Pos.SeasonMonth = null;
                }
                _hrUnitOfWork.PositionRepository.Attach(Pos);
                _hrUnitOfWork.PositionRepository.Entry(Pos).State = EntityState.Modified;
            }
          //  errors = SaveGrid1(grid1, ModelState.Where(a => a.Key.Contains("grid1")), Pos);
            if (errors.Count > 0) return Json(errors.First().errors.First().message);

            var msg = "OK";
            errors = SaveChanges(User.Identity.GetLanguage());
            if (errors.Count > 0)
                msg = errors.First().errors.First().message;

            return Json(msg);
        }
        public ActionResult GetBudget(int budgetId, int positionId)
        {
            return Json(_hrUnitOfWork.PositionRepository.GetPeriods(budgetId, positionId), JsonRequestBehavior.AllowGet);
        }

        private List<Error> SaveGrid1(SubperiodVM grid1, IEnumerable<KeyValuePair<string, ModelState>> state, Position record)
        {
            List<Error> errors = new List<Error>();

            var dbBudget = _hrUnitOfWork.Repository<PosBudget>().Where(a => a.PositionId == record.Id).ToList();

            if (grid1.updated != null)
            {
                foreach (var model in grid1.updated)
                {
                    var positionBudget = dbBudget.FirstOrDefault(a => a.Id == model.Id);
                    if (positionBudget != null)
                    {
                        if (model.Amount != null)
                        {
                            positionBudget.BudgetAmount = model.Amount.Value;
                            _hrUnitOfWork.PositionRepository.Attach(positionBudget);
                            _hrUnitOfWork.PositionRepository.Entry(positionBudget).State = EntityState.Modified;
                        }
                        else
                            _hrUnitOfWork.PositionRepository.Remove(positionBudget);
                    }
                    else if ((model.Amount != null))
                    {
                        PosBudget pos = new PosBudget()
                        {
                            BudgetAmount = model.Amount.Value,
                            PositionId = record.Id,
                            SubPeriodId = model.SubPeriodId
                        };
                        _hrUnitOfWork.PositionRepository.Add(pos);
                    }
                }
            }


            //try
            //{
            //    _hrUnitOfWork.Save();

            //}
            //catch (Exception ex)
            //{

            //    ErrorMessage mess = new ErrorMessage()
            //    {
            //        message = MsgUtils.Instance.Trls(ex.Message)
            //    };
            //    Error er = new Error();
            //    er.errors.Add(mess);
            //    return errors;
            //}

            return errors;
        }
        public ActionResult DeletePositions(int Id)
        {
            var message = "OK";
            DataSource<PositionViewModel> Source = new DataSource<PositionViewModel>();
            Position Pos = _hrUnitOfWork.PositionRepository.Get(Id);
            var assignments = _hrUnitOfWork.EmployeeRepository.GetAssignments(Language).Where(a => a.PositionId == Pos.Id).ToList();
            if (Pos != null && assignments.Count == 0)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = assignments,
                    ObjectName = "Positions",
                    Transtype = TransType.Delete
                });


                _hrUnitOfWork.PositionRepository.Remove(Pos);
                _hrUnitOfWork.PositionRepository.RemoveLName(Language, Pos.Name);
                List<PosBudget> pb = _hrUnitOfWork.Repository<PosBudget>().Where(a => a.PositionId == Id).ToList();
                foreach (var item in pb)
                {
                    _hrUnitOfWork.PositionRepository.Remove(item);
                }

            }
            else
            {
                message = MsgUtils.Instance.Trls("ThereisAssignments");
                Source.Errors = new List<Error>() { new Error { errors = new List<ErrorMessage>() { new ErrorMessage() {
                message = message
                } } } };
                return Json(Source);
            }

            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);
        }
        public ActionResult Hierarchey()
        {
            ViewBag.PositionHierarchy = _hrUnitOfWork.Repository<Diagram>().Where(c => c.CompanyId == CompanyId).Select(a => new { id = a.Id, name = a.Name });

            var nodes = _hrUnitOfWork.PositionRepository.GetDiagram(Language, CompanyId);
            var all = nodes.ToList();
            var result = new List<PositionDiagram>();

            foreach (var node in all.Where(a => a.ParentId == null))
            {
                result.Add(new PositionDiagram
                {
                    Id = node.Id,
                    Name = node.Name,
                    HeadCount = node.HeadCount,
                    ParentId = node.ParentId,
                    Employee = node.Employee,
                    NoofHolder = node.NoofHolder,
                    Relief = node.Relief,
                    colorSchema = node.colorSchema,
                    Children = AddNodes(all, node.Id)
                });
            }


            return View(result);
        }
        private IList<PositionDiagram> AddNodes(IList<PositionDiagram> all, int ParentId)
        {
            return all.Where(a => a.ParentId == ParentId).Select(a => new PositionDiagram
            {
                Id = a.Id,
                Name = a.Name,
                HeadCount = a.HeadCount,
                ParentId = a.ParentId,
                Employee = a.Employee,
                NoofHolder = a.NoofHolder,
                Relief = a.Relief,
                colorSchema = a.colorSchema,
                Children = AddNodes(all, a.Id)
            }).ToList();
        }
        public ActionResult SaveDiagram(Diagram NewDiagram, IList<DiagramNode> DiagramNodes, OptionsViewModel moreInfo)
        {
            List<int> newnodes = new List<int>();




            for (int i = 0; i < DiagramNodes.Count; i++)
            {
                if (!DiagramNodes.Select(a => a.ChildId).Contains(DiagramNodes[i].ParentId))
                {
                    newnodes.Add(DiagramNodes[i].ParentId);
                }
            }

            foreach (var item in newnodes.Distinct())
            {
                DiagramNode node = new DiagramNode()
                {
                    ParentId = 0,
                    ChildId = item,
                    DiagramId = DiagramNodes[0].DiagramId
                };
                DiagramNodes.Add(node);
            }

            Diagram dd = _hrUnitOfWork.Repository<Diagram>().Where(a => a.Id == NewDiagram.Id).FirstOrDefault();

            if (dd != null)
            {
                if (NewDiagram.Name != null)
                {
                    dd.Color = NewDiagram.Color;
                    dd.EndDate = NewDiagram.EndDate;
                    dd.Name = NewDiagram.Name;
                    dd.StartDate = NewDiagram.StartDate;
                    dd.CompanyId = CompanyId;
                    dd.ModifiedTime = DateTime.Now;
                    dd.ModifiedUser = UserName;
                    _hrUnitOfWork.PositionRepository.Attach(dd);
                    _hrUnitOfWork.PositionRepository.Entry(dd).State = EntityState.Modified;
                }
                var ndi = _hrUnitOfWork.Repository<DiagramNode>().Where(a => a.DiagramId == dd.Id).Select(b => b);
                foreach (var item in ndi)
                {
                    _hrUnitOfWork.PositionRepository.Remove(item);

                }
            }
            else
            {
                dd = new Diagram();
                dd.Color = NewDiagram.Color;
                dd.EndDate = NewDiagram.EndDate;
                dd.Name = NewDiagram.Name;
                dd.StartDate = NewDiagram.StartDate;
                dd.CompanyId = CompanyId;
                dd.CreatedTime = DateTime.Now;
                dd.CreatedUser = UserName;
                _hrUnitOfWork.PositionRepository.Add(dd);

            }
            for (int i = 0; i < DiagramNodes.Count; i++)
            {
                DiagramNode di = new DiagramNode()
                {
                    DiagramId = dd.Id,
                    ChildId = DiagramNodes[i].ChildId,
                    ParentId = DiagramNodes[i].ParentId
                };
                _hrUnitOfWork.PositionRepository.Add(di);
            }


            try
            {
                _hrUnitOfWork.Save();

            }
            catch (Exception ex)
            {


                var msg = _hrUnitOfWork.HandleDbExceptions(ex, Language);
                if (msg.Length > 0)
                    return Json(msg);
            }
            return Json("Ok," + dd.Id);
        }
        public ActionResult RedrawDiagram(int diagramId)
        {
            List<PositionDiagram> lst;
            var result = new List<PositionDiagram>();

            if (diagramId != 0)
                lst = _hrUnitOfWork.PositionRepository.GetDiagramNodes(Language, diagramId).ToList();
            else
                lst = _hrUnitOfWork.PositionRepository.GetDiagram(Language, CompanyId).ToList();


            foreach (var node in lst.Where(a => diagramId != 0 ? a.ParentId == 0 : a.ParentId == null))
            {
                result.Add(new PositionDiagram
                {
                    Id = node.Id,
                    Name = node.Name,
                    HeadCount = node.HeadCount,
                    ParentId = node.ParentId,
                    Employee = node.Employee,
                    NoofHolder = node.NoofHolder,
                    Relief = node.Relief,
                    colorSchema = node.colorSchema,
                    Children = AddNodes(lst, node.Id)
                });
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private IList<ManagerEmployeeDiagram> AddNodes(IList<ManagerEmployeeDiagram> all, int ParentId)
        {
            return all.Where(a => a.ParentId == ParentId).Select(a => new ManagerEmployeeDiagram
            {

                Id = a.Id,
                Name = a.Name,
                ParentId = a.ParentId,
                colorSchema = a.colorSchema,
                PositionName = a.PositionName,
                Image = a.Image,
                Gender = a.Gender,
                Children = AddNodes(all, a.Id)
            }).ToList();
        }
        public ActionResult EmpDiagram()
        {
            var lst = _hrUnitOfWork.EmployeeRepository.EmployeesDiagram(CompanyId, Language);
            var all = lst.ToList();
            var result = new List<ManagerEmployeeDiagram>();

            foreach (var node in all.Where(a => a.ParentId == null))
            {
                result.Add(new ManagerEmployeeDiagram
                {
                    Id = node.Id,
                    Name = node.Name,
                    ParentId = node.ParentId,
                    colorSchema = node.colorSchema,
                    Image = node.Image,
                    Gender = node.Gender,
                    PositionName = node.PositionName,
                    Children = AddNodes(all, node.Id)
                });
            }
            return View(result);
        }
        public ActionResult GetDiagram(int diagramId)
        {
            var obj = _hrUnitOfWork.Repository<Diagram>().Where(a => a.Id == diagramId).FirstOrDefault();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteDiagram(int diagramId)
        {
            Diagram dd = _hrUnitOfWork.Repository<Diagram>().Where(a => a.Id == diagramId).FirstOrDefault();

            _hrUnitOfWork.PositionRepository.Remove(dd);
            try
            {
                _hrUnitOfWork.Save();

            }
            catch (Exception ex)
            {


                var msg = _hrUnitOfWork.HandleDbExceptions(ex, Language);
                if (msg.Length > 0)
                    return Json(msg);
            }
            return Json("Ok");
        }
        [HttpGet]
        public ActionResult Relief(int id)
        {
            var r = _hrUnitOfWork.PositionRepository.GetRelief(id, Language);

            return Json(r, JsonRequestBehavior.AllowGet);
        }

    }
}