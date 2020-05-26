using System;
using System.ComponentModel.DataAnnotations;
using web_s10197636.DAL;
namespace web_s10197636.Models
{
    public class ValidateEmailExists : ValidationAttribute
    {
        private StaffDAL staffContext = new StaffDAL();
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string email = Convert.ToString(value);
            // Casting the validation context to the "Staff" model class
            Staff staff = (Staff)validationContext.ObjectInstance;
            // Get the Staff Id from the staff instance
            int staffId = staff.StaffId;
            if (staffContext.IsEmailExist(email, staffId))
                // validation failed
                return new ValidationResult
                ("Email address already exists!");
            else
                // validation passed
                return ValidationResult.Success;
        }

    }
}
