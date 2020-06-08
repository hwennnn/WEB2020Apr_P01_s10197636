using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_s10197636.DAL;
using web_s10197636.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_s10197636.Controllers
{
    public class BranchController : Controller
    {
        private BranchDAL branchContext = new BranchDAL();

        // GET: Branch
        public ActionResult Index(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Home");
            }

            BranchViewModel branchVM = new BranchViewModel();
            branchVM.branchList = branchContext.GetAllBranches();

            // BranchNo (id) present in the query string
            if (id != null)
            {
                ViewData["selectedBranchNo"] = id.Value;
                branchVM.staffList = branchContext.GetBranchStaff(id.Value);
            }
            else
            {
                ViewData["selectedBranchNo"] = "";
            }

            return View(branchVM);
        }
    }
}
