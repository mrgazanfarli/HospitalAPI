using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class Counter
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string IconClass { get; set; }

        [Required, MaxLength(30)]
        public string Key { get; set; }

        [Required]
        public int Value { get; set; }

        [Required]
        public byte Order { get; set; }
    }
}