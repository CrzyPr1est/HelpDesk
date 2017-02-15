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

        // Тот-же метод для создания
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

        // Метод для отображения информации для редактирования
        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public ActionResult Edit(int id)
        {
            User user = _db.Users.Find(id);
            SelectList departments = new SelectList(_db.Departments, "Id", "Name", user.DepartmentId);
            ViewBag.Departments = departments;
            SelectList roles = new SelectList(_db.Roles, "Id", "Name", user.RoleId);
            ViewBag.Roles = roles;

            return View(user);
        }

        // Тот-же метод для редактирования
        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            SelectList departments = new SelectList(_db.Departments, "Id", "Name");
            ViewBag.Departments = departments;
            SelectList roles = new SelectList(_db.Roles, "Id", "Name");
            ViewBag.Roles = roles;

            return View(user);
        }
        [Authorize(Roles = "Администратор")]
        public ActionResult Delete(int id)
        {
            User user = _db.Users.Find(id);
            _db.Users.Remove(user);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}