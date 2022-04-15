using System;
using System.Collections.Generic;

namespace QandA.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
     
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Likes> Likes { get; set; } = new();
        public List<Question> Questions { get; set; } = new();
        public List<Answer> Answers { get; set; } = new();

    }
}
