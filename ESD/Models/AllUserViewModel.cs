using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ESD.Models
{
        public class AllUserViewModel
        {
            public IdentityUser User { get; set; }
            public SelectList Roles { get; set; }
            public string Role { get; set; }
        }
}
