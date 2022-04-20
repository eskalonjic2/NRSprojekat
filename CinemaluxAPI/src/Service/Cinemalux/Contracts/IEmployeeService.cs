using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.Services
{
    public interface IEmployeeService
    {
        public SuccessfulLoginDTO LoginEmployee(LoginDTO Login);
        public bool IsEmailTaken(string Email);
        public bool IsUserNameTaken(string Username);
        public Employee AddEmployee(AddEmployeeDTO addEmployeeDto);
        public Employee UpdateEmployee(int id,  Employee dto);
        public bool DeleteEmployee(int id);
    }
}