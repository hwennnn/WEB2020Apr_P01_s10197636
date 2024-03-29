﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using web_s10197636.Models;

namespace web_s10197636.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StaffLogin(IFormCollection formData)
        {
            // Read inputs from textboxes
            // Email address converted to lowercase
            string loginID = formData["txtLoginID"].ToString().ToLower();
            string password = formData["txtPassword"].ToString();

            if (loginID == "abc@npbook.com" && password == "pass1234")
            {

                // Store Login ID in session with the key “LoginID”
                HttpContext.Session.SetString("LoginID", loginID);
                // Store user role “Staff” as a string in session with the key “Role”
                HttpContext.Session.SetString("Role", "Staff");
                // Redirect user to the "StaffMain" view through an action
                HttpContext.Session.SetString("LoginDT", DateTime.Now.ToString("dd-MMMM-y h:mm:ss tt"));
                return RedirectToAction("StaffMain");
            }

            else
            {
                // Store an error message in TempData for display at the index view
                TempData["Message"] = "Invalid Login Credentials!";
                return RedirectToAction("Index");
            }
            

        }


        public ActionResult StaffMain()
        {
           if(HttpContext.Session.GetString("Role") != null)
 {
                if (HttpContext.Session.GetString("Role") == "Staff")
                {
                    return View();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult LogOut()
        {
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();
            // Call the Index action of Home controller
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<ActionResult> StudentLogin()
        {
            //Retrieve the access token of the user
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            //Call API to obtain user information
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://ictonejourney.com");
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await
             client.GetAsync("/api/Users/userinfo");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                //Convert the JSON string into an Account object
                Account account = JsonConvert.DeserializeObject<Account>(data);
                HttpContext.Session.SetString("LoginID", account.Student.Name);
                HttpContext.Session.SetString("Role", "Student");
                HttpContext.Session.SetString("LoggedInTime",
                 DateTime.Now.ToString());
                return RedirectToAction("Index", "Book");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
