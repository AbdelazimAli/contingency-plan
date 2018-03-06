using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Db.Persistence.BLL
{
    public class UserBL
    {
        public UserBL()
        {

        }

        public string TranslateError(string error, string lang)
        {
            string msg;
            switch (error)
            {
                case "Passwords must have at least one non letter or digit character. Passwords must have at least one lowercase ('a'-'z'). Passwords must have at least one uppercase ('A'-'Z').":
                    msg = MsgUtils.Instance.Trls(lang, "passWordComplex");
                    break;
                case "The field PhoneNumber must match the regular expression '[0-9()\\-+]*'.":
                    msg = MsgUtils.Instance.Trls(lang, "WrongPhoneNumber");
                    break;
                default:
                    msg = MsgUtils.Instance.Trls(lang, error);
                    break;
            }
            return msg;
        }
       
        public void CheckPasswordStrength(ModelStateDictionary ModelState, string lang)
        {
            var dict = new Dictionary<string, string>();

            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    var error = item.Value.Errors.SingleOrDefault();
                    if (error != null)
                    {
                        item.Value.Errors.Remove(error);
                        dict.Add(item.Key, error.ErrorMessage);
                    }
                }

                foreach (var str in dict)
                    ModelState.AddModelError(str.Key, TranslateError(str.Value, lang));
            }
        }

        public bool DuplicatePassword(ModelStateDictionary ModelState, string newpass, string oldpass, string lang)
        {
            if (newpass == oldpass)
            {
                ModelState.AddModelError("ConfirmPassword", MsgUtils.Instance.Trls("dublicatePassword", lang));
                return false;
            }

            return true;
        }
    }
}
