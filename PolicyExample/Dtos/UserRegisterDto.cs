namespace PolicyExample.Dtos
{
    public class UserRegisterDto
    {

        public string UserName { get; set;  }
        public string Email { get; set; }
        public string Password { get; set; }

        public string? City { get; set; }
        public DateTime? BirthDay { get; set; }

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
}
