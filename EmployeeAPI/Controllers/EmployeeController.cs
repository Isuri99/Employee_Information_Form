using Microsoft.AspNetCore.Mvc;
using EmployeeAPI.Utilities;
using EmployeeAPI.Models;
using System.Data;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly DataBaseUtilities _dbService;

        public EmployeeController(DataBaseUtilities db)
        {
            _dbService = db;
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            try
            {
                _dbService.OpenConnection();
                DataTable dt = _dbService.Select("SelectEmployees");
                _dbService.CloseConnection();

                // Using LINQ to map DataTable to List (Modern Standard)
                var employees = dt.AsEnumerable().Select(row => new Employee
                {
                    EmployeeNumber = Convert.ToInt32(row["EmployeeNumber"]),
                    FirstName = row["FirstName"].ToString() ?? "",
                    LastName = row["LastName"].ToString() ?? "",
                    DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                    Salary = Convert.ToDecimal(row["Salary"])
                }).ToList();

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddOrUpdateEmployee([FromBody] Employee emp)
        {
            if (emp == null) return BadRequest("Employee data is null");

            try
            {
                _dbService.OpenConnection();
                _dbService.PopulateData(emp.EmployeeNumber, emp.FirstName, emp.LastName, emp.DateOfBirth, emp.Salary);
                _dbService.CloseConnection();

                return Ok(new { message = "Employee saved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                _dbService.OpenConnection();
                _dbService.Delete(id);
                _dbService.CloseConnection();
                return Ok(new { message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("average")]
        public IActionResult GetAverageSalary()
        {
            try
            {
                _dbService.OpenConnection();
                DataTable dt = _dbService.Select("SelectEmployees");
                _dbService.CloseConnection();

                if (dt.Rows.Count == 0) return Ok(0);

                // Use LINQ for the calculation
                decimal average = dt.AsEnumerable().Average(row => Convert.ToDecimal(row["Salary"]));

                return Ok(average);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}