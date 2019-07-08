using EmployeeApi.Models;
using KaleidaProject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace EmployeeApi.Controllers
{

    [Route("api/employees")]
    public class EmployeesController : Controller
    {
        private static List<Employee> Employees = new List<Employee>();
        private static readonly string DBPath = ConfigurationManager.AppSettings["CsvDatabasePath"];
        private static EmployeeDataStore EmployeeData = new EmployeeDataStore();

        // GET api/values
        [HttpGet()]
        public List<Employee> GetEmployees(string path)
        {
            Employees = EmployeeData.ProcessData(DBPath);
            return EmployeeDataStore.Current.ProcessData(path);
        }

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

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
