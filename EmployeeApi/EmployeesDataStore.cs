﻿using System;
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
        public static EmployeeDataStore Current { get; } = new EmployeeDataStore();
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

        public void AddEmployee(Employee employee)
        {
            var row = $"{Environment.NewLine}{employee.EmployeeId},{employee.FirstName}," +
                        $"{employee.LastName},{employee.DateOfBirth.ToShortDateString()},{employee.StartDate.ToShortDateString()},{employee.HomeTown},{employee.Department}";
            File.AppendAllText(DBPath, row);
        }

        public void RemoveEmployeeFromData(Employee employee)
        {
            var rowToDelete = $"{Environment.NewLine}{employee.EmployeeId},{employee.FirstName}," +
                                $"{employee.LastName},{employee.DateOfBirth.ToShortDateString()},{employee.StartDate.ToShortDateString()},{employee.HomeTown},{employee.Department}";
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

            var NewDOB = $"{dateofBirth.ToShortDateString()}";

            var OldDOB = $"{employee.DateOfBirth.ToShortDateString()}";
            var file = File.ReadAllText(DBPath);
            file = file.Replace(OldDOB, NewDOB);

            File.WriteAllText(DBPath, file);
        }


    }
}