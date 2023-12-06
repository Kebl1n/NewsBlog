using System.ComponentModel.DataAnnotations;

namespace NewsBlog.Viewmodels.FormsOfCategory
{
    public class CreateFormOfCategory
    {
        [Required(ErrorMessage = "Введите категорию")]
        [Display(Name = "Категория")]
        public required string FormOfCategory { get; set; }
    }
}
