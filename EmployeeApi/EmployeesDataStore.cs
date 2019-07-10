using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using EmployeeApi.Models;

namespace KaleidaProject
{ 
    public class EmployeeDataStore
    {
        private static string DBPath = ConfigurationManager.AppSettings["CsvDatabasePath"];
        public  List<EmployeeDto> Employees { get; set; }

        public List<Employee> ProcessData(string path)
        {
            Employees = new List<EmployeeDto>();
            {
                new EmployeeDto();
                try
                {
                    return File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Count() > 6)
                    .Select(Employee.ParseData)
                    .ToList();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                    return new List<Employee>();
                }
            }
        }
    }
}