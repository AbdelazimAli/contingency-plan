using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Extensions;

namespace WebApp.Controllers.Api
{
    // [RoutePrefix("api/test")]
    public class CompanyController : BaseApiController
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;

        public CompanyController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        [ResponseType(typeof(CompanyViewModel)), HttpGet]
        [Route("api/Company/GetCompanies")]
        public IHttpActionResult GetCompanies()
        {
            return Ok(_hrUnitOfWork.CompanyRepository.GetAllCompanies(User.Identity.GetLanguage()));
        }

        [ResponseType(typeof(CompanyBranch)), HttpPost]
        [Route("api/Company/AddCompanyBranch")]
        public IHttpActionResult AddCompanyBranch(IEnumerable<CompanyBranch> branch)
        {
            if (ModelState.IsValid)
            {
                //_hrUnitOfWork.CompanyRepository.Add(branch);
            }

            return Ok(branch);
        }

        [HttpPost]
        [Route("api/Company/DeleteBranch")]
        public IHttpActionResult DeleteBranch(int id)
        {
            return Ok();
        }


        [HttpGet]
        [Route("api/Company/PurposeList")]
        public IHttpActionResult PurposeList()
        {
            return Json(_hrUnitOfWork.Repository<LookUpCode>().Where(c => c.CodeName == "Purpose").Select(c => new { Purpose = c.Name }).ToList());
        }


        [ResponseType(typeof(IQueryable<BranchesViewModel>)), HttpGet]
        [Route("api/Company/GetCompanyBranches")]
        public IHttpActionResult GetCompanyBranches()
        {
            return Ok(_hrUnitOfWork.CompanyRepository.GetCompanyBranches(1124));
        }
        // GET: api/Companies/5
        //[ResponseType(typeof(Company))]
        //public IHttpActionResult GetCompany(int id)
        //{
        //    Company company = _hrUnitOfWork.CompanyRepository.Get(id);

        //    if (company == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(company);
        //}

        //public IEnumerable GetCountries()
        //{
        //    return Ok(_hrUnitOfWork.Repository<Country>().Select(c => new { id = c.Name, name = c.Name }).ToList());
        //}
        // PUT: api/Companies/5
        //[ResponseType(typeof(void)), HttpPut]
        //public IHttpActionResult PutCompany(int id, Company company)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != company.Name)
        //    {
        //        return BadRequest();
        //    }

        //    _hrUnitOfWork.CompanyRepository.Entry(company).State = EntityState.Modified;
        //    try
        //    {
        //        _hrUnitOfWork.Save();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CompanyExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Companies

        [HttpPut]
        public IHttpActionResult UpdateBranch([FromUri]IEnumerable<BranchesViewModel> branches)
        {
            return StatusCode(HttpStatusCode.NoContent);
        }

   
        [ResponseType(typeof(Company)), HttpPost]
        public IHttpActionResult PostCompany(Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _hrUnitOfWork.CompanyRepository.Add(company);
            _hrUnitOfWork.Save();
           
            return CreatedAtRoute("DefaultApi", new { id = company.Id }, company);
        }

        // DELETE: api/Companies/5
        [ResponseType(typeof(Company)), HttpDelete]
        public IHttpActionResult DeleteCompany(int id)
        {
            Company company = _hrUnitOfWork.CompanyRepository.Get(id);
            if (company == null)
            {
                return NotFound();
            }

            _hrUnitOfWork.CompanyRepository.Remove(company);
            _hrUnitOfWork.Save();

            return Ok(company);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _hrUnitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompanyExists(int id)
        {
            return _hrUnitOfWork.CompanyRepository.IsExist(e => e.Id == id);
        }
    }
}