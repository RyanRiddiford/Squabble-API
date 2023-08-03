using System.ComponentModel.DataAnnotations;

namespace Squabble.Models.RequestModels
{
    public class ResetPasswordRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string SecurityAnswerOne { get; set; }

        [Required]
        public string SecurityAnswerTwo { get; set; }
    }
}
