namespace Squabble.Models.RequestModels
{
    public class UpdateAccountRequest
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public string AvatarString { get; set; }
    }
}
