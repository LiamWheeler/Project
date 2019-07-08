using EmployeeApi.Models;
using KaleidaProject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace EmployeeApi.Controllers
{

    public class EmployeesController : ApiController
    {
        private static EmployeeDataStore _dataStore = new EmployeeDataStore();
        List<Employee> employeeList = new List<Employee>();
        private static readonly string DBPath = ConfigurationManager.AppSettings["CsvDatabasePath"];

        // GET api/values
        public List<Employee> GetEmployees()
        {
            return _dataStore.ProcessData(DBPath);           
        }

        //// GET api/values/5
        public IEnumerable<Employee> Get(int id)
        {
            return _dataStore.ProcessData(DBPath).Where(e => e.EmployeeId == id);
        }

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
