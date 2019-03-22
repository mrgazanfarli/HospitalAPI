using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class PhoneView
    {
        public int Id { get; set; }

        [Column(TypeName = "ntext")]
        public string Photo { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required, MaxLength(125)]
        public string Text { get; set; }
    }
}