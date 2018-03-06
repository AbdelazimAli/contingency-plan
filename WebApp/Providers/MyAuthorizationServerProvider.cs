using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using WebApp.Models;

namespace WebApp.Providers
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        UserContext db = new UserContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var result = await SignInManager.PasswordSignInAsync(context.UserName, context.Password, false, false);
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            var appUser = await _userManager.FindByNameAsync(context.UserName);
            var identity = await _userManager.CreateIdentityAsync(appUser, context.Options.AuthenticationType);
            if (result.Equals(SignInStatus.Success))
            {
                identity.AddClaim(new Claim("DefaultCompany", appUser.DefaultCompany.ToString()));
                context.Validated(identity);
            }
            else
            {
                context.SetError("Invalid Grant", "InvalidGrant");
                return;
            }
        }
    }
}