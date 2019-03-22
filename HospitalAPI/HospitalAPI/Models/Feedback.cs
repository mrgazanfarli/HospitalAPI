using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string Fullname { get; set; }

        [Column(TypeName = "ntext")]
        public string ProfilePhoto { get; set; }

        [Required, MaxLength(250)]
        public string Content { get; set; }
    }
}