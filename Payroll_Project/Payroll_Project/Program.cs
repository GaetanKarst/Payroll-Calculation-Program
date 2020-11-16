using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Linq;

namespace Payroll_Project
{
    class Program
    {
      static void Main(string[] args)
      {
            List<Staff> myStaff = new List<Staff>();
            FileReader fr = new FileReader();
            int _month = 0;
            int _year = 0;

            while (_year == 0)
            {
                Console.WriteLine("\n Please input the month and the year: ");

                try
                {
                    _year = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message + "Please try again");
                }
            }

            while (_month == 0)
            {
                try
                {
                    _month = Convert.ToInt32(Console.ReadLine());

                    if (_month < 1 && _month > 12)
                    {
                        Console.WriteLine("The Month should be between 1 and 12");
                        _month = 0;
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message + "Please try again");
                }
            }

            myStaff = fr.ReadFile();

            for (int i = 0; i < myStaff.Count; i++)
            {
                try
                {
                    Console.WriteLine("Input the hours worked for {0}", myStaff[i].NameOfStaff);
                    myStaff[i].HoursWorked = Convert.ToInt32(Console.ReadLine());
                    myStaff[i].CalculatePay();
                    Console.WriteLine(myStaff[i].ToString());
                }
                catch (Exception e)
                {
                    i--;
                    Console.WriteLine(e.Message + "Turns out an error has occured!");
                }
            }

            Payslip ps = new Payslip(_month, _year);
            ps.GeneratePaySlip(myStaff);
            ps.GenerateSummary(myStaff);
            Console.Read();
      }
    }

    class Staff
    {
        private float _hourlyRate;
        private int _hWorked;

        public float TotalPay { get; protected set; }
        public float BasicPay { get; private set; }
        public string NameOfStaff { get; private set; }
        public int HoursWorked
        {
            get
            {
                return _hWorked;
            }
            set
            {
                if(value > 0)
                {
                    _hWorked = value;
                }
                else
                {
                    _hWorked = 0;
                }
            }
        }

        public Staff(string name, float rate)
        {
            NameOfStaff = name;
            _hourlyRate = rate;
        }

        public virtual void CalculatePay()
        {
            BasicPay = _hWorked * _hourlyRate;
            TotalPay = BasicPay;
        }

        public override string ToString()
        {
            return "/nName of the Staff: " + "Hourly Rate = " + _hourlyRate +
                NameOfStaff + "/nHours Worked: " +
                HoursWorked + "/nTotalPay: " + TotalPay;
        }
    }

    class Manager : Staff
    {
        private const float _managerHourlyRate = 50f;

        public int Allowance { get; private set; }

        public Manager(string name) : base(name, _managerHourlyRate)
        {

        }

        public override void CalculatePay()
        {
            base.CalculatePay();
            Allowance = 0;

            if (HoursWorked > 160f)
            {
                Allowance = 1000;
                TotalPay = BasicPay + Allowance;
            }
        }

        public override string ToString()
        {
            return "\nName of Staff = " + NameOfStaff + "\nManager Hourly Rate = " + _managerHourlyRate +
                "\nHours Worked = " + HoursWorked + "\nBasic Pay = " + BasicPay +
                "\nAllowance = " + Allowance + "\nTotal Pay = " + TotalPay;
        }
    }

    class Admin : Staff
    {
        private const float _overTimeRate = 15.5f;
        private const float _adminHourlyRate = 30f;

        public float Overtime { get; private set; }

        public Admin(string name) : base(name, _adminHourlyRate)
        {

        }

        public override void CalculatePay()
        {
            base.CalculatePay();

            if (HoursWorked > 160f)
            {
                Overtime = _overTimeRate * (HoursWorked - 160);
                TotalPay = BasicPay + Overtime;
            }
        }

        public override string ToString()
        {
            return "\nName of Staff = " + NameOfStaff + "\nAdmin Hourly Rate = " + _adminHourlyRate +
                "\nHours Worked = " + HoursWorked + "\nBasic Pay = " + BasicPay +
                "\nOvertime = " + Overtime + "\nTotal Pay = " + TotalPay;
        }
    }

    class FileReader
    {
        public List<Staff> ReadFile()
        {
            List<Staff> myStaff = new List<Staff>();
            string[] result = new string[2];
            string path = "staff.txt";
            string[] seperator = { ", " };

            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        result = sr.ReadLine().Split(seperator, StringSplitOptions.RemoveEmptyEntries);

                        if (result[1] == "Manager")
                        {
                            myStaff.Add(new Manager(result[0]));
                        }

                        else if (result[1] == "Admin")
                        {
                            myStaff.Add(new Admin(result[0]));
                        }
                    }
                    sr.Close();
                }
            }

            else
            {
                Console.WriteLine("Path has not been found");
            }

            return myStaff;
        }
    }

    class Payslip
    {
        private int _month;
        private int _year;

        enum MonthOfTheYear
        {
            January = 1, February, March,
            April, May, June,
            July, August, September,
            October, November, December
        }

        public Payslip(int payMonth, int payYear)
        {
            _month = payMonth;
            _year = payYear;
        }

        public void GeneratePaySlip(List<Staff> myStaff)
        {
            string path;

            foreach (var staff in myStaff)
            {
               path = staff.NameOfStaff + ".txt";

                using (StreamWriter sw = new StreamWriter(path))
                {
                   sw.WriteLine("PaySlip for {0} {1}", (MonthOfTheYear)_month, _year);
                    sw.WriteLine("=========================================");
                    sw.WriteLine("Name of the Staff: {0}", staff.NameOfStaff);
                    sw.WriteLine("Hours Worked: {0} ", staff.HoursWorked);
                    sw.WriteLine("");//Put a space
                    sw.WriteLine("Basic Pay: {0:C} ", staff.BasicPay);

                    if (staff.GetType() == typeof(Manager))
                    {
                        sw.WriteLine("Allowance: {0:C} ", ((Manager)staff).Allowance);
                    }

                    else if (staff.GetType() == typeof(Admin))
                    {
                        sw.WriteLine("Overtime Pay: {0:C}", ((Admin)staff).Overtime);
                    }

                    sw.WriteLine("");
                    sw.WriteLine("=========================================");
                    sw.WriteLine("Total Pay: {0:C}", staff.TotalPay);
                    sw.WriteLine("=========================================");

                    sw.Close();
                }
            }
        }

        public void GenerateSummary(List<Staff> myStaff)
        {
            //Search staff members who worked less than 10 hours 
            var result = from staff in myStaff
                         where (staff.HoursWorked < 10f)
                         orderby staff.NameOfStaff ascending
                         select new { staff.NameOfStaff, staff.HoursWorked };

            string path = "Summary.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("Staff whith less than 10 working hours: ");
                sw.WriteLine("");

                foreach (var staff in result)
                { 
            
                    sw.WriteLine("Name Of Staff: {0}, Hours Worked: {1}",
                        staff.NameOfStaff, staff.HoursWorked);

                    sw.Close();
                }
            }
        }

        public override string ToString()
        {
            return "Date: " + _month + _year;
        }
    }
}
