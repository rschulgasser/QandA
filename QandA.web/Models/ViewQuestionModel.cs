using QandA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QandA.web.Models
{
    public class ViewQuestionModel
    {
        public Question Question { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool AlreadyLiked { get; set; }
    }
}
