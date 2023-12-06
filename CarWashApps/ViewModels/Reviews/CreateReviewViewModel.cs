using System.ComponentModel.DataAnnotations;
using static CarWashApps.Reviews.ReviewsController;
namespace CarWashApps.ViewModels.Reviews
{
    public class CreateReviewViewModel
    {
        [Required]
        [Display(Name = "Оценка")]
        public byte Rating { get; set; }

        [Display(Name = "Текст отзыва")]
        public string? ReviewText { get; set; }

        [Required]
        [Display(Name = "Дата отзыва")]
        public DateTime ReviewDateTime { get; set; }

        public string IdUser { get; set; }
    }
}
