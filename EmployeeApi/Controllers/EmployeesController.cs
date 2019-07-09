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
        // GET api/employees
        public List<Employee> GetEmployees()
        {
            return _dataStore.ProcessData(DBPath);           
        }

       [Route("api/employees/{id}")]
        // GET api/employees/id
        public IEnumerable<Employee> GetEmployee(int id)
        {
            return _dataStore.ProcessData(DBPath).Where(e => e.EmployeeId == id);
        }

        [Route("api/employees/towns")]
        // Get api/employees/employeesbytown
        public List<string> GetEmployeesByTown()
        {
            List<string> employeesByTown = new List<string>();
            var Employees = _dataStore.ProcessData(DBPath);
            var homeTownList = Employees.GroupBy(e => e.HomeTown)
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
        //[HttpGet]
        //Get api/employees/averageagebydepartment
        public List<string> GetListAverageAge()
        {
            List<string> departmentAges = new List<string>();
            var Employees = _dataStore.ProcessData(DBPath);

            var AgeByDepartment = Employees.GroupBy(e => e.Department)
                                            .OrderBy(e => e.Key)
                                            .Select(e => new
                                            {
                                                Department = e.Key,
                                                TotalAge = e.Sum(x => x.Age),
                                                EmployeesInDepartment = e.Count()
                                            });

            foreach (var employee in AgeByDepartment)
            {
                departmentAges.Add(employee.ToString());
            }
            return departmentAges;
        }

        [Route("api/employees")]
        [HttpPut]
        // POST api/values
        public List<Employee> PutNewEmployee([FromBody]Employee employee)
        {
            employees.Add(employee);
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
            var EmployeeToDelete = employees.FirstOrDefault(e => e.EmployeeId == id);

            employees.Remove(EmployeeToDelete);

            return employees;
            
        }
    }
}
