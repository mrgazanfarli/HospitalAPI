using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class DescCard
    {
        public int Id { get; set; }

        [Required, StringLength(25)]
        public string Name { get; set; }

        [Required, StringLength(125)]
        public string Desc { get; set; }

        [Column(TypeName = "ntext")]
        public string Photo { get; set; }
    }
}