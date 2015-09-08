using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Runtime.Security;
using Abp.UI;
using Abp.Web.Mvc.Authorization;
using Abp.Web.Mvc.Controllers;
using CMS.Application.Localization;
using CMS.Application.Role;
using CMS.Application.Role.Dto;
using CMS.Application.User;
using CMS.Application.User.Dto;
using CMS.Web.Models.User;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Abp.Extensions;
using System.Linq;

namespace CMS.Web.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IRoleAppService _roleAppService;
        private readonly IUnitOfWorkManager<Guid, Guid> _unitOfWorkManager;
        private readonly IUserAppService _userAppService;

        public UsersController(
            IUnitOfWorkManager<Guid, Guid> unitOfWorkManager,
            IUserAppService userAppService,
            IRoleAppService roleAppService,
            IMultiTenancyConfig multiTenancyConfig)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _userAppService = userAppService;
            _roleAppService = roleAppService;
            _multiTenancyConfig = multiTenancyConfig;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, CmsConsts.LocalizationSourceName);
        }

        [HttpGet]
        public ActionResult Login(string userNameOrEmailAddress = "", string returnUrl = "", string successMessage = "")
        {
            //if (string.IsNullOrWhiteSpace(returnUrl))
            //{
            //    returnUrl = Url.Action("Index", "Application");
            //}

            this.OutputModelMessage();

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;

            return View(
                new LoginFormViewModel
                {
                    TenancyName = "Default", //_tenancyNameFinder.GetCurrentTenancyNameOrNull(),
                    IsSelfRegistrationEnabled = true, //IsSelfRegistrationEnabled(),
                    SuccessMessage = successMessage,
                    UserNameOrEmailAddress = userNameOrEmailAddress
                });
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginModel, string returnUrl = "")
        {
            var loginResult =
                await
                    _userAppService.Login(loginModel.UsernameOrEmailAddress, loginModel.Password, loginModel.TenancyName);

            if (loginResult.Result == LoginResultType.Success)
            {
                SignIn(loginResult.User, loginResult.Identity, loginModel.RememberMe);
            }
            else
            {
                var name = "Login." + loginResult.Result;
                this.AddModelMessage(L(name).Localize());
            }

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

        public async Task<ActionResult> Index()
        {
            this.OutputModelMessage();

            var list = await _userAppService.GetUsers(new GetUsersInput());
            return View(list.Items);
        }

        [AbpMvcAuthorize("CMS.Admin.Users", "CMS.Create")]
        public async Task<ActionResult> Create()
        {
            var roles = await _roleAppService.GetRoles(new GetRolesInput { Sorting = "Order ASC" });
            ViewBag.Roles = roles.Items;

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AbpMvcAuthorize("CMS.Admin.Users", "CMS.Create")]
        public async Task<ActionResult> Create(CreateUserDto model, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var uow = _unitOfWorkManager.Begin())
                    {
                        model.IsActive = true;
                        var userId = await _userAppService.CreateUser(model);

                        var roles = await _roleAppService.GetRoles(new GetRolesInput());
                        var list = new List<UserRoleDto>();
                        foreach (var item in roles.Items)
                        {
                            var info = new UserRoleDto();
                            var chkName = "Role_" + item.Id;
                            var chkVal = collection[chkName];
                            if (chkVal == "on")
                            {
                                info.RoleId = item.Id;
                                info.UserId = userId;
                                info.Status = true;
                                list.Add(info);
                            }
                        }

                        await _userAppService.CreateOrUpdate(list);

                        await uow.CompleteAsync();
                    }

                    var lang = string.Format(L("Created.RecordSucceed").Localize(), model.UserName);

                    this.AddModelMessage("", lang, MessageTypes.Information);
                }
                catch (Exception ex)
                {
                    this.AddModelMessage("exception", ex.Message);
                }
            }

            return RedirectToAction("Index");
        }

        [AbpMvcAuthorize("CMS.Admin.Users", "CMS.Create", "CMS.Update")]
        public async Task<JsonResult> CheckUserName(string userName)
        {
            return await CheckExists(userName);
        }

        [AbpMvcAuthorize("CMS.Admin.Users", "CMS.Create", "CMS.Update")]
        public async Task<JsonResult> CheckEmail(string email, string initialEmail)
        {
            if (email == initialEmail)
                return Json(true, JsonRequestBehavior.AllowGet);

            return await CheckExists(email);
        }

        private async Task<JsonResult> CheckExists(string value)
        {
            var flag = true;
            var info = await _userAppService.GetUser(value);
            if (info != null)
            {
                flag = false;
            }

            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        [AbpMvcAuthorize("CMS.Admin.Users", "CMS.Update")]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (!id.HasValue)
                throw new UserFriendlyException("The paramenter id is null.");

            var roles = await _roleAppService.GetRoles(new GetRolesInput { Sorting = "Order ASC" });
            ViewBag.Roles = roles.Items;

            var info = await _userAppService.GetUser(new NullableIdInput<Guid> { Id = id });

            return View(info);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AbpMvcAuthorize("CMS.Admin.Users", "CMS.Update")]
        public async Task<ActionResult> Edit(UserEditDto model, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var uow = _unitOfWorkManager.Begin())
                    {
                        model.IsActive = true;
                        await _userAppService.UpdateUser(model);

                        var roles = await _roleAppService.GetRoles(new GetRolesInput());
                        var list = new List<UserRoleDto>();
                        foreach (var item in roles.Items)
                        {
                            var info = new UserRoleDto();
                            var chkName = "Role_" + item.Id;
                            var chkVal = collection[chkName];
                            var userRole = "UserRole_" + item.Id;

                            Guid userRoleId;
                            var status = Guid.TryParse(collection[userRole], out userRoleId);
                            if (status)
                                info.Id = userRoleId;

                            if (chkVal == "on")
                            {
                                info.RoleId = item.Id;
                                info.UserId = model.Id;
                                info.Status = true;
                                list.Add(info);
                            }
                            else
                            {
                                info.RoleId = item.Id;
                                info.UserId = model.Id;
                                info.Status = false;

                                if (status)
                                    list.Add(info);
                            }
                        }

                        await _userAppService.CreateOrUpdate(list);

                        await uow.CompleteAsync();
                    }

                    var lang = string.Format(L("Updated.RecordSucceed").Localize(), model.UserName);

                    this.AddModelMessage("", lang, MessageTypes.Information);
                }
                catch (Exception ex)
                {
                    this.AddModelMessage("exception", ex.Message);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AbpMvcAuthorize("CMS.Admin.Users", "CMS.Delete")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var list = id.Split(",").Select(Guid.Parse);
                using (var uow = _unitOfWorkManager.Begin())
                {
                    foreach (var item in list)
                    {
                        await _userAppService.DeleteUser(new IdInput<Guid> {Id = item});
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddModelMessage(ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}