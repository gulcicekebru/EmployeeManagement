using EmployeeManager.Data;
using EmployeeManager.Models;
using Microsoft.EntityFrameworkCore;


namespace EmployeeManager.Services
{
    public class EmployeeService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public EmployeeService(IDbContextFactory<AppDbContext> context)
        {
            _contextFactory = context;
        }


        public async Task AddEmployeeAsync(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            await using var context =
                await _contextFactory.CreateDbContextAsync();

            if (string.IsNullOrWhiteSpace(employee.Name))
            {
                throw new ArgumentException("Name cannot be empty!");
            }

            if (string.IsNullOrWhiteSpace(employee.Department))
            {
                throw new ArgumentException("Department cannot be empty!");
            }
            if (employee.HireDate == DateTime.MinValue)
            {
                throw new ArgumentException("Invalid date");
            }

            context.Employees.Add(employee);
            await context.SaveChangesAsync();

        }

        public async Task<int> GetEmployeeCountAsync()
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();
            return await context.Employees.CountAsync();
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();
            return await context.Employees
                .AsNoTracking()
                .ToListAsync();

        }

        public async Task DeleteEmployeeAsync(int id)
        {

            await using var context =
                await _contextFactory.CreateDbContextAsync();

            var employee = await context.Employees.FindAsync(id);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} was not found.");
            }

            context.Employees.Remove(employee);

            await context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(int id,string name, string department)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Name can not be empthy!");
            }
            if (string.IsNullOrWhiteSpace(department))
            {
                throw new ArgumentNullException("Department can not be empthy!");
            }

            await using var context =
                await _contextFactory.CreateDbContextAsync();

            var employee = await context.Employees.FindAsync(id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} was not found.");
            }
            employee.Name= name;
            employee.Department= department;

            await context.SaveChangesAsync();

        }

        public async Task<List<Employee>> SearchEmployeesAsync(string searchTerm)
        {
            searchTerm= searchTerm.Trim();
            await using var context =
                await _contextFactory.CreateDbContextAsync();
            return await context.Employees
                                .Where(e => e.Name.Contains(searchTerm) || e.Department.Contains(searchTerm))
                                .AsNoTracking()
                                .ToListAsync();
        }
    }
}
