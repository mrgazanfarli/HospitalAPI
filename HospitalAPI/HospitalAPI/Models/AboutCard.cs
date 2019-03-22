using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class AboutCard
    {
        public int Id { get; set; }

        [Required, MaxLength(25)]
        public string Name { get; set; }

        [Required, MaxLength(160)]
        public string Desc { get; set; }

        [Required, MaxLength(30)]
        public string IconClass { get; set; }
    }
}