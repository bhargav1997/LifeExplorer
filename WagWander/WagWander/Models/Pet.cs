using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WagWander.Models
{
    public class Pet
    {
        [Key]
        public int PetId { get; set; }

        public string Name { get; set; }

        public string Species { get; set; }

        public string Breed { get; set; }

        public int Age { get; set; }

        // A pet can have many inquiries
        public ICollection<Inquiry> Inquiries { get; set; }
    }
    public class PetDto
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
    }


}