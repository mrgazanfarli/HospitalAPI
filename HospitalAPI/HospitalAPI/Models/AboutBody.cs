using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class AboutBody
    {
        public int Id { get; set; }

        [Column(TypeName = "ntext")]
        public string Photo { get; set; }

        [Required, MaxLength(30)]
        public string Heading { get; set; }

        [Required, MaxLength(70)]
        public string Title { get; set; }

        [Required, MaxLength(300), MinLength(70)]
        [Column(TypeName = "ntext")]
        public string Text { get; set; }

        [Required, MaxLength(11)]
        public string ButtonText { get; set; }

        [Required, MaxLength(100)]
        public string ButtonURL { get; set; }
    }
}