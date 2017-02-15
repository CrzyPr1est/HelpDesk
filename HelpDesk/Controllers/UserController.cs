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

        // Метод на отображение информации о ролях и подразделениях
        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public ActionResult Create()
        {
            SelectList departments = new SelectList(_db.Departments, "Id", "Name");
            ViewBag.Departments = departments;
            SelectList roles = new SelectList(_db.Roles, "Id", "Name");
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            SelectList departments = new SelectList(_db.Departments, "Id", "Name");
            ViewBag.Departments = departments;
            SelectList roles = new SelectList(_db.Roles, "Id", "Name");
            ViewBag.Roles = roles;

            return View(user);
        }
    }
}