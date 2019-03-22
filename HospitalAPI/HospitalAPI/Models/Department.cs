using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required, MaxLength(20)]
        public string HeadPosition { get; set; }

        [Required, MaxLength(40)]
        public string HeadFullname { get; set; }

        [MaxLength(30)]
        public string EventDays { get; set; }

        [MaxLength(35)]
        public string EventName { get; set; }

        [Required, MaxLength(35)]
        public string Address { get; set; }

        [Required, MaxLength(40)]
        public string Email { get; set; }

        [Required, MaxLength(20)]
        public string Phone { get; set; }

        [Column(TypeName = "ntext")]
        public string Photo { get; set; }

        [Required, MaxLength(70)]
        public string Title { get; set; }

        [Required, MaxLength(1500), MinLength(300)]
        [Column(TypeName = "ntext")]
        public string Text { get; set; }

        [MaxLength(100)]
        public string Facebook { get; set; }

        [MaxLength(100)]
        public string Twitter { get; set; }

        [MaxLength(100)]
        public string Instagram { get; set; }
    }
}