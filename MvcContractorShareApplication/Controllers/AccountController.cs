﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MvcContractorShareApplication.Filters;
using MvcContractorShareApplication.Models;
using MvcContractorShareApplication.Enumerations;

namespace MvcContractorShareApplication.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //Initialize the WebService client
        ContractorShareServiceReference.ContractorShareClient ContractorShareService = new ContractorShareServiceReference.ContractorShareClient();
        ContractorShareServiceReference.LoginInfo logininfo = new ContractorShareServiceReference.LoginInfo();
        ContractorShareServiceReference.AuthenticationResult loginresult = new ContractorShareServiceReference.AuthenticationResult();
        ContractorShareServiceReference.AuthenticationResult registerresult = new ContractorShareServiceReference.AuthenticationResult();
        ContractorShareServiceReference.RegisterInfo registerinfo = new ContractorShareServiceReference.RegisterInfo();
        ContractorShareServiceReference.ResetPasswordInfo resetpasswordinfo = new ContractorShareServiceReference.ResetPasswordInfo();
        ContractorShareServiceReference.ResetPasswordResult resetpasswordresult = new ContractorShareServiceReference.ResetPasswordResult();
        ContractorShareServiceReference.ChangePasswordInfo changepasswordinfo = new ContractorShareServiceReference.ChangePasswordInfo();
        ContractorShareServiceReference.ChangePreferencesInfo changepreferencesinfo = new ContractorShareServiceReference.ChangePreferencesInfo();
        ContractorShareServiceReference.PreferencesResult preferencesresult = new ContractorShareServiceReference.PreferencesResult();

        UserProfile userprofile = new UserProfile();
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                logininfo.Email = model.UserName;
                logininfo.Password = model.Password;
                
                loginresult = ContractorShareService.Login(logininfo);
                               
                if (loginresult.UserId > 0)
                {
                        userprofile.UserId = loginresult.UserId;
                        userprofile.UserName = model.UserName;

                        Session["userId"] = loginresult.UserId;
                        Session["username"] = model.UserName;
                        Session["usertype"] = loginresult.UserType;

                        returnUrl = Url.Action("Index", "Home");
                        return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", loginresult.error);
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.RemoveAll();

            return RedirectToAction("Index", "Home");
        }

        //ResetPassword
        //GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        //
        // POST: /Account/ResetPassword

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                resetpasswordinfo.Email = model.UserName;

                resetpasswordresult = ContractorShareService.ResetPassword(resetpasswordinfo);

                if (resetpasswordresult.Result.Equals("OK"))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", resetpasswordresult.Result);
                    return View(model);
                }
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The email provided is incorrect.");
            return View(model);
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    registerinfo = new ContractorShareServiceReference.RegisterInfo();
                    registerinfo.Email = model.UserName;
                    registerinfo.Password = model.Password;
                    registerinfo.UserType = 1;

                    registerresult = ContractorShareService.Register(registerinfo);

                    logininfo = new ContractorShareServiceReference.LoginInfo();
                    logininfo.Email = model.UserName;
                    logininfo.Password = model.Password;

                    if (registerresult.error.Equals("OK"))
                    {
                        loginresult = ContractorShareService.Login(logininfo);

                        if (loginresult.UserId > 0)
                        {
                                userprofile.UserId = loginresult.UserId;
                                userprofile.UserName = model.UserName;

                                Session["userId"] = loginresult.UserId;
                                Session["username"] = model.UserName;
                                Session["usertype"] = loginresult.UserType;

                                return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", loginresult.error);
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", registerresult.error);
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/RegisterContractor

        [AllowAnonymous]
        public ActionResult RegisterContractor()
        {
            return View();
        }

        //
        // POST: /Account/RegisterContractor

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterContractor(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    registerinfo = new ContractorShareServiceReference.RegisterInfo();
                    registerinfo.Email = model.UserName;
                    registerinfo.Password = model.Password;
                    registerinfo.UserType = 2;

                    logininfo = new ContractorShareServiceReference.LoginInfo();
                    logininfo.Email = model.UserName;
                    logininfo.Password = model.Password;

                    registerresult = ContractorShareService.Register(registerinfo);

                    if (registerresult.error.Equals("OK"))
                    {
                        loginresult = ContractorShareService.Login(logininfo);

                        if (loginresult.UserId > 0)
                        {
                                userprofile.UserId = loginresult.UserId;
                                userprofile.UserName = model.UserName;
                                Session["userId"] = loginresult.UserId;
                                Session["username"] = model.UserName;
                                Session["usertype"] = loginresult.UserType;

                                return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", loginresult.error);
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", registerresult.error);
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");

            return View();
        }

        [ChildActionOnly]
        public ActionResult ChangeUserPreferences()
        {
            UserPreferences userpreferencesmodel = new UserPreferences();

            preferencesresult = ContractorShareService.GetPreferences(Session["userId"].ToString());
            if(preferencesresult != null)
            {
                userpreferencesmodel.enableNotifications = (bool)preferencesresult.enableNotifications;
                userpreferencesmodel.shareLocation = (bool)preferencesresult.shareLocation;
                userpreferencesmodel.showContactNumber = (bool)preferencesresult.showContactNumber;
                userpreferencesmodel.showContactEmail = (bool)preferencesresult.showContactEmail;
            }
            userpreferencesmodel.preferencestab = true;

            return PartialView("_UserPreferencesPartial", userpreferencesmodel);
        }

        [ChildActionOnly]
        public ActionResult MyProfile()
        {
            var myprofile = ContractorShareService.GetUserProfile(Session["userId"].ToString());

            ViewClientModel model = new ViewClientModel();

            model.userId = (int)Session["userId"];
            model.Name = string.Concat(myprofile.Firstname, " ", myprofile.Surname);
            model.Address = myprofile.Address;
            model.City = string.Concat(myprofile.City, ", ", myprofile.Country);
            model.Email = myprofile.Email;
            model.PostalCode = myprofile.PostalCode;
            model.Country = myprofile.Country;
            model.PhoneNumber = myprofile.PhoneNumber.ToString();
            model.MobileNumber = myprofile.MobileNumber.ToString();

            return PartialView("MyClientProfile", model);
        }

        [ChildActionOnly]
        public ActionResult Help()
        {
            Suggestion helpmodel = new Suggestion();

            helpmodel.helptab = true;
            return PartialView("_Help", helpmodel);
        }

        [ChildActionOnly]
        public ActionResult Issue()
        {
            Bug issuemodel = new Bug();

            issuemodel.issuetab = true;
            return PartialView("_NotifyIssue", issuemodel);
        }

        [ChildActionOnly]
        public ActionResult ListBugs()
        {
            IEnumerable<MvcContractorShareApplication.Models.Bug> bugs = new List<Bug>();

            var bugsinfolist = ContractorShareService.ListBugs(Session["userId"].ToString());

            List<Bug> bugslist = new List<Bug>();

            foreach (var b in bugsinfolist)
            {
                Bug bug = new Bug();
                bug.Created = b.Created;
                bug.comment = b.Message;

                bugslist.Add(bug);
            }

            bugs = (IEnumerable<Bug>)bugslist;
            return PartialView("_ListBugsPartial", bugs);
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model, UserPreferences userpreferencesmodel, Suggestion helpmodel, Bug issuemodel)
        {
            //if user has submitted the user preferences changes
            if (userpreferencesmodel.preferencestab)
            {
                changepreferencesinfo.shareLocation = (bool)userpreferencesmodel.shareLocation;
                changepreferencesinfo.showContactEmail = (bool)userpreferencesmodel.showContactEmail;
                changepreferencesinfo.showContactNumber = (bool)userpreferencesmodel.showContactNumber;
                changepreferencesinfo.enableNotifications = (bool)userpreferencesmodel.enableNotifications;

                string result = ContractorShareService.ChangePreferences(Session["userId"].ToString(), changepreferencesinfo);
                if (result == "OK")
                {
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", result);
                    return View(model);
                }
            }
            else if (helpmodel.helptab)
            {
                string comment = helpmodel.comment;
                var result = ContractorShareService.AddSuggestion(Session["userId"].ToString(),comment);
                if (result.message == "OK")
                {
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", result.message);
                    return View(model);
                }
            }
            else if (issuemodel.issuetab)
            {
                string comment = issuemodel.comment;
                var result = ContractorShareService.AddBug(Session["userId"].ToString(), comment);
                if (result.message == "OK")
                {
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", result.message);
                    return View(model);
                }
            }
            //else: user has submitted change password
            else
            {
                bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                ViewBag.HasLocalPassword = hasLocalAccount;
                ViewBag.ReturnUrl = Url.Action("Manage");

                if (hasLocalAccount)
                {
                    if (ModelState.IsValid)
                    {
                        // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                        bool changePasswordSucceeded;
                        try
                        {
                            changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                        }
                        catch (Exception)
                        {
                            changePasswordSucceeded = false;
                        }

                        if (changePasswordSucceeded)
                        {
                            //call webservice change password
                            changepasswordinfo.email = User.Identity.Name;
                            changepasswordinfo.OldPassword = model.OldPassword;
                            changepasswordinfo.NewPassword = model.NewPassword;

                            string result = ContractorShareService.ChangePassword(Session["userId"].ToString(), changepasswordinfo);

                            if (result == "OK")
                            {
                                return RedirectToAction("Manage");
                            }
                            else
                            {
                                ModelState.AddModelError("", result);

                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                        }
                    }
                }
                else
                {
                    // User does not have a local password so remove any validation errors caused by a missing
                    // OldPassword field
                    ModelState state = ModelState["OldPassword"];
                    if (state != null)
                    {
                        state.Errors.Clear();
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
        }
         

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
