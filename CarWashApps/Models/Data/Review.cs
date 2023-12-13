using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWashApps.Models.Data
{
    public class Review
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        [Required]
        [Display(Name = "Оценка")]
        public byte Rating { get; set; }

        [Display(Name = "Текст отзыва")]
        public string? ReviewText { get; set; }

        [Required]
        [Display(Name = "Дата отзыва")]
        public DateTime ReviewDateTime { get; set; }

        [Required]
        [Display(Name = "Имя пользователя")]
        public string IdUser { get; set; }

        // Навигационные свойства        
        [ForeignKey("IdUser")]
        [Display(Name = "Имя пользователя")]
        public User User { get; set; }

    }
}