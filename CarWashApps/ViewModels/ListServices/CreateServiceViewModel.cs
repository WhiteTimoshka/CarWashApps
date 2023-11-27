using System.ComponentModel.DataAnnotations;

namespace CarWashApps.ViewModels.ListServices
{
    public class CreateServiceViewModel
    {
        [Required(ErrorMessage = "Введите услугу")]
        [Display(Name = "Услуга")]
        public string ServiceName { get; set; }

        [Display(Name = "Описание услуги")]
        public string? ServiceDescription { get; set; }
    }
}