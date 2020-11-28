using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models.ReactionModels
{
    public class Reaction
    {
        public int ReactionID { get; set; }
        public int CommentID { get; set; }
        [ForeignKey("UserID")]
        public int? UserID { get; set; }
        public int ArticleID { get; set; }
    }
}
