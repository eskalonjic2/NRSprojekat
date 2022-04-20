using System.Net;
using CinemaluxAPI.Common;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
using CinemaluxAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaluxAPI.API.Controllers
{
    [ApiController]
    [Route("identities")]
    public class IdentitiesController : CinemaluxControllerBase
    {
        #region Properties
        private IEmployeeService EmployeeService { get; }

        #endregion
        
        #region Constructor
        
        public IdentitiesController(IEmployeeService employeeService)
        {
            EmployeeService = employeeService;
        } 

        #endregion
        
        #region Routes

        [HttpPost("registration")]
        public CreatedResult EmployeeRegistration([FromBody] AddEmployeeDTO dto)
        {
            return Created("Uspjesno kreiran", EmployeeService.AddEmployee(dto));
        }

        [HttpPut("update/{id}")]
        public ActionResult EmployeeUpdate([FromRoute] int id, [FromBody] Employee dto)
        {
            if (dto.Id != CurrentIdentity.Id)
                throw new HttpResponseException(HttpStatusCode.Forbidden, "Token ID and ID don't match");
            
            return Ok(EmployeeService.UpdateEmployee(id, dto));
        }
        
        [HttpPost("delete/{id}")]
        public ActionResult EmployeeDelete([FromRoute] int id)
        {
            return Ok(EmployeeService.DeleteEmployee(id));
        }

        #endregion
    }
}