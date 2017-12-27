using Interface.Core;
using Model.Domain;
using Model.Domain.Payroll;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers.Api
{
    public class DiscplinController : BaseApiController
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;


        public DiscplinController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        [ResponseType(typeof(EmpDisciplineViewModel)), HttpGet]
        [Route("api/Discplin/GetEmpiscplin")]
        public IHttpActionResult GetEmpiscplin(int MenuId)
        {
            var Discplin = _hrUnitOfWork.Repository<Discipline>().Select(a => new { text = a.Name, value = a.Id }).ToList();
            var query = _hrUnitOfWork.DisciplineRepository.ReadEmpDiscipline(User.Identity.GetLanguage(), User.Identity.GetDefaultCompany());
            IEnumerable<EmpDisciplineViewModel> queryResult = null;
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {
                try
                {
                    query = query.Where(whecls);
                    queryResult = query.ToList();
                    foreach (var item in queryResult)
                    {
                        item.Discplin = Discplin.Where(a => a.value == item.DiscplinId).Select(a => a.text).FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    //TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Ok("");
                }
            }
            return Ok(queryResult);
        }
        [ResponseType(typeof(PeopleTrainFormViewModel)), HttpGet]
        [Route("api/Discplin/GetEmpDiscplinObj")]
        public IHttpActionResult GetEmpDiscplinObj(int id = 0)
        {
            string culture = User.Identity.GetLanguage();
            var empId = _hrUnitOfWork.Repository<EmpDiscipline>().Where(a => a.Id == id).Select(a => a.EmpId).FirstOrDefault();
            var InvestigatId = _hrUnitOfWork.Repository<Investigation>().Select(a => new { id = a.Id, name = a.Name }).ToList();
            //List<string> columns = _hrUnitOfWork.PeopleRepository.GetAutoCompleteColumns("People", User.Identity.GetDefaultCompany(), Version);
            //if (columns.FirstOrDefault(fc => fc == "EmpId") == null)
            var EmpId = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(User.Identity.GetLanguage(), empId, User.Identity.GetDefaultCompany()).Select(a => new { id = a.Id, name = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus }).Distinct().ToList();
            var DiscplinId = _hrUnitOfWork.DisciplineRepository.SysDiscipline().Select(a => new { name = a.Name, id = a.Id, Systype = a.SysType }).ToList();

            EmpDisciplineFormViewModel EmpDisciplineObj;
            if (id == 0)
                EmpDisciplineObj = new EmpDisciplineFormViewModel();
            else
                EmpDisciplineObj = _hrUnitOfWork.DisciplineRepository.ReadEmployeeDiscipline(id);
            return Ok(new { DisciplineObj = EmpDisciplineObj, DiscplinId = DiscplinId, EmpId = EmpId, InvestigatId= InvestigatId });
        }
        [ResponseType(typeof(PeopleTrainFormViewModel)), HttpPost]
        [Route("api/Discplin/SaveEmpDiscplin")]
        public IHttpActionResult SaveEmpDiscplin(EmpDisciplineFormViewModel model)
        {
            return Ok();
        }
    }
}
