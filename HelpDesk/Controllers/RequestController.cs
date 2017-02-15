using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelpDesk.Models.DAL;
using HelpDesk.Models;
using System.Data.Entity;

namespace HelpDesk.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        HelpdeskContext _db = new HelpdeskContext();
        public ActionResult Index()
        {
            User user = _db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            var requests = _db.Requests.Where(r => r.UserId == user.Id) //получаем заявки для текущего пользователя
                                    .Include(r => r.Category)  // добавляем категории
                                    .Include(r => r.Lifecycle)  // добавляем жизненный цикл заявок
                                    .Include(r => r.User)         // добавляем данные о пользователях
                                    .OrderByDescending(r => r.Lifecycle.Opened); // упорядочиваем по дате по убыванию   

            return View(requests.ToList());
        }

        // Вывод страницы по созданию заявки
        [HttpGet]
        public ActionResult Create()
        {
            // Запрос на текущего пользователя
            User user = _db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                // Запрос активов для департамента, в котором работает текущий пользователь
                var cabs = from cab in _db.Activs where cab.DepartmentId == user.DepartmentId select cab;
                ViewBag.Cabs = new SelectList(cabs, "Id", "CabNumber");
                ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");
                return View();
            }
            return RedirectToAction("LogOff", "Account");
        }

        // Метод для создания новой заявки
        [HttpPost]
        public ActionResult Create(Request request, HttpPostedFileBase error)
        {
            User user = _db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            
            if (user == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            if (ModelState.IsValid)
            {
                // Указываем статус Открыта у заявки
                request.Status = (int)Models.Request.RequestStatus.Open;
                // Получаем время открытия
                DateTime current = DateTime.Now;

                //Создаем запись о жизненном цикле заявки
                Lifecycle newLifecycle = new Lifecycle() { Opened = current };
                request.Lifecycle = newLifecycle;

                //Добавляем жизненный цикл заявки
                _db.Lifecycles.Add(newLifecycle);

                // указываем пользователя заявки
                request.UserId = user.Id;

                // если получен файл
                if (error != null)
                {
                    // Получаем расширение
                    string ext = error.FileName.Substring(error.FileName.LastIndexOf('.'));
                    // сохраняем файл по определенному пути на сервере
                    string path = current.ToString("dd/MM/yyyy H:mm:ss").Replace(":", "_").Replace("/", ".") + ext;
                    error.SaveAs(Server.MapPath("~/Files/" + path));
                    request.File = path;
                }
                //Добавляем заявку
                _db.Requests.Add(request);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(request);
        }
    }
}