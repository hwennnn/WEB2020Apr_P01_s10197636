﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace web_s10197636.Models
{
    public class Fine
    {
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Number of Books Overdue")]
        [Range(1, 10, ErrorMessage = "Invalid value! Please enter a value from 1 to 10")]
        public int NumBooksOverdue { get; set; }

        [Display(Name = "Number of Days Overdue")]
        public int NumDaysOverdue { get; set; }

        [Display(Name = "Fine Rate (SGD)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.00}")]
        public double FineRate { get; set; }

        [Display(Name = "Fine (SGD)")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public double FineAmt { get; set; }

    }
}
