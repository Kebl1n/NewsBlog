using NewsBlog.Models.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NewsBlog.Viewmodels.BlogsOfForm
{
    public class EditFormOfBlog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }


        [Required(ErrorMessage = "Введите название новости")]
        [Display(Name = "Заголовок новости")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Введите текст")]
        [Display(Name = "Текст новости")]
        public required string Text { get; set; }

        [Display(Name = "Дата публикации")]
        public DateTime Date { get; set; }

        public void CreateField()
        {
            Date = DateTime.Now;
        }
        //навигационные св-ва
        [Required]
        [Display(Name = "Категория")]
        public short IdCategory { get; set; }

        [Display(Name = "Категория")]
        [ForeignKey("IdCategory")]
        public Category Category { get; set; }


        //[Required]
        //        public string IdUser { get; set; }

        //      [ForeignKey("IdUser")]
        //    public User User { get; set; }


    }
}
