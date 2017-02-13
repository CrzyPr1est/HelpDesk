using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models
{
    public class Request
    {
        public enum RequestStatus
        {
            Open = 1,
            Distributed = 2,
            Proccesing = 3,
            Checking = 4,
            Closed = 5
        }
        // Перечисление для приоритета заявки
        public enum RequestPriority
        {
            Low = 1,
            Medium = 2,
            High = 3,
            Critical = 4
        }
        // ID 
        public int Id { get; set; }
        // Наименование заявки
        [Required]
        [Display(Name = "Название заявки")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }
        // Описание
        [Required]
        [Display(Name = "Описание")]
        [MaxLength(200, ErrorMessage = "Превышена максимальная длина записи")]
        public string Description { get; set; }
        // Комментарий к заявке
        [Display(Name = "Комментарий")]
        [MaxLength(200, ErrorMessage = "Превышена максимальная длина записи")]
        public string Comment { get; set; }
        // Статус заявки
        [Display(Name = "Статус")]
        public int Status { get; set; }
        // Приоритет заявки
        [Display(Name = "Приоритет")]
        public int Priority { get; set; }
        // Номер кабинета
        [Display(Name = "Кабинет")]
        public int? ActivId { get; set; }
        public Activ Activ { get; set; }
        // Файл ошибки
        [Display(Name = "Файл с ошибкой")]
        public string File { get; set; }

        // Внешний ключ Категория
        [Display(Name = "Категория")]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        // Внешний ключ
        // ID Пользователя - обычное свойство
        public int? UserId { get; set; }
        // Отдел пользователя - Навигационное свойство
        public User User { get; set; }

        // Внешний ключ
        // ID Пользователя - обычное свойство
        public int? ExecutorId { get; set; }
        // Отдел пользователя - Навигационное свойство
        public User Executor { get; set; }

        // Внешний ключ
        // ID жизненного цикла заявки - обычное свойство
        public int LifecycleId { get; set; }
        // Ссылка на жизненный цикл заявки - Навигационное свойство
        public Lifecycle Lifecycle { get; set; }
    }
}
