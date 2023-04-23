using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quizzes.Models;

namespace Quizzes.MVC.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<Question>().Navigation(q => q.Answers).AutoInclude();
        builder.Entity<Question>().Navigation(q => q.Quiz).AutoInclude();
        builder.Entity<Answer>().Navigation(a => a.Question).AutoInclude();
    }

    public DbSet<Quizzes.Models.Quiz>? Quiz { get; set; }
    public DbSet<Quizzes.Models.Question>? Question { get; set; }
    public DbSet<Quizzes.Models.Answer>? Answer { get; set; }
}
