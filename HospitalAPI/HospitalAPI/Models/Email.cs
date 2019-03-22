using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class Email
    {
        public int Id { get; set; }

        [Required, MaxLength(40)]
        public string EmailAddress { get; set; }
    }
}