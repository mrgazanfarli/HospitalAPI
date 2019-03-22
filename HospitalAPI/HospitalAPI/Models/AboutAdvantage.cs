using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class AboutAdvantage
    {
        public int Id { get; set; }

        [Required, MaxLength(25)]
        public string Name { get; set; }
    }
}