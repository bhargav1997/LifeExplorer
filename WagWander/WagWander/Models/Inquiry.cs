using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WagWander.Models
{
    public class Inquiry
    {
        [Key]
        public int InquiryId { get; set; }

        public string PetName { get; set; }

        [ForeignKey("Pet")]
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }

        public string Username { get; set; }

        public string InquiryText { get; set; }
    }

    public class InquiryDto
    {
        public int InquiryId { get; set; }
        public string PetName { get; set; }
        public int PetId { get; set; }
        public string Username { get; set; }
        public string InquiryText { get; set; }
    }
}
