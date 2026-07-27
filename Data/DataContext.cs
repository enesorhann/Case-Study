using CaseStudy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseStudy.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Survey> Surveys { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Survey>()
                .Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Survey>()
                .Property(s => s.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<Survey>()
                .Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            modelBuilder.Entity<Survey>()
                .Property(s => s.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<Survey>()
                .HasIndex(s => s.CreatedAt);

            modelBuilder.Entity<Question>()
                .Property(q => q.Text)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Question>()
                .Property(q => q.QuestionType)
                .IsRequired()
                .HasMaxLength(20)
                .HasConversion<string>();

            modelBuilder.Entity<Question>()
                .Property(q => q.Order)
                .IsRequired();

            modelBuilder.Entity<Question>()
                .Property(q => q.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<Question>()
                .HasIndex(q => new { q.SurveyId, q.Order });

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Survey)
                .WithMany(s => s.Questions)
                .HasForeignKey(q => q.SurveyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Answer>()
                .Property(a => a.RespondentName)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Answer>()
                .Property(a => a.Value)
                .IsRequired()
                .HasMaxLength(2000);

            modelBuilder.Entity<Answer>()
                .Property(a => a.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<Answer>()
                .HasIndex(a => a.QuestionId);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
