using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required, MaxLength(21)]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        public string Photo { get; set; }

        public List<Blog> Blogs { get; set; }
    }
}