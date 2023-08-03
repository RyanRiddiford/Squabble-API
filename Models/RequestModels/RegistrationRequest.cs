using System.ComponentModel.DataAnnotations;
using Squabble.Models.BaseModels;

namespace Squabble.Models.RequestModels
{
    //Registration model for a new account request
    public class RegistrationRequest : BaseUser
    {
        // TODO: Re-enable required password, by moving SSO registration to its own model, with no
        //       password fields.
        // [Required(ErrorMessage = "Password is a required field")]
        public string Password
        {
            get; set;
        }

        // [Required(ErrorMessage = "Password is a required field")]
        public string ConfirmPassword
        {
            get; set;
        }

        // [Required(ErrorMessage = "You must enter a security question.")]
        [MaxLength(150, ErrorMessage = "The security question is too long.")]
        public string SecurityQuestionOne { get; set; }

        // [Required(ErrorMessage = "You must enter a security answer.")]
        [MaxLength(150, ErrorMessage = "The security answer is too long.")]
        public string SecurityAnswerOne { get; set; }

        // [Required(ErrorMessage = "You must enter a security question.")]
        [MaxLength(150, ErrorMessage = "The security question is too long.")]
        public string SecurityQuestionTwo { get; set; }

        // [Required(ErrorMessage = "You must enter a security answer.")]
        [MaxLength(150, ErrorMessage = "The security answer is too long.")]
        public string SecurityAnswerTwo { get; set; }

        public string MicrosoftSsoId { get; set; }
    }
}
