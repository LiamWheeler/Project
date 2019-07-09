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
        public List<Employee> GetEmployees()
        {
            return employees;  
        }

       [Route("api/employees/{id}")]
       [HttpGet]
        // GET api/employees/id
        public IEnumerable<Employee> GetEmployee(int id)
        {
            return employees.Where(e => e.EmployeeId == id);
        }

        [Route("api/employees")]
        [HttpPut]
        // POST api/values
        public List<Employee> PutNewEmployee([FromBody]Employee employee)
        {
            employees.Add(employee);
            var row = $"{Environment.NewLine}{employee.EmployeeId},{employee.FirstName}," +
            $"{employee.LastName},{employee.DateOfBirth.ToShortDateString()},{employee.StartDate.ToShortDateString()},{employee.HomeTown},{employee.Department}";
            File.AppendAllText(DBPath, row);
            return employees;
        }

        [Route("api/employees/{id}")]
        [HttpPut]
        // PUT api/values/5
        public List<Employee> PutEditedEmployee(int id, [FromBody]DateTime newDOB)
        {
            var EmployeeToEdit = employees.FirstOrDefault(e => e.EmployeeId == id);

            EmployeeToEdit.DateOfBirth = newDOB;
            return employees;
        }

        [Route("api/employees/{id}")]
        [HttpDelete]
        // DELETE api/values/5
        public List<Employee> DeleteEmployee(int id)
        {
            var employeeToDelete = employees.FirstOrDefault(e => e.EmployeeId == id);


            var rowToDelete = $"{Environment.NewLine}{employeeToDelete.EmployeeId},{employeeToDelete.FirstName}," +
                    $"{employeeToDelete.LastName},{employeeToDelete.DateOfBirth.ToShortDateString()}," +
                    $"{employeeToDelete.StartDate.ToShortDateString()},{employeeToDelete.HomeTown},{employeeToDelete.Department}";
            var file = File.ReadAllText(DBPath);
            file = file.Replace(rowToDelete, "");
            File.WriteAllText(DBPath, file);
            employees.Remove(employeeToDelete);
            return employees;
            
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
                                                TotalAge = e.Sum(x => x.Age),
                                                EmployeesInDepartment = e.Count(),
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
