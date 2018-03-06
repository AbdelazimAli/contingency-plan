using Interface.Core;
using Model.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApp.Controllers.NewApi
{
    public class UpdateNotifyModel
    {
        public int Id { get; set; }
        public string Culture { get; set; }
        public int CompanyId { get; set; }
        public string UserName { get; set; }

    }
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NotificationsController : BaseApiController
    {
        public NotificationsController()
        {
        }
        protected IHrUnitOfWork hrUnitOfWork { get; private set; }

        public NotificationsController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            hrUnitOfWork = unitOfWork;
        }



        [HttpPost]
        [Route("newApi/Notifications/Update")]
        public IHttpActionResult UpdateNotification([FromBody] UpdateNotifyModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (model == null)
            {
                return NotFound();
            }
            var Notify = hrUnitOfWork.NotificationRepository.GetNotify(model.Id, model.Culture, model.CompanyId, model.UserName);
            return Ok(Notify);
        }

        [HttpPost]
        [Route("newApi/Notifications/Count")]
        public IHttpActionResult NotificationCount([FromBody]NotificationVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (model == null)
            {
                return NotFound();
            }

            var LetterCount = hrUnitOfWork.NotificationRepository.GetMyLetters(model.CompanyId, model.Language, model.EmpId).Count(l => l.read == false);
            var NotifyCount = hrUnitOfWork.NotificationRepository.GetAllNotifications(model.UserName, model.Language, model.CompanyId).Count(n => n.Read == false);

            return Ok(new { NotifyCount = NotifyCount, LetterCount = LetterCount });
        }

        [HttpPost]
        [Route("newApi/Notifications/UpdateNotifyLetter/{id}")]
        public IHttpActionResult UpdateNotifyLetter([FromUri] int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            
            var NotifyObject = hrUnitOfWork.Repository<NotifyLetter>().Where(s => s.Id == id).FirstOrDefault();
            if (NotifyObject == null)
            {
                return NotFound();
            }
            
            NotifyObject.read = true;
            NotifyObject.ReadTime = DateTime.Now;
            hrUnitOfWork.NotificationRepository.Attach(NotifyObject);
            hrUnitOfWork.NotificationRepository.Entry(NotifyObject).State = EntityState.Modified;
            //Create Notify Letters To Attendee to Cancel 
            var Errors = SaveChanges("en-GB");
            if (Errors.Count>0)
            {
                return StatusCode(HttpStatusCode.NotModified); //304
            }

            return Ok(NotifyObject);
        }
    }
}
