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
        private static string DBPath = ConfigurationManager.AppSettings["CsvDatabasePath"];
        private static List<Employee> Employees = new List<Employee>();

        static void Main(string[] args)
        {
            MainMenu();
            Console.ReadKey();
        }

        private static List<Employee> ProcessCsv(string path)
        {
            try {
                
                    return File.ReadAllLines(path)
                    .Skip(1)
                    .Select(Employee.ParseFromCsv)
                    .ToList();
                
                }catch(Exception ex){
                    return new List<Employee>();
                }
        }

        private static void MainMenu()
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

            var userInput = Convert.ToInt32(Console.ReadLine().Trim());

            if (userInput == 1)
            {
                ListEmployees();
            }
            else if (userInput == 2)
            {
                var employee = ManualAdd();
                AddEmployeeToCSV(employee);                
            }
            else if (userInput == 3)
            {
                EditEmployee();
                MainMenu();
            }
            else if (userInput == 4)
            {
                RemoveEmployee();

            }
            else if (userInput == 5)
            {
                ListUpcomingAnniversaries();

            }
            else if (userInput == 6)
            {
                ListAverageAge();

            }
            else if (userInput == 7)
            {
                ListByTown();

            }
            else if (userInput == 8)
            {
                Console.WriteLine("Goodbye");
            }
            else
            {
                Console.WriteLine("Invalid input, enter a number 1-8.");
                Console.ReadLine();
               
            }
            MainMenu();
        }

        public static void ListEmployees()
        {
            var employeeData = ProcessCsv(DBPath);
            foreach (var employee in employeeData)
            {
                Console.WriteLine($"\r\nName: {employee.FirstName} {employee.LastName} " +
                    $"\r\nDate of birth: {employee.DateOfBirth.ToShortDateString()}" +
                    $"\r\nEmployment start date: {employee.StartDate.Date.ToShortDateString()}" +
                    $"\r\nHome town: {employee.HomeTown}" +
                    $"\r\nDepartment: {employee.Department}");
            }
        }

        private static string GetUserInput(string message){
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        public static Employee ManualAdd() 
        {
            {
                var firstName = GetUserInput("Enter First Name");

                Console.WriteLine("Enter Last Name");
                var lastName = Console.ReadLine();

                Console.WriteLine("Enter date of birth (DD/MM/YYYY)");
                var dateofBirth = Convert.ToDateTime(Console.ReadLine());

                Console.WriteLine("Enter start date (DD/MM/YYYY)");
                var startDate = Convert.ToDateTime(Console.ReadLine());

                Console.WriteLine("Enter home town");
                var homeTown = Console.ReadLine();

                Console.WriteLine("Enter department");
                var department = Console.ReadLine();

                return new Employee(firstName, lastName, dateofBirth, startDate, homeTown, department); 
            }
        }

        private static void AddEmployeeToCSV(Employee employee)
        {
            var row = $"{Environment.NewLine}{employee.FirstName},{employee.LastName},{employee.DateOfBirth},{employee.StartDate},{employee.HomeTown},{employee.Department}";
            File.AppendAllText(DBPath, row);    
        }

        private static void RemoveEmployee()
        {
            Console.WriteLine("Enter the name of the Employee you wish to remove.");
            var name = Console.ReadLine();
            var x = Employees.FirstOrDefault(e => e.FirstName == name);

            RemoveEmployeeFromCSV(x);
        }

        private static void RemoveEmployeeFromCSV(Employee employee)
        {
            var rowToDelete = $"{Environment.NewLine}{employee.FirstName},{employee.LastName},{employee.DateOfBirth},{employee.StartDate},{employee.HomeTown},{employee.Department}";
            var file = File.ReadAllText(DBPath);   
            file = file.Replace(rowToDelete, "");
            File.WriteAllText(DBPath, file);
        }

        private static void EditEmployee()
        {
            Console.WriteLine("Enter the name of the employee whose details you wish to edit.");
            Console.ReadLine();
        }

        public static void UpdateEmployee(Employee old) {

        }

        public static void ListUpcomingAnniversaries()
        {
            Console.WriteLine("These employees have their anniversary in the next month.");
            Console.ReadLine();
        }

        public static void ListAverageAge()
        {
            Console.WriteLine("These are the average ages of the employees in each department");
            Console.ReadLine();
        }

        public static void ListByTown()
        {
            Console.WriteLine("Here are the employees arranged by home town.");
            Console.ReadKey();
        }
    }
}


