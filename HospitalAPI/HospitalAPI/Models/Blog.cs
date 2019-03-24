using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [Column(TypeName = "ntext")]
        public string SmallPhoto { get; set; }

        [Column(TypeName = "ntext")]
        public string DetailsPhoto { get; set; }

        public DateTime Date { get; set; }

        [Required, MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required, MaxLength(2000), MinLength(300)]
        [Column(TypeName = "ntext")]
        public string Text { get; set; }

        [Required, MaxLength(110)]
        public string Desc { get; set; }

        [MaxLength(500)]
        [Column(TypeName = "ntext")]
        public string SpecialText { get; set; }

        [MaxLength(30)]
        public string PostedBy { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public Author Author { get; set; }
    }
}