using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class ContactInfo
    {
        public int Id { get; set; }

        [Required, MaxLength(80)]
        public string ContactDesc { get; set; }

        [MaxLength(100)]
        public string Facebook { get; set; }

        [MaxLength(100)]
        public string Twitter { get; set; }

        [MaxLength(100)]
        public string Google { get; set; }

        [MaxLength(100)]
        public string Linkedin { get; set; }

        [Required, MaxLength(100)]
        public string VideoLink { get; set; }

        [Required, MaxLength(60)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string Lon { get; set; }

        [MaxLength(100)]
        public string Lat { get; set; }

    }
}