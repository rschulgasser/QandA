using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QandA.Data
{
  public  class QuestionsTag
    {
        public int QuestionId { get; set; }
        public int TagId { get; set; }
        public Question Question { get; set; } 
        public Tag Tag { get; set; } 
    }
}
