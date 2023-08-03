namespace Squabble.Models.RequestModels
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool Sso { get; set; }

        public string MicrosoftSsoId { get; set; }
    }
}
