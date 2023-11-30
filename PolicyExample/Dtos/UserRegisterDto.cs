using PolicyExample.Context;

namespace PolicyExample.Dtos
{
    public class UserRegisterDto
    {

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string? City { get; set; }
        public DateTime? BirthDay { get; set; }

        public string? Name { get; set; }
        public string? SurName { get; set; }
        public int Balance { get; set; } = 0;

    }

    public class UserLoginDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

    }
    public class UserClaimAddDto
    {
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }


    public class DailyCreateDto
    {
        //public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        //public string? AddedIP { get; set; }
        // public string AppUserId { get; set; }

        /*
         
        public string? AddedIP { get; set; }

        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }

         
         */
    }


    public class DailyUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        //public DateTime Date { get; set; }
        public string AppUserId { get; set; }

    }
}
