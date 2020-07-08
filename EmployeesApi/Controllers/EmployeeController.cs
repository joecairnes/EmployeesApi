using AutoMapper;
using EmployeesApi.Domain;
using EmployeesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using EmployeesApi.Services;

namespace EmployeesApi.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeesDataContext Context;
        private readonly IMapper Mapper;
        private readonly MapperConfiguration MapperConfig;

        public EmployeeController(EmployeesDataContext context, IMapper mapper, MapperConfiguration mapperConfig)
        {
            Context = context;
            Mapper = mapper;
            MapperConfig = mapperConfig;
        }

        [HttpPut("employees/{id:int}/firstname")]
        public async Task<ActionResult> UpdateFirstName(int id, [FromBody] string firstname)
        {
            var employee = await Context.Employees.SingleOrDefaultAsync(e => e.Id == id && e.Active);
            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                employee.FirstName = firstname;
                await Context.SaveChangesAsync();
                return NoContent();
            }
        }

        // delete
        [HttpDelete("employees/{id:int}")]
        public async Task<ActionResult> Fire(int id)
        {
            var employee = await Context.Employees.SingleOrDefaultAsync(e => e.Id == id && e.Active);
            if (employee != null)
            {
                employee.Active = false;
                await Context.SaveChangesAsync();
            }
            return NoContent(); // this is a success status code. It just means there is no entity (body) in the response.
            // sort of the passive-aggressive "Fine" of APIs.
            // It would make no sense to 404
        }


        // POST to employees
        [HttpPost("employees")]
        [ValidateModel]
        public async Task<ActionResult> Hire([FromBody] PostEmployeeRequest employeeToHire)
        {
            //// 1. validate and return 400 for bad
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //else
            //{
            // 2. Add it to the db
            //      a. Map it from the model to the employee
            var employee = Mapper.Map<Employee>(employeeToHire);

            //      b. Add it to the context
            Context.Employees.Add(employee); // the id is zero
                                             //      c. Save the changes
            await Context.SaveChangesAsync(); // the id is whatever the db assigned to it

            // 3. Response
            //      a. Return a 201 created status code
            //      b. Add a location header to the response 201 Created Location: like http://localhost:1337/99 or whatever
            //      c. Retunr a copy of whatever they would get if they did a get rewuest to the location, because they probably will
            var employeeToReturn = Mapper.Map<GetEmployeeDetailsResponse>(employee);
            return CreatedAtRoute("employees-get-by-id", new { id = employeeToReturn.Id }, employeeToReturn);
            //}
        }


        // GET forEMp ID
        [HttpGet("employees/{id:int}", Name = "employees-get-by-id")]
        public async Task<ActionResult> GetAnEmployee(int id)
        {
            var employee = await Context.Employees
                .Where(e => e.Id == id && e.Active)
                .ProjectTo<GetEmployeeDetailsResponse>(MapperConfig)
                .SingleOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(employee);
            }
        }


        [HttpGet("employees")]
        public async Task<ActionResult> GetAllEmployees()
        {
            var employees = await Context.Employees
                .Where(e => e.Active)
                .ProjectTo<EmployeeListItem>(MapperConfig)
                .ToListAsync();

            var response = new GetEmployeesResponse
            {
                Data = employees
            };
            return Ok(response);
        }
    }

}
