using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Abp.Configuration.Startup;
using Abp.Runtime.Security;
using CMS.Application.User;
using CMS.Application.User.Dto;
using CMS.Web.Models.User;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CMS.Web.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserAppService _userAppService;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        public UsersController(IUserAppService userAppService, IMultiTenancyConfig multiTenancyConfig)
        {
            _userAppService = userAppService;
            _multiTenancyConfig = multiTenancyConfig;
        }
        [HttpGet]
        public ActionResult Login(string userNameOrEmailAddress = "", string returnUrl = "", string successMessage = "")
        {
            //if (string.IsNullOrWhiteSpace(returnUrl))
            //{
            //    returnUrl = Url.Action("Index", "Application");
            //}

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;

            return View(
                new LoginFormViewModel
                {
                    TenancyName = "Default",//_tenancyNameFinder.GetCurrentTenancyNameOrNull(),
                    IsSelfRegistrationEnabled = true,//IsSelfRegistrationEnabled(),
                    SuccessMessage = successMessage,
                    UserNameOrEmailAddress = userNameOrEmailAddress
                });
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginModel, string returnUrl = "")
        {
            var loginResult = await _userAppService.Login(loginModel.UsernameOrEmailAddress, loginModel.Password, loginModel.TenancyName);
            SignIn(loginResult.User, loginResult.Identity, loginModel.RememberMe);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Url.Action("Index", "Dashboard");
            }

            return Redirect(returnUrl);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        } 

        private void SignIn(UserEditDto user, ClaimsIdentity identity = null, bool rememberMe = false)
        {
            if (identity == null)
            {
                identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                identity.AddClaim(new Claim(AbpClaimTypes.UserNameClaimType, user.UserName));
                identity.AddClaim(new Claim(AbpClaimTypes.UserIdClaimType, user.Id.ToString()));
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public async Task<ActionResult> Index()
        {
            var list = await _userAppService.GetUsers(new GetUsersInput());

            return View(list.Items);
        }

        public async Task<ActionResult> Create()
        {
            return View();
        }
    }
}
