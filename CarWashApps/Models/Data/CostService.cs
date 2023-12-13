using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWashApps.Models.Data
{
    public class CostService
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        [Required]
        [Display(Name = "Услуга")]
        public int IdService { get; set; }

        [Required(ErrorMessage = "Введите стоимость услуги")]
        [Display(Name = "Стоимость")]
        public decimal Cost { get; set; }

        [Required]
        [Display(Name = "Дата установки")]
        public DateTime DateCost { get; set; }

        // Навигационные свойства        
        [ForeignKey("IdService")]
        [Display(Name = "Услуга")]
        public ListService ListService { get; set; }

    }
}