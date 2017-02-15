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

        // Метод для отображения списка пользователей
        [HttpGet]
        public ActionResult Index()
        {
            var users = _db.Users.Include(u => u.Department).Include(u => u.Role).ToList();

            // Добавляем возможность выбора всех отделов
            List<Department> departments = _db.Departments.ToList();
            departments.Insert(0, new Department { Id = 0, Name = "Все" });
            ViewBag.Departments = new SelectList(departments, "Id", "Name");

            // Добавляем возможность выбора всех ролей
            List<Role> roles = _db.Roles.ToList();
            roles.Insert(0, new Role { Id = 0, Name = "Все" });
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(users);
        }

        // Метод для сортировке по отделу и роли
        [HttpPost]
        public ActionResult Index(int department, int role)
        {
            IEnumerable<User> allUsers = null;
            if (role == 0 && department == 0)
            {
                return RedirectToAction("Index");
            }
            else if (role !=0 && department == 0)
            {
                allUsers = from user in _db.Users.Include(u => u.Department).Include(u => u.Role) where user.RoleId == role select user;
            }
            else if (role == 0 && department != 0)
            {
                allUsers = from user in _db.Users.Include(u => u.Department).Include(u => u.Role) where user.DepartmentId == department select user;
            }
            else
            {
                allUsers = from user in _db.Users.Include(u => u.Department).Include(u => u.Role) where user.DepartmentId == department && user.RoleId == role select user;
            }
            List<Department> departments = _db.Departments.ToList();
            //Добавляем в список возможность выбора всех
            departments.Insert(0, new Department { Name = "Все", Id = 0 });
            ViewBag.Departments = new SelectList(departments, "Id", "Name");

            List<Role> roles = _db.Roles.ToList();
            roles.Insert(0, new Role { Name = "Все", Id = 0 });
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(allUsers.ToList());
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