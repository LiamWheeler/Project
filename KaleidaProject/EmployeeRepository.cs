using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace KaleidaProject
{
    public class EmployeeRepository
    {
        private static string DBPath = ConfigurationManager.AppSettings["CsvDatabasePath"];
        private static List<Employee> Employees = new List<Employee>();

        public  List<Employee> ProcessCsv(string path)
        {
            try
            {
                return File.ReadAllLines(path)
                .Skip(1)
                .Where(l => l.Count() > 6)
                .Select(Employee.ParseFromCsv)
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

        public void AddEmployeeToCSV(Employee employee)
        {
            var row = $"{Environment.NewLine}{employee.EmployeeId},{employee.FirstName}," +
                        $"{employee.LastName},{employee.DateOfBirth},{employee.StartDate},{employee.HomeTown},{employee.Department}";
            File.AppendAllText(DBPath, row);
        }

        public void RemoveEmployeeFromCSV(Employee employee)
        {
            var rowToDelete = $"{Environment.NewLine}{employee.EmployeeId},{employee.FirstName}," +
                                $"{employee.LastName},{employee.DateOfBirth},{employee.StartDate},{employee.HomeTown},{employee.Department}";
            var file = File.ReadAllText(DBPath);
            file = file.Replace(rowToDelete, "");
            File.WriteAllText(DBPath, file);
        }

        private static string GetUserInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        public void UpdateEmployee(Employee employee)
        {
            var dateofBirthInput = GetUserInput("Enter date of birth. (DD/MM/YYYY)");
            DateTime.TryParse(dateofBirthInput, out DateTime dateofBirth);

            var NewDOB = $"{dateofBirth}";

            var OldDOB = $"{employee.DateOfBirth}";
            var file = File.ReadAllText(DBPath);
            file = file.Replace(OldDOB, NewDOB);

            File.WriteAllText(DBPath, file);
        }


    }
}

