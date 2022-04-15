using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QandA.Data
{
   public class QandAContext: DbContext
    {
        private readonly string _connectionString;

        public QandAContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet <Answer> Answers { get; set; }
        public DbSet<Likes> Likes { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QuestionsTag> QuestionsTags { get; set; }
        public DbSet<User> Users{ get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //set up composite primary key
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<QuestionsTag>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });
            modelBuilder.Entity<Likes>()
                .HasKey(qt => new { qt.QuestionId, qt.UserId });

        }
    }
}

