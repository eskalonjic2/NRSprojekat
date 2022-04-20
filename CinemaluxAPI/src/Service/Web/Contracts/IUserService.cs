using CinemaluxAPI.Common;
using CinemaluxAPI.DAL.OrganizationDbContext.Models;
using CinemaluxAPI.Service.Web.DTO;
using CinemaluxAPI.Services;

namespace CinemaluxAPI.Service.Web.Contracts
{
    public interface IUserService
    {
        public GridData<UserResponseDTO> GetUsers(UserQueryParams queryParams);
        public SuccessfulLoginDTO Login(LoginDTO dto);
        public bool IsEmailTaken(string email);
        public bool IsUserNameTaken(string username);
        public User Create(CreateUserDTO dto);
        public SuccessfulLoginDTO UpdateGeneralInformation(int id, UpdateUserDTO dto);
        public string UpdatePassword(int id, UpdatePasswordDTO dto);
        public GridData<UserMovieReviewsDTO> GetReviews(int id, GridParams gridParams);
        public bool Lock(int id);
        public bool Unlock(int id);
        public bool Archive(int id);
        public bool Delete(int id);
    }
}