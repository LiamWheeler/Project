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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime StartDate { get; set; }
        public string HomeTown { get; set; }
        public string Department { get; set; }


        public Employee()
        {
        }

        public Employee(string firstName, string lastName, DateTime dateofBirth, DateTime startDate, string homeTown, string department)
        {
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
                FirstName = columns[0],
                LastName = columns[1],
                DateOfBirth = DateTime.Parse(columns[2]),
                StartDate = DateTime.Parse(columns[3]),
                HomeTown = columns[4],
                Department = columns[5]
            };

        }
    }
}
