using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WagWander.Models
{
    public class WagWanderModel
    {
        public IEnumerable<LocationDto> Locations { get; set; }
        public IEnumerable<MediaItemDto> MediaItems { get; set; }
    }
}