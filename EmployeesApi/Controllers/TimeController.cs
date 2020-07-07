using EmployeesApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesApi.Controllers
{
    public class TimeController : ControllerBase
    {
        // ALternate way of setting up injection
        [HttpGet("time")] // GET Time
        public ActionResult GetTheTime([FromServices] ISystemTime clock)
        {
            //throw new ArgumentOutOfRangeException();
            return Ok($"The time is {clock.GetCurrent().ToLongTimeString()}");
        }
    }
}
