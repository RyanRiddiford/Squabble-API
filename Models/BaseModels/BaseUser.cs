using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Squabble.Models.BaseModels
{
    //Base class with common user properties
    public abstract class BaseUser
    {


        //TODO: DATA ANNOTATIONS
        //TODO: SHOULD BE UNIQUE
        [Required(ErrorMessage = "Username is a required field")]
        public string UserName
        {
            get; set;
        }


        [Required(ErrorMessage = "First name is a required field")]
        [RegularExpression("^[a-zA-Z]{1,50}$", ErrorMessage = "First name must be between 1-50 characters long")]
        public string FirstName
        {
            get; set;
        }

        [RegularExpression("^[a-zA-Z]{1,50}$", ErrorMessage = "Middle name must be between 1-50 characters long")]
        public string MiddleName
        {
            get; set;
        }

        [RegularExpression("^[a-zA-Z]{1,50}$", ErrorMessage = "Surname must be between 1-50 characters long")]
        [Required(ErrorMessage = "Surname is a required field")]
        public string Surname
        {
            get; set;
        }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(320, ErrorMessage = "Must be no more than 320 characters")]
        [Required(ErrorMessage = "First name is a required field")]
        public string Email
        {
            get; set;
        }
    }
}
