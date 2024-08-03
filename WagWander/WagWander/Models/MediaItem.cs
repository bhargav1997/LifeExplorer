using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WagWander.Models {
    public class MediaItem {
        [Key]
        public int MediaItemID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; } // Either "Game" or "Anime"
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }

        public virtual ICollection<UserMediaItem> UserMediaItems { get; set; }
    }

    public class MediaItemDto {
        public int MediaItemID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; } // Either "Game" or "Anime"
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
    }
}