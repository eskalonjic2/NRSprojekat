using System.Net;
using CinemaluxAPI.Common;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.Service.Web.Contracts;
using CinemaluxAPI.Service.Web.DTO;
using CinemaluxAPI.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace CinemaluxAPI.API.Controllers.Web
{
    [ApiController]
    [Route("users")]
    public class UserController : CinemaluxControllerBase
    {
        #region Properties
        private IUserService UserService { get; set; }

        #endregion

        #region Constructor
        
        public UserController(IUserService _userService)
        { 
            UserService = _userService;
        }
        
        #endregion
        
        #region Routes

        [HttpGet("all")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult<GridData<UserResponseDTO>> GetUsers([FromQuery] UserQueryParams queryParams)
        {
            return Ok(UserService.GetUsers(queryParams));
        }
        
        [HttpGet("reviews")]
        [Authority(Roles = "User")]
        public ActionResult<GridData<UserMovieReviewsDTO>> GetReviews([FromQuery] UserQueryParams queryParams)
        {
            return Ok(UserService.GetReviews(CurrentIdentity.Id, queryParams));
        }
        
        [HttpPost("")]
        public CreatedResult Create([FromBody] CreateUserDTO dto)
        {
            if(!dto.Password.Equals(dto.PasswordRepeat))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "New password does not match confirmation password");
            
            return Created("Uspjesno kreiran", UserService.Create(dto));
        }
        
        [HttpPut("")]
        [Authority(Roles = "User")]
        public ActionResult<SuccessfulLoginDTO> UpdateGeneralInformation([FromBody] UpdateUserDTO dto)
        {
            return Ok(UserService.UpdateGeneralInformation(CurrentIdentity.Id, dto));
        }
        
        [HttpPut("updatePassword")]
        [Authority(Roles = "User")]
        public ActionResult<string> UpdatePassword([FromBody] UpdatePasswordDTO dto)
        {
            if(dto.NewPassword.Equals(dto.NewPasswordRepeat))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "New password does not match confirmation password");
            
            return Ok(UserService.UpdatePassword(CurrentIdentity.Id, dto));
        }
        
        [HttpPut("lock/{id}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult<bool> Lock([FromRoute] int id)
        {
            return Ok(UserService.Lock(id));
        }
        
        [HttpPut("unlock/{id}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult<bool> Unlock([FromRoute] int id)
        {
            return Ok(UserService.Unlock(id));
        }
        
        [HttpDelete("archive/{id}")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult<bool> Archive([FromRoute] int id)
        {
            return Ok(UserService.Archive(id));
        }
        
        [HttpDelete("{id}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult<bool> Delete([FromRoute] int id)
        {
            return Ok(UserService.Delete(id));
        }
        
        #endregion
    }
}