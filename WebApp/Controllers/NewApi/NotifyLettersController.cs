using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Interface.Core;
using System.Web.Http.Cors;
using System.ComponentModel.DataAnnotations;
using Model.ViewModel.Personnel;

namespace WebApp.Controllers.NewApi
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Model.Domain;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<NotifyLetter>("NotifyLetters");
    builder.EntitySet<Person>("People"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class LetterVM
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int EmpId { get; set; }
        public string Language { get; set; }
        public bool ReadParam { get; set; }
    }
    //public class ReturnData
    //{
    //    public IQueryable<NotifiyLetterViewModel> Letters { get; set; }
    //    public int Count { get; set; }
    //}
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NotifyLettersController : BaseODataController
    {

        protected IHrUnitOfWork _hrUnitOfWork { get; private set; }
        public NotifyLettersController(IHrUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        // GET: odata/NotifyLetters
        [EnableQuery]
        [AllowAnonymous]
        public IHttpActionResult Post([FromBody] LetterVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (model == null)
            {
                return NotFound();
            }

            var Letters = _hrUnitOfWork.NotificationRepository.GetMyLetters(model.CompanyId, model.Language, model.EmpId).Where(l => l.read == model.ReadParam).AsQueryable();


            return Ok(Letters);

        }

        // GET: odata/NotifyLetters(5)


        // PUT: odata/NotifyLetters(5)


        // POST: odata/NotifyLetters

        // PATCH: odata/NotifyLetters(5)


        // DELETE: odata/NotifyLetters(5)

        // GET: odata/NotifyLetters(5)/Emp

    }
}
