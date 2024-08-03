using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WagWander.Models.ViewModels {
    public class DetailsMediaItem {
        
        public MediaItemDto SelectedMediaItem { get; set; }

        //all of the related users to that particular media item
        public IEnumerable<UserDto> RelatedUsers { get; set; }

        public IEnumerable<UserMediaItemDto> RelatedUserLists { get; set; }
    }
}