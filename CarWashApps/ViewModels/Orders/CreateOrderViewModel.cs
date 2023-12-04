﻿using System.ComponentModel.DataAnnotations;

namespace CarWashApps.ViewModels.Orders
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Выберите услугу")]
        [Display(Name = "Услуга")]
        public int IdService { get; set; }

        [Required(ErrorMessage = "Выберите время")]
        [Display(Name = "Время")]
        [DataType(DataType.DateTime, ErrorMessage = "Некорректная дата")]
        public DateTime? OrderDateTime { get; set; }

        public string IdUser { get; set; }
    }
}
