using System;
using System.ComponentModel.DataAnnotations;
namespace web_s10197636.Models
{
    public class Branch
    {
        [Display(Name = "ID")]
        public int BranchNo { get; set; }

        public string Address { get; set; }

        [RegularExpression(@"[689]\d{7}|\+65[689]\d{7}$", ErrorMessage = "Invalid Singapore PhoneNumber")]
        public string Telephone { get; set; }
    }
}
