using Interface.Core;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class AutoMapperParm
    {
       // public IHrUnitOfWork HrUnitOfWork { get; set; }
        public object Source { get; set; }
        public object Destination { get; set; }
        public string ObjectName { get; set; }
        public string Id { get; set; } = "Id";
        public Model.Domain.TransType Transtype { get; set; } = Model.Domain.TransType.Update;

        //public string UserName { get; set; }
        //public int CompanyId { get; set; }
        /// <summary>
        /// we use version in audit trail to know the page version
        /// </summary>
        public byte Version { get; set; } = 0;
        public OptionsViewModel Options { get; set; } = null;
    }
}