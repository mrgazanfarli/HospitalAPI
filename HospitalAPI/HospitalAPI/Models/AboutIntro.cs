using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class AboutIntro
    {
        public int Id { get; set; }

        [Required, StringLength(40)]
        public string Title { get; set; }

        [Required]
        [MaxLength(525), MinLength(100)]
        [Column(TypeName = "ntext")]
        public string Desc { get; set; }

        [Required, StringLength(10)]
        public string ButtonText { get; set; }
    }
}