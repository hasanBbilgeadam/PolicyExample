using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace PolicyExample.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public DbSet<Daily> Dailies { get; set; }
        public DbSet<Log> Logs { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

    }


    public class AppUser : IdentityUser
    {
        public string? City { get; set; }
        public DateTime? BirthDay { get; set; }
        public string?  Name { get; set; }
        public string?  SurName { get; set; }
        public int Balance { get; set; }

        public List<Daily> Dailies { get; set; } = new();
    }

    public class Daily
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string? AddedIP { get; set; }

        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }

    }

    public class Log
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string ErrorMessage { get; set; }
    }
}
