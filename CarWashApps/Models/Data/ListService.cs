using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWashApps.Models.Data
{
    public class ListService
    {
        // Key - первичный ключ
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название услуги")]
        [Display(Name = "Услуга")]
        public string ServiceName { get; set; }

        [Display(Name = "Описание услуги")]
        public string? ServiceDescription { get; set; }

        // Навигация
        public ICollection<CostService> CostServices { get; set; }
    }
}
