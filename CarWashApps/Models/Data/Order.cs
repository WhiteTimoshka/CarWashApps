using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWashApps.Models.Data
{
    public class Order
    {
        // Key - первичный ключ
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Выберите услугу")]
        [Display(Name = "Услуга")]
        public int IdService { get; set; }

        [Required(ErrorMessage = "Выберите время")]
        [Display(Name = "Время")]
        [DataType(DataType.DateTime, ErrorMessage = "Некорректная дата")]
        public DateTime? OrderDateTime { get; set; }

        [Required]
        public string IdUser { get; set; }


        // Навигация
        [ForeignKey("IdUser")]
        public User User { get; set; }

        [ForeignKey("IdService")]
        [Display(Name = "Услуга")]
        public ListService ListService { get; set; }
    }
}
