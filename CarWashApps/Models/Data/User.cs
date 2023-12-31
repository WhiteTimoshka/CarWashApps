﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarWashApps.Models.Data
{
    public class User : IdentityUser
    {
        //Дополнительные поля для каждого пользователя        
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Введите Имя")]
        public string NameUser { get; set; }
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Введите Фамилию")]
        public string Surname { get; set; }
        [Display(Name = "Дата регистрации")]
        [Required]
        public DateTime RegDate { get; set; }

        
        //Навигационные св-ва
        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
