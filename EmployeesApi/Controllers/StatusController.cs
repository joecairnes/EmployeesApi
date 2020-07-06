using EmployeesApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesApi.Controllers
{
    public class StatusController : ControllerBase
    {
        ISystemTime Time;

        public StatusController(ISystemTime time)
        {
            Time = time;
        }

        // GET /status
        [HttpGet("status")]
        public ActionResult GetStatus()
        {
            // 1. TODO: Get actual status
            StatusResponse response = new StatusResponse
            {
                Status = "I'm giving it all she's got!",
                CheckedBy = "Montgomery Scott",
                LastChecked = Time.GetCurrent().AddMinutes(-15)
            };
            return Ok(response);
            //return "Everything is awesome!";
        }

        // 1. route params
        [HttpGet("books/{bookId:int}")]
        public ActionResult GetABook(int bookId)
        {
            return Ok($"Getting info on book {bookId}");
        }

        [HttpGet("blogs/{year:int}/{month:int:range(1,12)}/{day:int}")]
        public ActionResult GetBlogPostsFor(int year, int month, int day)
        {
            return Ok($"Getting blog posts for {month}/{day}/{year}");
        }

        // 2. Query strings
        [HttpGet("books")]
        public ActionResult GetBooks([FromQuery] string genre = "All")
        {
            return Ok($"Getting you books in the {genre} genre.");
        }

        // 3. Briefly, headers
        [HttpGet("whoami")]
        public ActionResult WhoAmI([FromHeader(Name="User-Agent")] string userAgent)
        {
            return Ok($"I see you are running {userAgent}");
        }


        //// GET /status
        //[HttpGet("status")]
        //public ActionResult GetStatus()
        //{
        //    return NotFound("No status available");
        //}
    }

    public class StatusResponse
    {
        public string Status { get; set; }
        public string CheckedBy { get; set; }
        public DateTime LastChecked { get; set; }
    }
}
