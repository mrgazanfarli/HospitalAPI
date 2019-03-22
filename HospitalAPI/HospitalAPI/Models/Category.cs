using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public List<Blog> Blogs { get; set; }
    }
}