using System;
using System.Net;
using System.Linq;
using CinemaluxAPI.Auth;
using CinemaluxAPI.Common;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.Common.Enumerations;
using CinemaluxAPI.DAL.CinemaluxCatalogue;
using CinemaluxAPI.DAL.OrganizationDbContext;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.Services
{
    public class EmployeeService : IEmployeeService
    {

        #region Properties
        
        private CinemaluxDbContext DbContext { get; }
        private OrganizationDbContext OrganizationDbContext { get; }
        private IAuthorizationService AuthorizationService { get;  }
        
        #endregion

        #region Constructor

        public EmployeeService(CinemaluxDbContext dbContext, OrganizationDbContext organizationContext, IAuthorizationService _authorizationService)
        {
            DbContext = dbContext;
            OrganizationDbContext = organizationContext;
            AuthorizationService = _authorizationService;
        }
        
        #endregion

        #region Action Methods

        public SuccessfulLoginDTO LoginEmployee(LoginDTO dto)
        {
            Employee employee = DbContext.Employees.FirstOrDefault(x => x.Username == dto.Credential || x.Email == dto.Credential);
            employee.EnsureNotNull("Korisnik sa tim kredencijalima ne postoji");
       
            if (BCrypt.Net.BCrypt.Verify(dto.Password, employee.Password))
            {
                return new SuccessfulLoginDTO
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Surname = employee.Surname,
                    Username = employee.Username,
                    Email = employee.Email,
                    ContactPhone = employee.ContactPhone,
                    Role = employee.Role,
                    Token = AuthorizationService.GenerateJWT(employee)
                };
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized, "Password nije tacan");
        }
        public bool IsEmailTaken(string email)
        {
            return DbContext.Employees.FirstOrDefault(x => x.Email == email) != null;
        }
        public bool IsUserNameTaken(string username)
        {
            return DbContext.Employees.FirstOrDefault(x => x.Username == username) != null;
        }
        public Employee AddEmployee(AddEmployeeDTO dto)
        {
            if (IsEmailTaken(dto.Email))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Email zauzet");
            else if(IsUserNameTaken(dto.Username))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Username zauzet");

            if (dto.Role == Role.Administrator && dto.ManagerId != null)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Administrator ne moze imati menadžera");

            if ((dto.Salary == null || dto.Salary == 0) && dto.Role != Role.Volunteer)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Plata ne smije biti null/0 za nekog ko nije volonter.");
            
            if (dto.ManagerId != null)
            {
                Employee manager = DbContext.Employees.FirstOrDefault(x => x.Id == dto.ManagerId);
                manager.EnsureNotNull("Korisnik sa tim kredencijalima ne postoji");
            }
            
            var employee = new Employee
            {
                ManagerId = dto.ManagerId,
                Name = dto.Name,
                Surname = dto.Surname,
                Username = dto.Username,
                Email = dto.Email,
                BornAt = Utils.DateIdToDate(dto.BornAt),
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                ContactPhone = dto.ContactPhone,
                Address = dto.Address,
                Salary = dto.Salary,
                Key = dto.Key,
                Role = (byte) dto.Role
            };
                
            DbContext.Add(employee);
            DbContext.SaveChanges();

            return employee;
        }
        
        public Employee UpdateEmployee(int id, Employee dto)
        {
            var emp = DbContext.Employees.FirstOrDefault(x => x.Id == id);
            emp.EnsureNotNull("Employee ne postoji");

            DbContext.Employees.Update(dto);
            DbContext.SaveChanges();

            return emp;
        }

        public bool DeleteEmployee(int id)
        {
            Employee employee = DbContext.Employees.FirstOrDefault(x => x.Id == id);
            employee.EnsureNotNull("No employee found with the given Id");

            DbContext.Archive(employee);
            DbContext.SaveChanges();

            return true;
        }

        #endregion

        #region Private Methods

        public Employee GetEmployeeById(int id)
        {
            return DbContext.Employees.FirstOrDefault(x => x.Id == id);
        }

        #endregion
    }
}