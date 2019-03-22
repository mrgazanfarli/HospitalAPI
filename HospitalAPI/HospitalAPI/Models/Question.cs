using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string SenderFullname { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Subject { get; set; }

        [MaxLength(30)]
        public string Number { get; set; }

        [Required, MaxLength(500), MinLength(50)]
        [Column(TypeName = "ntext")]
        public string Message { get; set; }

        public DateTime Date { get; set; }

        public bool IsHidden { get; set; }
    }
}