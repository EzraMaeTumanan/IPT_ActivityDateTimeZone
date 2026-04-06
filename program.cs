using System;
using System.Collections.Generic;

namespace EmployeeTimeSystem
{
    // 1. ABSTRACTION: Abstract class for math logic
    public abstract class WorkCalculator
    {
        public abstract double CalculateHours(DateTime start, DateTime end);
        public abstract string GenerateWorkNote(double actualHours);
    }

    // 2. INHERITANCE: Office handles location and inherits math traits
    public class Office : WorkCalculator
    {
        // 3. ENCAPSULATION: Private field with public Property
        private string _location;
        public string Location { get => _location; private set => _location = value; }

        public Office(string location) => _location = location;

        public override double CalculateHours(DateTime start, DateTime end) 
            => (end - start).TotalHours;

        public override string GenerateWorkNote(double actualHours)
        {
            const double StandardHours = 9.0;
            if (Math.Abs(actualHours - StandardHours) < 0.1) return ""; // Exactly 9 hours
            
            if (actualHours < StandardHours)
            {
                double remaining = StandardHours - actualHours;
                return $"Early Out. Hours left: {remaining:F0} hours";
            }
            
            double extended = actualHours - StandardHours;
            return $"Overtime. Hours extended: {extended:F0} hours";
        }

        public DateTime GetLocalTime()
        {
            string zoneId = Location.ToLower() switch
            {
                "philippines" => "Singapore Standard Time",
                "india" => "India Standard Time",
                "united states" => "Eastern Standard Time",
                _ => "UTC"
            };
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(zoneId));
        }
    }

    class Program
    {
        // Dictionary name as per instruction
        static Dictionary<string, string> EmployeeTimeinTimeOutRecord = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            // INPUTS
            Console.Write("Enter Employee Number: ");
            string empNum = Console.ReadLine();

            Console.Write("Enter Employee Name: ");
            string empName = Console.ReadLine();

            Console.Write("Office Location (Philippines, United States, India): ");
            string locInput = Console.ReadLine();

            Office office = new Office(locInput);

            // LOGGING TIME IN
            DateTime timeIn = office.GetLocalTime();
            Console.WriteLine($"\nClocked IN at: {timeIn:hh:mm:ss tt}");

            // SIMULATION: Para ma-test natin, hihingi tayo ng manual Time Out 
            // Pero sa actual system, ito ay kukunin uli sa GetLocalTime()
            Console.WriteLine("\n--- Simulating Work Day ---");
            Console.Write("Enter how many hours you worked (e.g., 8 or 11): ");
            double hoursWorked = double.Parse(Console.ReadLine());
            DateTime timeOut = timeIn.AddHours(hoursWorked); 

            // CALCULATIONS
            double totalHours = office.CalculateHours(timeIn, timeOut);
            string note = office.GenerateWorkNote(totalHours);

            // SAVING TO DICTIONARY (Strictly following instructions)
            EmployeeTimeinTimeOutRecord["Employee Number"] = empNum;
            EmployeeTimeinTimeOutRecord["Employee Name"] = empName;
            EmployeeTimeinTimeOutRecord["Office Location"] = office.Location;
            EmployeeTimeinTimeOutRecord["Time In Date"] = timeIn.ToString("MM/dd/yyyy");
            EmployeeTimeinTimeOutRecord["Time In Time"] = timeIn.ToString("hh:mm:ss tt");
            EmployeeTimeinTimeOutRecord["Time Out Date"] = timeOut.ToString("MM/dd/yyyy");
            EmployeeTimeinTimeOutRecord["Time Out Time"] = timeOut.ToString("hh:mm:ss tt");
            EmployeeTimeinTimeOutRecord["Total Hours"] = totalHours.ToString("F2");
            EmployeeTimeinTimeOutRecord["Note"] = note;

            // FINAL LOG DISPLAY
            Console.WriteLine("\n================ LOG RECORD ================");
            foreach (var item in EmployeeTimeinTimeOutRecord)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }
    }
}
