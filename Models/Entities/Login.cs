using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Squabble.Models.Entities;

namespace Squabble.Models
{

    //Database model for the Login entity
    public class Login
    {

        [EmailAddress(ErrorMessage = "That email address is not valid.")]
        [MaxLength(320, ErrorMessage = "Email addresses can't be that long... can they?!")]
        [Required(ErrorMessage = "You must enter an email address.")]
        public string Email
        {
            get; set;
        }

        [Required(ErrorMessage = "You must enter a username.")]
        [MaxLength(20, ErrorMessage = "That username is too long.")]
        public string UserName
        {
            get; set;
        }


        // TODO: Temp disabled to support SSO. Does SSO even need to use this table or will it
        //       break other parts of the app if we don't use it?
        // [Required]
        public string PasswordHash
        {
            get; set;
        }

        [ForeignKey("User")]
        [Required]
        public int AccountId
        {
            get; set;
        }
        public virtual User Account { get; set; }

        // TODO: Temp disabled to support SSO. Does SSO even need to use this table or will it
        //       break other parts of the app if we don't use it?
        // [Required(ErrorMessage = "You must enter a security question.")]
        [MaxLength(150, ErrorMessage = "The security question is too long.")]
        public string SecurityQuestionOne { get; set; }

        // TODO: Temp disabled to support SSO. Does SSO even need to use this table or will it
        //       break other parts of the app if we don't use it?
        // [Required(ErrorMessage = "You must enter a security answer.")]
        [MaxLength(150, ErrorMessage = "The security answer is too long.")]
        public string SecurityAnswerOne { get; set; }

        // TODO: Temp disabled to support SSO. Does SSO even need to use this table or will it
        //       break other parts of the app if we don't use it?
        // [Required(ErrorMessage = "You must enter a security question.")]
        [MaxLength(150, ErrorMessage = "The security question is too long.")]
        public string SecurityQuestionTwo { get; set; }

        // TODO: Temp disabled to support SSO. Does SSO even need to use this table or will it
        //       break other parts of the app if we don't use it?
        // [Required(ErrorMessage = "You must enter a security answer.")]
        [MaxLength(150, ErrorMessage = "The security answer is too long.")]
        public string SecurityAnswerTwo { get; set; }
    }
}
