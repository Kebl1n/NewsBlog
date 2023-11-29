using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsBlog.Models.Data
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }


        [Required(ErrorMessage = "Введите категорию")]
        [Display(Name ="Категория")]
        public required string FormOfCategory {  get; set; }

    }
}
