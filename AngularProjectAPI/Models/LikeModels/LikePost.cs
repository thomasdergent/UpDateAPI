using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models.LikeModels
{
    public class LikePost
    {
        public int LikeID { get; set; }
        public int Number { get; set; }
        public int UserID { get; set; }
        public int ArticleID { get; set; }
    }
}
