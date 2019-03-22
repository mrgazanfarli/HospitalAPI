using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class CareCenter
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string Title { get; set; }

        [Required, MaxLength(150)]
        public string Desc { get; set; }

        [MaxLength(30)]
        public string IconClass { get; set; }

        [Required, MaxLength(11)]
        public string ButtonText { get; set; }
    }
}