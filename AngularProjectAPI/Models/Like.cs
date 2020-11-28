using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Like
    {
        public int LikeID { get; set; }
        public int Number { get; set; }
        [ForeignKey("UserID")]
        public int? UserID { get; set; }
        public User User { get; set; }
        [ForeignKey("ArticleID")]
        public int? ArticleID { get; set; }
        public Article Article { get; set; }

    }
}

