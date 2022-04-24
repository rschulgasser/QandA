using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

using System.Data.SqlClient;
using System;

using Microsoft.Extensions.Logging;

using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using System.Text;


namespace QandA.Data
{
  public  class QandARepository
    {
        private string _connectionString;

        public QandARepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Question> GetAll()
        {
            using var context = new QandAContext(_connectionString);
            return context.Questions.Include(q=>q.QuestionsTags).ThenInclude(q=>q.Tag).Include(q=>q.Answers).Include(i=>i.Likes).OrderByDescending(o => o.DatePosted).ToList();
        }
        public void Add(User user,string password)
        {
            using var context = new QandAContext(_connectionString);
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            context.Users.Add(user);
            context.SaveChanges();
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (isValidPassword)
            {
                return user; 
            }

            return null;
        }

        public User GetByEmail(string email)
        {
            using var context = new QandAContext(_connectionString);
            return context.Users.FirstOrDefault(f => f.Email.ToLower() == email.ToLower());
        }
        public Question GetQuestionById(int id)
        {
            using var context = new QandAContext(_connectionString);
            return context.Questions.Include(i => i.Likes).Include(i=>i.Answers).ThenInclude(i=>i.User).Include(i=>i.User).Include(i=>i.QuestionsTags).ThenInclude(i=>i.Tag).Include(i => i.QuestionsTags).ThenInclude(j=>j.Question).FirstOrDefault(f => f.Id == id);
        }
        public User GetUserByEmail(string email)
        {
            using var context = new QandAContext(_connectionString);
            // List <Question> qs= context.Questions.Include(i => i.Answers).Include(i => i.User).Include(i => i.QuestionsTags).ToList();
            // return qs.FirstOrDefault(f => f.User.Email.ToLower() == email.ToLower());
            return context.Users.FirstOrDefault(f => f.Email == email);
        }
        public bool AlreadyLiked(int questionId, int UserId)
        {
            using var context = new QandAContext(_connectionString);
            Question question = GetQuestionById(questionId);
            Likes likes = context.Likes.FirstOrDefault(f => f.QuestionId == questionId && f.UserId == UserId);
            
            if (likes == null)
            {
                return false;
            }
            else
            {
                return likes.Liked;
            }
            }
        public void AddLike(Likes like)
        {
            using var context = new QandAContext(_connectionString);
          
            //context.Likes.Attach(like);
            context.Likes.Add(like);
            context.SaveChanges();
        }
        private Tag GetTag(string name)
        {
            using var ctx = new QandAContext(_connectionString);
            return ctx.Tags.FirstOrDefault(t => t.Name == name);
        }

        private int AddTag(string name)
        {
            using var ctx = new QandAContext(_connectionString);
            var tag = new Tag { Name = name };
            ctx.Tags.Add(tag);
            ctx.SaveChanges();
            return tag.Id;
        }
        public void AddQuestion(Question question, IEnumerable<string> tags)
        {
            using var ctx = new QandAContext(_connectionString);
            ctx.Questions.Add(question);
            ctx.SaveChanges();
            foreach (string tag in tags)
            {
                Tag t = GetTag(tag);
                int tagId;
                if (t == null)
                {
                    tagId = AddTag(tag);
                }
                else
                {
                    tagId = t.Id;
                }
                ctx.QuestionsTags.Add(new QuestionsTag
                {
                    QuestionId = question.Id,
                    TagId = tagId
                });
            }

            ctx.SaveChanges();
        }
        public List<QuestionsTag> GetTagsForQuestion(int id)
        {
            using var context = new QandAContext(_connectionString);
            return context.QuestionsTags.Where(i => i.QuestionId == id).Include(t=>t.Tag).ToList();
        }
        public void AddAnswer(Answer answer)
        {
            using var context = new QandAContext(_connectionString);

       
            context.Answers.Add(answer);
            context.SaveChanges();
        }
        public int GetLikesByQuestionId(int questionId)
        {
            using var context = new QandAContext(_connectionString);
            return context.Likes.Where(p => p.QuestionId == questionId).ToList().Count();
        }
        public List<Question> GetQuestionsForTag(string name)
        {
            using var ctx = new QandAContext(_connectionString);
            return ctx.Questions.Include(q => q.QuestionsTags).ThenInclude(q => q.Tag).Include(q => q.Answers).Include(i => i.Likes).OrderByDescending(o => o.DatePosted)
                
                .Where(c => c.QuestionsTags.Any(t => t.Tag.Name == name))
                .ToList();
        }
    }


    
}
