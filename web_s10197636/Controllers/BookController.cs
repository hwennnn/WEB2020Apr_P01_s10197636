using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using web_s10197636.Models;

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
    }
}
