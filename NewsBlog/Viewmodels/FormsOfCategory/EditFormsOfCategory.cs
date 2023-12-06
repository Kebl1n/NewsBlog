using System.ComponentModel.DataAnnotations;

namespace NewsBlog.Viewmodels.FormsOfCategory
{
    public class EditFormsOfCategory
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите категорию")]
        [Display(Name = "Категория")]
        public required string FormOfCategory { get; set; }
    }
}