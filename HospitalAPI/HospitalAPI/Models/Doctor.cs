using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Fullname { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required, MaxLength(35)]
        public string Address { get; set; }

        [Required, MaxLength(20)]
        public string Phone { get; set; }

        [Required, MaxLength(40)]
        public string Email { get; set; }

        [Required, MaxLength(40)]
        public string Speciality { get; set; }

        [Required, MaxLength(40)]
        public string Degree { get; set; }

        [Required, MaxLength(50)]
        public string ExpertIn { get; set; }

        [Required, MaxLength(30)]
        public string Category { get; set; }

        [MaxLength(100)]
        public string Facebook { get; set; }

        [MaxLength(100)]
        public string Twitter { get; set; }

        [MaxLength(100)]
        public string Instagram { get; set; }

        [MaxLength(100)]
        public string Linkedin { get; set; }

        [Column(TypeName = "ntext")]
        public string Photo { get; set; }

        [Required, MaxLength(2000), MinLength(500)]
        [Column(TypeName = "ntext")]
        public string Text { get; set; }

    }
}