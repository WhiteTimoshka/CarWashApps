using System.ComponentModel.DataAnnotations;

namespace CarWashApps.ViewModels.CostServices
{
    public class EditCostServiceViewModel
    {
        public short Id { get; set; }

        [Required]
        [Display(Name = "Услуга")]
        public int IdService { get; set; } //будем передавать ИД формы обучения

        [Required(ErrorMessage = "Введите стоимость услуги")]
        [Display(Name = "Стоимость")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Введите дату установки")]
        [Display(Name = "Дата установки")]
        public DateTime DateCost { get; set; }
    }
}