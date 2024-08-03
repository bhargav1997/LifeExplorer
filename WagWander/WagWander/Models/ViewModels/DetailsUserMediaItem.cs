using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WagWander.Models.ViewModels
{
    public class DetailsUserMediaItem
    {
        public UserMediaItemDto UserMediaItem { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
        public IEnumerable<MediaItemDto> MediaItems { get; set; }
    }
}