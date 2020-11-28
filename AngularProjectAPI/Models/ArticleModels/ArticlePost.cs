using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models.ArticleModels
{
    public class ArticlePost
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ShortSummary { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }

        //Relations
        public int TagID { get; set; }
        public int UserID { get; set; }
        public int ArticleStatusID { get; set; }
    }
}
