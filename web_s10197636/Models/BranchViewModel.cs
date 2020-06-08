using System;
using System.Collections.Generic;

namespace web_s10197636.Models
{
    public class BranchViewModel
    {
        public List<Branch> branchList { get; set; }
        public List<Staff> staffList { get; set; }

        public BranchViewModel()
        {
            branchList = new List<Branch>();
            staffList = new List<Staff>();
        }

    }
}
