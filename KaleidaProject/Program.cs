using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaleidaProject
{
    static class Program
    {
        private static readonly string DBPath = ConfigurationManager.AppSettings["CsvDatabasePath"];
        private static EmployeeRepository EmployeeRepo = new EmployeeRepository();
      
        static void Main(string[] args)
        {
            EmployeeRepo.MainMenu();
            Console.ReadKey();          
        }
    }
}


