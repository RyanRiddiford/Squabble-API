//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using Squabble.Data;

//namespace Squabble.Models.CustomAttributes
//{
//    //Checks if the username chosen already exists
//    public class IsUniqueUserName : ValidationAttribute
//    {

//        private readonly SquabbleContext _context;
//        public IsUniqueUserName(SquabbleContext context)
//        {
//            _context = context;
//        }
    
//        public string GetErrorMessage()
//        {
//            return string.Format("Username already taken!");
//        }

//        //Data attribute validation
//        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//        {

//            if (_context.Accounts.Where(x => x.UserName == (string)value) == null)
//            {
//                return ValidationResult.Success;
//            }
//            else
//            {
//                return new ValidationResult(GetErrorMessage());
//            }

                

//        }
//    }
//}
