using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class Phone
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string Number { get; set; }
    }
}