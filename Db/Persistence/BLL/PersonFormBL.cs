using Interface.Core;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Domain.Notifications;
using System.Web.Mvc;
using Model.Domain;

namespace Db.Persistence.BLL
{
   public class PersonFormBL
    {
        IHrUnitOfWork hrUnitOfWork;
        string Lang;
        int CompanyId;
        public PersonFormBL(IHrUnitOfWork _hrUnitOfWork, string _lang, int _companyid)
        {
            hrUnitOfWork = _hrUnitOfWork;
            Lang = _lang;
            CompanyId = _companyid;
        }

        #region Save
        public ModelStateDictionary IsValid(ModelStateDictionary ModelState, bool ServerValidationEnabled)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PersonFormPage",
                        TableName = "PersonForms",
                        Columns = MsgUtils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
                        Culture = Lang
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
                        return ModelState;
                    }
                }
            }

            return ModelState;
        }
        

        #endregion
    }
}
