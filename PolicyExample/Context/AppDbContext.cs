using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PolicyExample.Context
{
    public class AppDbContext:IdentityDbContext<AppUser,IdentityRole,string>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
                
        }

    }


    public class AppUser:IdentityUser
    {
        public string? City { get; set; }
        public DateTime? BirthDay { get; set; }

    }
}
