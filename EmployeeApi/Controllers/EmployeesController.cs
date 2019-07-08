﻿using EmployeeApi.Models;
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
        [Route("api/employees")]
    public class EmployeesController : ApiController
    {
        private static EmployeeDataStore _dataStore = new EmployeeDataStore();
        List<Employee> employeeList = new List<Employee>();
        private static readonly string DBPath = ConfigurationManager.AppSettings["CsvDatabasePath"];

        // GET api/employees
        public List<Employee> GetEmployees()
        {
            return _dataStore.ProcessData(DBPath);           
        }

        [HttpGet()]
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


        // POST api/values
        //public IEnumerable Post( [FromBody]string value)
        //{
        //    return value;
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/values/5
        //public IEnumerable<Employee> DeleteEmployee(int id)
        //{
        //}
    }
}
