using EmployeesApi.Models;
using EmployeesApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Produces("application/json")]
        public ActionResult<StatusResponse> GetStatus()
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
        /// <summary>
        /// Get a book by giving us an ID
        /// </summary>
        /// <param name="bookId">The ID of the book you want.</param>
        /// <returns>Information about the book</returns>
        /// <response code="200">This worked! Here is your book info!</response>
        /// <response code="404">There is no book with that ID. Please check.</response>
        [HttpGet("books/{bookId:int}")]
        [SwaggerResponse(200, "Everything is cool")] // these aren't really necessary, but some people like to use them
        [SwaggerResponse(400, "Cannot find that", typeof(ErrorResponse))] // these aren't really necessary, but some people like to use them
        public ActionResult GetABook(int bookId)
        {
            if (bookId % 2 == 0)
            {
                return Ok($"Getting info on book {bookId}");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("blogs/{year:int}/{month:int:range(1,12)}/{day:int}")]
        public ActionResult GetBlogPostsFor(int year, int month, int day)
        {
            return Ok($"Getting blog posts for {month}/{day}/{year}");
        }

        // 2. Query strings 
        [HttpGet("books")] // filter a collection resource
        public ActionResult GetBooks([FromQuery] string genre = "All")
        {
            return Ok($"Getting you books in the {genre} genre.");
        }

        // 3. Briefly, headers
        [HttpGet("whoami")]
        public ActionResult WhoAmI([FromHeader(Name = "User-Agent")] string userAgent)
        {
            return Ok($"I see you are running {userAgent}");
        }

        // 4. Entities
        [HttpPost("games")]
        public ActionResult AddGame([FromBody] PostGameRequest game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok($"adding {game.Title} for {game.Platform} for {game.Price.Value:c}");
            }
        }
    }

    public class PostGameRequest : IValidatableObject
    {
        [Required]
        [StringLength(50, ErrorMessage ="That name is too danged long!")]
        public string Title { get; set; }
        [Required]
        public string Platform { get; set; }
        [Required]
        public decimal? Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title.ToLower() == "fortnite" && Platform.ToLower() == "ps4")
            {
                yield return new ValidationResult("It sucks on the ps4",
                    new string[] { "Tile, Platform" });
            }
        }
    }



    //// GET /status
    //[HttpGet("status")]
    //public ActionResult GetStatus()
    //{
    //    return NotFound("No status available");
    //}

    public class StatusResponse
    {
        public string Status { get; set; }
        public string CheckedBy { get; set; }
        public DateTime LastChecked { get; set; }
    }
}
