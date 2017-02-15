using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HelpDesk.Models.DAL;
using HelpDesk.Models;

namespace HelpDesk.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class ServiceController : Controller
    {
        HelpdeskContext _db = new HelpdeskContext();
        // Вывод информации об отделах
        [HttpGet]
        public ActionResult Departments()
        {
            ViewBag.Departments = _db.Departments;
            return View();
        }

        // Метод для добавления отдела
        [HttpPost]
        public ActionResult Departments(Department dept)
        {
            if (ModelState.IsValid)
            {
                _db.Departments.Add(dept);
                _db.SaveChanges();
            }
            ViewBag.Departments = _db.Departments;
            return View(dept);
        }
        // Метод для удаления отдела
        public ActionResult DeleteDepartment(int id)
        {
            Department dept = _db.Departments.Find(id);
            _db.Departments.Remove(dept);
            _db.SaveChanges();
            return RedirectToAction("Departments");
        }
        /************^редактирование отделов^******************/

        // Вывод информации об активах
        [HttpGet]
        public ActionResult Activ()
        {
            ViewBag.Activs = _db.Activs.Include(s => s.Department);
            ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name");
            return View();
        }

        // Метод для добавления актива
        [HttpPost]
        public ActionResult Activ(Activ activ)
        {
            if (ModelState.IsValid)
            {
                _db.Activs.Add(activ);
                _db.SaveChanges();
            }
            ViewBag.Activs = _db.Activs.Include(s => s.Department);
            ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name");
            return View(activ);
        }

        // Метод для удаления актива
        public ActionResult DeleteActiv(int id)
        {
            Activ activ = _db.Activs.Find(id);
            _db.Activs.Remove(activ);
            _db.SaveChanges();
            return RedirectToAction("Activ");
        }
        /***********^редактирование активов^***********/

        // Вывод информации о категориях заявок
        [HttpGet]
        public ActionResult Categories ()
        {
            ViewBag.Categories = _db.Categories;
            return View();
        }

        // Метод для добавления новой категории заявок
        [HttpPost]
        public ActionResult Categories (Category cat)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(cat);
                _db.SaveChanges();
            }
            ViewBag.Categories = _db.Categories;
            return View(cat);
        }

        // Метод для удаления категории заявок
        public ActionResult DeleteCategory (int id)
        {
            Category cat = _db.Categories.Find(id);
            _db.Categories.Remove(cat);
            _db.SaveChanges();
            return RedirectToAction("Categories");
        }
        /***********^редактирование категорий заявок^************/
    }
}