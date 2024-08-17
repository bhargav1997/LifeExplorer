using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WagWander.Models {
    public class MediaItem {
        [Key]
        public int MediaItemID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; } // Either "Game" or "Anime"
      
        [AllowHtml]
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }

        public virtual ICollection<UserMediaItem> UserMediaItems { get; set; }

        // Foreign Key
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

    }

    public class MediaItemDto {
        public int MediaItemID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; } // Either "Game" or "Anime"
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }

        // Added location details
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }
}