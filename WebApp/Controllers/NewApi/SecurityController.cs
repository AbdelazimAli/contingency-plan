using Interface.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApp.Controllers.Api;
using WebApp.CustomFilters;
using WebApp.Models;

namespace WebApp.Controllers.NewApi
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SecurityController : BaseApiController
    {
        UserContext db = new UserContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public SecurityController()
        {
        }
        protected IHrUnitOfWork hrUnitOfWork { get; private set; }

        public SecurityController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            hrUnitOfWork = unitOfWork;
        }
        public SecurityController(ApplicationSignInManager signInManager)
        {
            SignInManager = signInManager;
        }

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
        [HttpPost]
        [MyCustomAuthorize]
        [Route("newApi/Security/Login")]
        public async Task<IHttpActionResult> Login([FromBody]LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (result.Equals(SignInStatus.Success))
            {
                _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                var appUser = await _userManager.FindByNameAsync(model.UserName);
                
                var R = await _userManager.GetRolesAsync(appUser.Id.ToString());
                var user = db.Users.Where(u => u.UserName == model.UserName).Select(u => new
                {
                    u.Id,
                    u.Culture,
                    CompanyId = u.DefaultCompany,
                    u.Email,
                    u.EmpId,
                    u.Locked,
                    u.UserName,
                    User.Identity.AuthenticationType,
                    Name = User.Identity.Name,
                    u.ResetPassword,
                    IsAuthenticated = User.Identity.IsAuthenticated,
                    u.Language,
                    Roles = R,
                    Password = model.Password
                }).SingleOrDefault();

                if (user == null)
                {
                    return NotFound();
                }
                if (user.Locked)
                {
                    return Unauthorized();
                }
                return Ok(user);
            }
            return StatusCode(HttpStatusCode.Forbidden);
        }
        [HttpPost]
        [Route("newApi/Security/Reset")]
        public async Task<IHttpActionResult> Reset([FromBody]LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            var user = (from p in db.Users
                        where p.UserName == model.UserName
                        select p).SingleOrDefault();
            if (user == null)
            {
                return NotFound();
            }


            if (user.ResetPassword == true)
            {
                if (model.ResetPassword == model.conFirm)
                {
                    _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                    user.ResetPassword = false;
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(model.ResetPassword);
                    model.Password = model.ResetPassword;
                    int res = await db.SaveChangesAsync();
                    if (res > 0)
                    {

                        return Ok(res);
                    }
                    return StatusCode(HttpStatusCode.NotModified);
                }
                else
                {
                    return Conflict();
                }
            }
            return NotFound();
        }

        [HttpPost]
        [Route("newApi/Security/ResetLanguage")]
        public async Task<IHttpActionResult> ResetLanguage([FromBody]NewLangViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Equals(SignInStatus.Success))
            {
                _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                var appUser = db.Users.Where(u => u.UserName == model.UserName).SingleOrDefault();
                appUser.Language = model.Language;
                int res = await db.SaveChangesAsync();
                if (res > 0)
                {
                    
                    return Ok(res);
                }
                return StatusCode(HttpStatusCode.NotModified);

            }
            return StatusCode(HttpStatusCode.Forbidden);
        }


    }

    public class NewLangViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Language { get; set; }

    }
}
