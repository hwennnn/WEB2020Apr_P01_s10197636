using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web_s10197636.Models;

namespace web_s10197636.Controllers
{
    public class FineController : Controller
    {
        public ActionResult Calculate()
        {
            //Prepare the ViewData to be use in Calculate.cshtml view
            ViewData["ShowResult"] = false;
            Fine fine = new Fine
            {
                DueDate = DateTime.Today,
                FineRate = 0.50
            };
            return View(fine);
        }

        [HttpPost]
        public ActionResult Calculate(Fine fine)
        {
            // The fine object contains user inputs from view
            if (!ModelState.IsValid) // validation fails
            {
                return View(fine); // returns the view with errors
            }
            // Calculate the cumulative fine and its breakdown
            double fineTotal = 0.0;
            string fineBreakdown = "";
            for (int count = 1; count <= fine.NumBooksOverdue; count++)
            {
                double fineForEachBook = count * fine.FineRate *
                fine.NumDaysOverdue;
                fineTotal += fineForEachBook;
                fineBreakdown += "Overdue cost for Book " + count + " = $" +
                fineForEachBook.ToString("#,##0.00") +
               "<br />";
            }
            fine.FineAmt = fineTotal;
            // Prepare the ViewData to be used in Calculate.cshtml view
            ViewData["ShowResult"] = true;
            ViewData["FineBreakdown"] = fineBreakdown;
            // Route to Calculate.cshtml view to display result
            return View(fine);
        }
    }
    
}
