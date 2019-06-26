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

        private static List<Employee> ProcessCsv(string path)
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

        public void MainMenu()
        {
            Employees = ProcessCsv(DBPath);

            Console.WriteLine("\r\nOptions:");
            Console.WriteLine("1. List all employees.");
            Console.WriteLine("2. Add a new employee.");
            Console.WriteLine("3. Edit an existing employee.");
            Console.WriteLine("4. Remove an existing employee.");
            Console.WriteLine("5. List all employees whose work anniversary is within the next month.");
            Console.WriteLine("6. List the average age of the employees in each department.");
            Console.WriteLine("7. List the number of employees in each town.");
            Console.WriteLine("8. Exit.");

            var input = Console.ReadLine().Trim();
            Int32.TryParse(input, out var userInput);

            var AppInUse = true;
            while (AppInUse)
            {
                try
                {

                    if (userInput == 1)
                    {
                        ListEmployees();
                        MainMenu();
                    }
                    else if (userInput == 2)
                    {
                        var employee = ManualAdd();
                        AddEmployeeToCSV(employee);
                        MainMenu();
                    }
                    else if (userInput == 3)
                    {
                        EditEmployee();
                        MainMenu();
                    }
                    else if (userInput == 4)
                    {
                        RemoveEmployee();
                        MainMenu();
                    }
                    else if (userInput == 5)
                    {
                        ListUpcomingAnniversaries();
                        MainMenu();
                    }
                    else if (userInput == 6)
                    {
                        ListAverageAge(Employees);
                        MainMenu();
                    }
                    else if (userInput == 7)
                    {
                        ListByTown(Employees);
                        MainMenu();
                    }
                    else if (userInput == 8)
                    {
                        Console.WriteLine("Goodbye");
                        break;
                    }
                    else throw new ArgumentNullException();
                }
                catch (ArgumentNullException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input, enter a number 1-8.");
                    Console.ResetColor();
                    MainMenu();
                }
            }
        }

        public static void ListEmployees()
        {
            var employeeData = ProcessCsv(DBPath);
            foreach (var employee in employeeData)
            {
                Console.WriteLine($"\r\nId: {employee.EmployeeId}" +
                    $"\r\nName: {employee.FirstName} {employee.LastName} " +
                    $"\r\nDate of birth: {employee.DateOfBirth.ToShortDateString()}" +
                    $"\r\nEmployment start date: {employee.StartDate.Date.ToShortDateString()}" +
                    $"\r\nHome town: {employee.HomeTown}" +
                    $"\r\nDepartment: {employee.Department}");
            }
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

        private static string GetUserInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        public static Employee ManualAdd()
        {
            {
                var employeeIdInput = GetUserInput("Enter employee id");
                Int32.TryParse(employeeIdInput, out int employeeId);
                var firstName = GetUserInput("Enter First Name");
                var lastName = GetUserInput("Enter Last Name");
                var dateofBirthInput = GetUserInput("Enter date of birth. (DD/MM/YYYY)");
                DateTime.TryParse(dateofBirthInput, out DateTime dateofBirth);
                var startDateInput = GetUserInput("Enter start date. (DD/MM/YYYY)");
                DateTime.TryParse(startDateInput, out DateTime startDate);
                var homeTown = GetUserInput("Enter home town");
                var department = GetUserInput("Enter department");

                return new Employee(employeeId, firstName, lastName, dateofBirth, startDate, homeTown, department);
            }
        }

        private static void AddEmployeeToCSV(Employee employee)
        {
            var row = $"{Environment.NewLine}{employee.EmployeeId},{employee.FirstName}," +
                        $"{employee.LastName},{employee.DateOfBirth},{employee.StartDate},{employee.HomeTown},{employee.Department}";
            File.AppendAllText(DBPath, row);
        }

        private static void RemoveEmployee()
        {
            Console.WriteLine("Enter the Id of the Employee you wish to remove.");
            var input = Console.ReadLine();
            Int32.TryParse(input, out int Id);
            var x = Employees.FirstOrDefault(e => e.EmployeeId == Id);

            RemoveEmployeeFromCSV(x);
            Console.WriteLine($"Employee {Id} has been deleted from the file.");
        }

        private static void RemoveEmployeeFromCSV(Employee employee)
        {
            var rowToDelete = $"{Environment.NewLine}{employee.EmployeeId},{employee.FirstName}," +
                                $"{employee.LastName},{employee.DateOfBirth},{employee.StartDate},{employee.HomeTown},{employee.Department}";
            var file = File.ReadAllText(DBPath);
            file = file.Replace(rowToDelete, "");
            File.WriteAllText(DBPath, file);
        }

        private static void EditEmployee()
        {
            Console.WriteLine("Enter the Id of the Employee whose details you wish to edit.");
            var input = Console.ReadLine();
            Int32.TryParse(input, out int Id);
            var x = Employees.FirstOrDefault(e => e.EmployeeId == Id);

            UpdateEmployee(x);
            Console.WriteLine($"Employee {Id} has been edited.");
        }

        public static void UpdateEmployee(Employee employee)
        {
            var dateofBirthInput = GetUserInput("Enter date of birth. (DD/MM/YYYY)");
            DateTime.TryParse(dateofBirthInput, out DateTime dateofBirth);

            var NewDOB = $"{dateofBirth}";

            var OldDOB = $"{employee.DateOfBirth}";
            var file = File.ReadAllText(DBPath);
            file = file.Replace(OldDOB, NewDOB);

            File.WriteAllText(DBPath, file);
        }







        public static void ListUpcomingAnniversaries()
        {
            Console.WriteLine("These employees have their anniversary in the next month.");
            Console.ReadLine();
        }


        public static void ListAverageAge(List<Employee> averageAge)
        {
            Console.WriteLine("These are the average ages of the employees in each department");

            Console.ReadKey();
        }

        public static void ListByTown(List<Employee> homeList)
        {
            var homeTownList = homeList.GroupBy(e => e.HomeTown)
                .OrderBy(e => e.Key)
                .Select(e => new
                {
                    HomeTown = e.Key,
                    numberOfEmployees = e.Count()
                });

            foreach (var employee in homeTownList)
            {
                Console.WriteLine($"The number of employees whose home town is {employee.HomeTown} are {employee.numberOfEmployees}.");
            }
            Console.ReadKey();
        }
    }
}

