using System;
using System.Linq;
using System.Web;
using KaleidaProject;
using System.Configuration;

namespace EmployeeApi.Models
{
    public class EmployeeDto
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
            public float Age
            {
                get
                {
                    int age = DateTime.Now.Year - DateOfBirth.Year;
                    if (DateOfBirth.DayOfYear > DateTime.Now.DayOfYear)
                    {
                        age = age - 1;
                    }
                    return age;
                }
            }

            public bool Anniversary
            {
                get
                {
                    bool anniversary = false;
                    if (StartDate.DayOfYear <= DateTime.Now.DayOfYear + 28 && StartDate.DayOfYear >= DateTime.Now.DayOfYear)
                    {
                        anniversary = true;
                    }
                    return anniversary;
                }
            }

            public int DaysTillAnniversary
            {
                get
                {
                    var daysTillAnn = 0;
                    if (DateTime.Now.DayOfYear > StartDate.DayOfYear)
                    {
                        daysTillAnn = 365 - (DateTime.Now.DayOfYear - StartDate.DayOfYear);
                    }
                    else daysTillAnn = StartDate.DayOfYear - DateTime.Now.DayOfYear;
                    return daysTillAnn;
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

            public static Employee ParseData(string data)
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
}
