using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HelpDesk.Models.DAL;
using HelpDesk.Models;

namespace HelpDesk.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login (LogViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Request");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильные данные входа!");
                }
               
            }
            return View(model);
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        private bool ValidateUser(string login, string password)
        {
            bool IsValid = false;

            using (HelpdeskContext _db = new HelpdeskContext())
            {
                try
                {
                    User user = (from u in _db.Users where u.Login==login && u.Password==password select u).FirstOrDefault();
                    if (user != null)
                    {
                        IsValid = true;
                    }
                    else
                    {
                        IsValid = false;
                    }
                }
                catch
                {
                    IsValid = false;
                }                
            }
            return IsValid;
        }
    }
}