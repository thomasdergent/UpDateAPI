using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models.ReactionModel
{
    public class ReactionPost
    {
        public int ReactionID { get; set; }
        public string Body { get; set; }
        public int UserID { get; set; }
        public int ArticleID { get; set; }
    }
}
