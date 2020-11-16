# Payroll-Calculation-Program

Description:

This simple program allows the user to generate automatically the payroll of each employee. The user first provides a text file containing the name and the position in the company of each employee. Then the program will tell the user to input the work time for each employee, so that the calculation of the due salary can be made based on hourly rates that are different depending on the position of the employees. Overtimes, admin rates, and special allowances for managers, are taken into account in the calculation of the salary. Employees who worked less than 10 hours in the month also appear on a separate file.

How it works:

Asks the user for the following inputs:
1. Month and Year of the payroll (error massage handled in case of incorrect input format)
2. The hours each employee worked during the month
3. Give a brief summary in the console, so that the user can have a first overview of the final payroll
4. Once the user has input worked hours for each employee, a separate payslip (.txt file) for each employee is generated in the bin folder.

The program in detail:

- Features a Staff class on which inherits the Manager and the Admin classes.
- A FileReader class that, as indicated in the name, will take care of the main files streams.
- A PaySlip class that will handle the payslip's layout, the generation of the payslips in separate text files, and the generation of the summary.
