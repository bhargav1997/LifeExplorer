using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WagWander.Models.ViewModels {
    public class DetailsUser {
        public UserDto SelectedUser { get; set; }
        public IEnumerable<UserMediaItemDto> UserPersonalList { get; set; }
    }
}