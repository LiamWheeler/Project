using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaleidaProject
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime StartDate { get; set; }
        public string HomeTown { get; set; }
        public string Department { get; set; }
        public int Age { get
            {
                int age = DateTime.Now.Year - DateOfBirth.Year;
                if (DateOfBirth.DayOfYear > DateTime.Now.DayOfYear)
                {
                        age = age - 1;
                }           
                    return age;
            }
        }

        public Employee()
        {
        }

        public Employee(int employeeId, string firstName, string lastName, DateTime dateofBirth, DateTime startDate, string homeTown, string department)
        {
            this.EmployeeId = employeeId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateofBirth;
            this.StartDate = startDate;
            this.HomeTown = homeTown;
            this.Department = department;
        }

        internal static Employee ParseFromCsv (string data)
        {
            var columns = data.Split(',');

            return new Employee()
            {
                EmployeeId = int.Parse(columns[0]),
                FirstName = columns[1],
                LastName = columns[2],
                DateOfBirth = DateTime.Parse(columns[3]),
                StartDate = DateTime.Parse(columns[4]),
                HomeTown = columns[5],
                Department = columns[6]
            };

        }
    }
}
