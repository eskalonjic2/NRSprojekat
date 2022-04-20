using System;
using System.Net;
using CinemaluxAPI.Common.Enumerations;
using CinemaluxAPI.Services;
using Microsoft.AspNetCore.Mvc;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.Service.Web;
using CinemaluxAPI.Service.Web.Contracts;
using CinemaluxAPI.Service.Web.DTO;

namespace CinemaluxAPI.Auth.Controller
{
    [Route("auth")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        #region Properties
        
        private IEmployeeService EmployeeService { get; }
        private IUserService UserService { get; }
        private IAuthorizationService AuthorizationService { get; }

        #endregion
        
        #region Constructor

        public AuthorizationController(IEmployeeService employeeService, IUserService _userService, IAuthorizationService authorizationService)
        {
            EmployeeService = employeeService;
            UserService = _userService;
            AuthorizationService = authorizationService;
        } 

        #endregion
        
        #region Action Methods
        
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDTO dto)
        {
            try
            {
                var employee = EmployeeService.LoginEmployee(dto);
                return Ok(employee);
            }
            catch (HttpResponseException ex)
            {
                var user = UserService.Login(dto);
                return Ok(user);
            }
        }

        [HttpPost("isTokenValid/{token}")]
        public ActionResult IsUserTokenValid([FromRoute] string token)
        {
            return Ok(AuthorizationService.ValidateToken(token, Role.Employee));
        }
        
        [HttpPost("isEmployeeEmailTaken/{email}")]
        public ActionResult IsEmployeeEmailTaken([FromRoute] string email)
        {
            return Ok(EmployeeService.IsEmailTaken(email));
        }

        [HttpPost("web/isUsernameTaken/{username}")]
        public ActionResult IsUserUserNameTaken([FromRoute] string username)
        {
            return Ok(UserService.IsUserNameTaken(username));
        }
        
        [HttpPost("isUsernameTaken/{username}")]
        public ActionResult IsEmployeeUserNameTaken([FromRoute] string username)
        {
            return Ok(EmployeeService.IsUserNameTaken(username));
        }
        
        [HttpPost("signup")]
        public CreatedResult UserRegistration([FromBody] CreateUserDTO dto)
        {
            return Created("Uspjesno kreiran", UserService.Create(dto));
        }

        [HttpPost("signupEmp")]
        public CreatedResult EmployeeRegistration([FromBody] AddEmployeeDTO addEmployeeDto)
        {
            return Created("Uspjesno kreiran", EmployeeService.AddEmployee(addEmployeeDto));
        }
        
        #endregion
    }
}