namespace ApiConsume.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class LoginDto
    {

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
