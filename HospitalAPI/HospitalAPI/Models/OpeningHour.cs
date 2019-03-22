using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class OpeningHour
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string Key { get; set; }

        [Required, MaxLength(30)]
        public string Value { get; set; }
    }
}