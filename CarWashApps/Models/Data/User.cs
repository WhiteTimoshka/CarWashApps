using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarWashApps.Models.Data
{
    public class User : IdentityUser
    {
        //Дополнительные поля для каждого пользователя
        public int Id { get; set; }
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Введите Имя")]
        public string NameUser { get; set; }
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Введите Фамилию")]
        public string Surname { get; set; }
        public DateTime RegDate { get; set; }

        //Навигационные св-ва
    }
}
