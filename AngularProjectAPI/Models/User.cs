using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string Token { get; set; }

        //Relations
        public int RoleID { get; set; }
        public Role Role { get; set; }
        [JsonIgnore]
        public ICollection<Reaction> Reactions { get; set; }
        [JsonIgnore]
        public ICollection<Like> Likes { get; set; }
    }
}
