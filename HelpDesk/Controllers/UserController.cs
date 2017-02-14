using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HelpDesk.Models;
using HelpDesk.Models.DAL;

namespace HelpDesk.Controllers
{
    [Authorize(Roles = "Администратор, Модератор, Исполнитель")]
    public class UserController : Controller
    {
        private HelpdeskContext _db = new HelpdeskContext();

        [HttpGet]
        public ActionResult Index()
        {
            var users = _db.Users.Include(u => u.Department).Include(u => u.Role).ToList();
            return View(users);
        }
    }
}