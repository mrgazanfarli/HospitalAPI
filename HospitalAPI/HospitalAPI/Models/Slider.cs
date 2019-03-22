using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class Slider
    {
        public int Id { get; set; }

        [Required, StringLength(40)]
        public string Desc { get; set; }

        [Column(TypeName = "ntext")]
        public string BackImage { get; set; }
    }
}