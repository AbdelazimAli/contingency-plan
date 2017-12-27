using Interface.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Http.OData;

namespace WebApp.Controllers.NewApi
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public abstract class BaseODataController: ODataController
    {
        protected IHrUnitOfWork HrUnitOfWork { get; private set; }
        protected bool ServerValidationEnabled = false;
        public BaseODataController()
        {

        }
        protected BaseODataController(IHrUnitOfWork unitOfWork)
        {
            HrUnitOfWork = unitOfWork;
            ServerValidationEnabled = System.Configuration.ConfigurationManager.AppSettings["ServerValidationEnabled"] == "true";

        }
    }
}