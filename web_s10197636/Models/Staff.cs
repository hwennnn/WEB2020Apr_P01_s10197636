using System;
using System.ComponentModel.DataAnnotations;

namespace web_s10197636.Models
{
    public class Staff
    {
        [Display(Name = "ID")]
        public int StaffId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public char Gender { get; set; }

        [Required] // set DOB field as required to prevent error
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        public string Nationality { get; set; }

        [Display(Name = "Email address")]
        [Required]
        // Validation Annotation for email address format
        [EmailAddress]
        // Custom Validation Attribute for checking email address exists
        [ValidateEmailExists]
        public string Email { get; set; }

        [Display(Name = "Monthly Salary (SGD)")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        [Range(1.0,10000.00)]
        public decimal Salary { get; set; }

        [Display(Name = "Full-Time Staff")]
        public bool IsFullTime { get; set; }

        [Display(Name = "Branch")]
        public int? BranchNo { get; set; }


    }
}
