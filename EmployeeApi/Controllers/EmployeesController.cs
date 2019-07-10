using EmployeeApi.Models;
using KaleidaProject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeApi.Controllers
{

    public class EmployeesController : ApiController
    {
        private static EmployeeDataStore _dataStore = new EmployeeDataStore();
        private static readonly string DBPath = ConfigurationManager.AppSettings["CsvDatabasePath"];
        List<Employee> employees = _dataStore.ProcessData(DBPath);

        [Route("api/employees")]
        [HttpGet]
        // GET api/employees
        public IEnumerable<Employee> GetEmployees()
        {
            return employees.OrderBy(e => e.EmployeeId);  
        }

       [Route("api/employees/{id}")]
       [HttpGet]
        // GET api/employees/id
        public IHttpActionResult GetEmployee(int id)
        {
            var employeeToReturn = employees.FirstOrDefault(e => e.EmployeeId == id);
            if (employeeToReturn == null)
            {
                return NotFound();
            }
            else return Ok(employeeToReturn);
        }

        [Route("api/employees")]
        [HttpPut]
        // POST api/values
        public IHttpActionResult PutNewEmployee([FromBody]Employee employee)
        {
            if(employee == null || employee.EmployeeId == 0 || employee.FirstName == null || employee.LastName == null || employee.DateOfBirth == null ||
                employee.StartDate == null || employee.HomeTown == null || employee.Department == null)
            {
                ModelState.AddModelError("Description","A new employee requires a Employee ID, First and Last name, date of birth, start date, home town and department");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           

                employees.Add(employee);
            var row = $"{Environment.NewLine}{employee.EmployeeId},{employee.FirstName}," +
            $"{employee.LastName},{employee.DateOfBirth.ToString("yyyy-MM-dd")},{employee.StartDate.ToString("yyyy-MM-yy")},{employee.HomeTown},{employee.Department}";
            File.AppendAllText(DBPath, row);
            return Ok(employees.OrderBy(e => e.EmployeeId));
        }

        [Route("api/employees/{id}")]
        [HttpPut]
        // PUT api/values/5
        public IHttpActionResult PutEditedEmployee(int id, [FromBody]DateTime newDOB)
        {
            if(newDOB == null || newDOB >= DateTime.Today)
            {
                ModelState.AddModelError("Description", "entered date of birth is  invalid");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var EmployeeToEdit = employees.FirstOrDefault(e => e.EmployeeId == id);

            if(EmployeeToEdit == null)
            {
                return NotFound();
            }

            var NewDOB = newDOB.ToString("yyyy-MM-dd");

            var OldDOB = EmployeeToEdit.DateOfBirth.ToString("yyyy-MM-dd");
            var file = File.ReadAllText(DBPath);
            file = file.Replace(OldDOB, NewDOB);
            File.WriteAllText(DBPath, file);

            return Ok(employees.OrderBy(e => e.EmployeeId));
        }

        [Route("api/employees/{id}")]
        [HttpDelete]
        // DELETE api/values/5
        public IHttpActionResult DeleteEmployee(int id)
        {
            var employeeToDelete = employees.FirstOrDefault(e => e.EmployeeId == id);
            if(employeeToDelete == null)
            {
                ModelState.AddModelError("Description", $"Employee with id: {id}, could not be found ");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rowToDelete = $"{Environment.NewLine}{employeeToDelete.EmployeeId},{employeeToDelete.FirstName}," +
                    $"{employeeToDelete.LastName},{employeeToDelete.DateOfBirth.ToString("yyyy-MM-dd")}," +
                    $"{employeeToDelete.StartDate.ToString("yyyy-MM-dd")},{employeeToDelete.HomeTown},{employeeToDelete.Department}";
            var file = File.ReadAllText(DBPath);
            file = file.Replace(rowToDelete, "");
            File.WriteAllText(DBPath, file);

            employees.Remove(employeeToDelete);

            return Ok(employees.OrderBy(e => e.EmployeeId));
            
        }

        [Route("api/employees/towns")]
        [HttpGet]
        // Get api/employees/employeesbytown
        public List<string> GetEmployeesByTown()
        {
            List<string> employeesByTown = new List<string>();
            var homeTownList = employees.GroupBy(e => e.HomeTown)
                               .OrderBy(e => e.Key)
                               .Select(e => new
                               {
                                   HomeTown = e.Key,
                                   numberOfEmployees = e.Count()
                               });

            foreach (var employee in homeTownList)
            {
                employeesByTown.Add(employee.ToString());
            }

            return employeesByTown;
        }

        [Route("api/employees/ages")]
        [HttpGet]
        //Get api/employees/averageagebydepartment
        public List<string> GetListAverageAge()
        {
            List<string> departmentAges = new List<string>();

            var AgeByDepartment = employees.GroupBy(e => e.Department)
                                            .OrderBy(e => e.Key)
                                            .Select(e => new
                                            {
                                                Department = e.Key,
                                                AverageAge = e.Sum(x => x.Age)/ e.Count()
                                            });

            foreach (var employee in AgeByDepartment)
            {
                departmentAges.Add(employee.ToString());
            }
            return departmentAges;
        }

        [Route("api/employees/anniversaries")]
        [HttpGet]
        public List<string> GetUpcomingAnniversaries()
        {
            List<string> anniversaries = new List<string>();

            var UpcomingAnniversaries = employees.Where(e => e.Anniversary == true)
                                                 .Select(e => new
                                                 {
                                                     Name = e.FirstName + " " + e.LastName,
                                                     DaysTillAnniversary = e.DaysTillAnniversary,
                                                    StartDate = e.StartDate.ToShortDateString()
                                                 });

            foreach (var employee in UpcomingAnniversaries)
            {
                anniversaries.Add(employee.ToString());
            }

            return anniversaries;
        }

    }
}
