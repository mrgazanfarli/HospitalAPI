using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class DepartmentCard
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string IconClass { get; set; }

        [Required, MaxLength(20)]
        public string Name { get; set; }

        [Required, MaxLength(110)]
        public string Desc { get; set; }
    }
}