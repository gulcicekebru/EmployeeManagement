using EmployeeManager.Components;
using EmployeeManager.Data;
using EmployeeManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Services
{
    public class EmployeeService
    {
        private readonly AppDbContext _dbContext;

        public EmployeeService(AppDbContext context){
            _dbContext = context;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Name))
            {
                throw new ArgumentException("Name cannot be empty!");
            }

            if (string.IsNullOrWhiteSpace(employee.Department))
            {
                throw new ArgumentException("Department cannot be empty!");
            }
            if(employee.HireDate == DateTime.MinValue)
            {
                throw new ArgumentException("Invalid date");
            }

            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
            
        }

        public async Task<int> GetEmployeeCountAsync()
        {
            return await _dbContext.Employees.CountAsync();
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
           return await _dbContext.Employees
                .AsNoTracking()
                .ToListAsync();

        }

    }
}
