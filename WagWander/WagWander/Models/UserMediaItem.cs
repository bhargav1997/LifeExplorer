using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WagWander.Models {
    public class UserMediaItem {
        [Key]
        public int UserMediaItemID { get; set; }

        //Multiple users can have lists
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }
       

        //Muliple media items can be present in a list
        [ForeignKey("MediaItem")]
        public int MediaItemID { get; set; }
        public virtual MediaItem MediaItem { get; set; }

        public int? Rating { get; set; } // Rating between 0 and 10
        
        [AllowHtml]
        public string Review { get; set; }
        public string Status { get; set; } // "Playing", "Completed", "Watching", "Dropped", "Planning"
        

    }

    public class UserMediaItemDto {
        public int UserMediaItemID { get; set; }

        //Multiple users can have lists
        public int UserID { get; set; }
        
        public string UserName { get; set; }
        //Muliple media items can be present in a list
        public int MediaItemID { get; set; }
        public string Title { get; set; }

        public string Type { get; set; }

        public int? Rating { get; set; } // Rating between 0 and 10
        public string Review { get; set; }
        public string Status { get; set; } // "Playing", "Completed", "Watching", "Dropped", "Planning"
    }
}