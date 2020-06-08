using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_s10197636.DAL;
using web_s10197636.Models;
using System.IO; 

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_s10197636.Controllers
{
    public class StaffController : Controller
    {

        private StaffDAL staffContext = new StaffDAL();
        private BranchDAL branchContext = new BranchDAL();

        public ActionResult Index()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Staff> staffList = staffContext.GetAllStaff();
            return View(staffList);
        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["CountryList"] = GetCountries();
            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Staff staff)
        {
            //Get country list for drop-down list
            //in case of the need to return to Create.cshtml view
            ViewData["CountryList"] = GetCountries();
            if (ModelState.IsValid)
            {
                //Add staff record to database
                staff.StaffId = staffContext.Add(staff);
                //Redirect user to Staff/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(staff);
            }
        }

        private List<SelectListItem> GetCountries()
        {
            List<SelectListItem> countries = new List<SelectListItem>();
            countries.Add(new SelectListItem
            {
                Value = "Singapore",
                Text = "Singapore"
            });
            countries.Add(new SelectListItem
            {
                Value = "Malaysia",
                Text = "Malaysia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Indonesia",
                Text = "Indonesia"
            });
            countries.Add(new SelectListItem
            {
                Value = "China",
                Text = "China"
            });
            return countries;
        }

        private List<Branch> GetAllBranches()
        {
            // Get a list of branches from database
            List<Branch> branchList = branchContext.GetAllBranches();
            // Adding a select prompt at the first row of the branch list
            branchList.Insert(0, new Branch
            {
                BranchNo = 0,
                Address = "--Select--"
            });
            return branchList;
        }

        // GET: Staff/Edit/5
        public ActionResult Edit(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            ViewData["BranchList"] = GetAllBranches();
            Staff staff = staffContext.GetDetails(id.Value);
            if (staff == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Staff staff)
        {
            //Get branch list for drop-down list
            //in case of the need to return to Edit.cshtml view
            ViewData["BranchList"] = GetAllBranches();

            if (ModelState.IsValid)
            {
                //Update staff record to database
                staffContext.Update(staff);
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(staff);
            }
        }

        // GET: Staff/Delete/5
        public ActionResult Delete(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            Staff staff = staffContext.GetDetails(id.Value);
            if (staff == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        // POST: Staff/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Staff staff)
        {
            // Delete the staff record from database
            staffContext.Delete(staff.StaffId);
            return RedirectToAction("Index");
        }

        // GET: Staff/Details/5
        public ActionResult Details(int id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Home");
            }
            Staff staff = staffContext.GetDetails(id);
            StaffViewModel staffVM = MapToStaffVM(staff);
            return View(staffVM);
        }

        public StaffViewModel MapToStaffVM(Staff staff)
        {
            string branchName = "";
            if (staff.BranchNo != null)
            {
                List<Branch> branchList = branchContext.GetAllBranches();
                foreach (Branch branch in branchList)
                {
                    if (branch.BranchNo == staff.BranchNo.Value)
                    {
                        branchName = branch.Address;
                        //Exit the foreach loop once the name is found
                        break;
                    }
                }
            }

            string employmentStatus;
            if (staff.IsFullTime)
                employmentStatus = "Full-Time";
            else
                employmentStatus = "Part-Time";

            StaffViewModel staffVM = new StaffViewModel
            {
                StaffId = staff.StaffId,
                Name = staff.Name,
                Gender = staff.Gender,
                DOB = staff.DOB,
                Nationality = staff.Nationality,
                Email = staff.Email,
                Salary = staff.Salary,
                Status = employmentStatus,
                BranchName = branchName,
                Photo = staff.Name + ".jpg"
            };

            return staffVM;
        }

        public ActionResult UploadPhoto(int id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Home");
            }
            Staff staff = staffContext.GetDetails(id);
            StaffViewModel staffVM = MapToStaffVM(staff);
            return View(staffVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhoto(StaffViewModel staffVM)
        {
            if (staffVM.fileToUpload != null && staffVM.fileToUpload.Length > 0)
            {
                try
                {
                    // Find the filename extension of the file to be uploaded.
                    string fileExt = Path.GetExtension(
                    staffVM.fileToUpload.FileName);
                    Console.WriteLine(fileExt);

                    // Rename the uploaded file with the staff’s name.
                    string uploadedFile = staffVM.Name + fileExt;
                    Console.WriteLine(uploadedFile);

                    // Get the complete path to the images folder in server
                    string savePath = Path.Combine(
                    Directory.GetCurrentDirectory(),"wwwroot//images", uploadedFile); // i think "wwwroot\\images" is wrong so i change to "//"
                    
                    Console.WriteLine(savePath);

                    // Upload the file to server
                    using (var fileSteam = new FileStream(
                    savePath, FileMode.Create))
                    {
                        await staffVM.fileToUpload.CopyToAsync(fileSteam);
                    }

                    staffVM.Photo = uploadedFile;
                    ViewData["Message"] = "File uploaded successfully.";
                }
                catch (IOException e)
                {
                    Console.WriteLine(
                   "{0}: The write operation could not " +
                   "be performed because the specified " +
                   "part of the file is locked.",
                   e.GetType().Name);
                    //File IO error, could be due to access rights denied
                    ViewData["Message"] = "File uploading fail!";
                }
                catch (Exception ex) //Other type of error
                {
                    ViewData["Message"] = ex.Message;
                }
            }
            return View(staffVM);
        }
    }
}
