using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using web_s10197636.Models;
using static web_s10197636.Models.Book;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_s10197636.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public async Task<ActionResult> Index()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://ictonejourney.com");
            HttpResponseMessage response = await client.GetAsync("/api/books");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<Book> bookList = JsonConvert.DeserializeObject<List<Book>>(data);
                return View(bookList);
            }
            else
            {
                return View(new List<Book>());
            }
        }

        [Authorize]
        public async Task<ActionResult> Reserve(int id)
        {
            //Retreive access token
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://ictonejourney.com");
            //Add the access token to the header
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
            BookReserve br = new BookReserve(id);
            //Convert the BookReserve object to JSON string
            string json = JsonConvert.SerializeObject(br);
            StringContent reserveContent = new StringContent(json,
             UnicodeEncoding.UTF8, "application/json");
            //Call Web API to create reserve a selected book
            HttpResponseMessage response = await client.PostAsync(
             "/api/books", reserveContent);
            if (response.IsSuccessStatusCode)
                TempData["Status"] = "Book Reserved";
            else
                TempData["Status"] = "Error Reserving Book";
            return RedirectToAction(nameof(Index));
        }
    }
}
