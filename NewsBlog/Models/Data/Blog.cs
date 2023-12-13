using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsBlog.Models.Data
{
    public class Blog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "��")]
        public short Id { get; set; }


        [Required(ErrorMessage = "������� �������� �������")]
        [Display(Name = "��������� �������")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "������� �����")]
        [Display(Name = "����� �������")]
        public required string Text { get; set; }

        [Display(Name = "���� ����������")]
        public DateTime Date { get; set; }

        public void CreateField()
        {
            Date = DateTime.Now;
        }
        //������������� ��-��
        [Required]
        [Display(Name = "���������")]
        public short IdCategory { get; set; }

        [Display(Name = "���������")]
        [ForeignKey("IdCategory")]
        public Category Category { get; set; }


        //[Required]
        //        public string IdUser { get; set; }

        //      [ForeignKey("IdUser")]
        //    public User User { get; set; }


    }
}
