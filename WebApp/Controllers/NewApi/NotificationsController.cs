using Interface.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApp.Controllers.Api;

namespace WebApp.Controllers.NewApi
{
    public class UpdateNotifyModel
    {
        public int Id { get; set; }
        public string Culture { get; set; }
        public int CompanyId { get; set; }
        public string UserName { get; set; }

    }
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
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
            var NotifyCount = hrUnitOfWork.NotificationRepository.GetAllNotifications(model.UserName, model.Language, model.CompanyId).Count(n => n.Read == false);

            return Ok(NotifyCount);
        }
    }
}
