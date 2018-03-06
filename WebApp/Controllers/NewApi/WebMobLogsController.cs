using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Interface.Core;
using System.Web.Http.Cors;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Controllers.NewApi
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Model.Domain.Notifications;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<WebMobLog>("WebMobLogs");
    builder.EntitySet<Notification>("Notification"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class NotificationVM
    {
        [Required]
        public string UserName { get; set; }
        public string Language { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public int EmpId { get; set; }

    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WebMobLogsController : BaseODataController
    {
        protected IHrUnitOfWork _hrUnitOfWork { get; private set; }
        public WebMobLogsController(IHrUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        // GET: odata/WebMobLogs
        [EnableQuery]
        [AllowAnonymous]
        public IHttpActionResult Post([FromBody]NotificationVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (model == null)
            {
                return NotFound();
            }
            var Notify = _hrUnitOfWork.NotificationRepository.GetAllNotifications(model.UserName, model.Language, model.CompanyId).AsQueryable();

            return Ok(Notify);
        }






    }
}
