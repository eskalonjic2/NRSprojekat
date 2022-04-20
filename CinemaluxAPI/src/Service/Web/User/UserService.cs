using System;
using System.Linq;
using System.Net;
using CinemaluxAPI.Auth;
using CinemaluxAPI.Common;
using CinemaluxAPI.Services;
using CinemaluxAPI.Service.Web.DTO;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.Service.Web.Contracts;
using CinemaluxAPI.DAL.CinemaluxCatalogue;
using CinemaluxAPI.DAL.OrganizationDbContext;
using CinemaluxAPI.DAL.OrganizationDbContext.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaluxAPI.Service.Web
{
    public class UserService : IUserService
    {
        #region Properties
        
        private CinemaluxDbContext DbContext { get; }
        private OrganizationDbContext OrganizationDbContext { get; }
        private IAuthorizationService AuthorizationService { get;  }

        #endregion
    
        #region Constructor
        
        public UserService(CinemaluxDbContext dbContext, OrganizationDbContext organizationDbContext, IAuthorizationService authorizationService)
        {
            DbContext = dbContext;
            OrganizationDbContext = organizationDbContext;
            AuthorizationService = authorizationService;
        }
        
        #endregion
    
        #region Action Methods

        public  GridData<UserResponseDTO> GetUsers(UserQueryParams queryParams)
        {
            IQueryable<User> scope = OrganizationDbContext.Users.IgnoreQueryFilters();

            if (queryParams.NameQuery != null)
                scope = scope.Where(x => x.Name.Contains(queryParams.NameQuery) || x.Username.Contains(queryParams.NameQuery));

            IQueryable<UserResponseDTO> rows = 
                from user in scope
                select new UserResponseDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Username = user.Username,
                    Email = user.Email,
                    IsLocked = user.IsLocked,
                    IsArchived = user.ArchivedAt != null,
                    IsBanned = user.ArchivedAt != null,
                    TotalReviews = user.MovieReviews.Count(),
                    TotalReservations = GetReservationCount(DbContext, user.Id),
                    CreatedAt = user.CreatedAt
                };
            
            if (queryParams.SQ.IsNotNull())
                rows = rows.Where(x => x.Name.ToLower().Contains(queryParams.SQ) 
                                       || x.Surname.ToLower().Contains(queryParams.SQ)
                                       || x.Username.ToLower().Contains(queryParams.SQ)
                                       || x.Email.ToLower().Contains(queryParams.SQ));
            
            return new GridData<UserResponseDTO>(rows, queryParams);
        }
        
        public SuccessfulLoginDTO Login(LoginDTO dto)
        {
            User user = OrganizationDbContext.Users.IgnoreQueryFilters().FirstOrDefault(x => x.Username.Equals(dto.Credential) || x.Email.Equals(dto.Credential));
            user.EnsureNotNull("Korisnik sa tim kredencijalima ne postoji");
            
            if(user.IsLocked)
                throw new HttpResponseException(HttpStatusCode.Unauthorized, "Korisnik je banovan");
            
            if (BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return new SuccessfulLoginDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Username = user.Username,
                    Email = user.Email,
                    ContactPhone = user.ContactPhone,
                    Role = 0,
                    Token = AuthorizationService.GenerateJWT(user)
                };
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized, "Password nije tacan");
        }
     
        public bool IsEmailTaken(string email)
        {
            return OrganizationDbContext.Users.Any(x => x.Email == email);
        }
      
        public bool IsUserNameTaken(string username)
        {
            return OrganizationDbContext.Users.Any(x => x.Username == username);
        }
        
        public User Create(CreateUserDTO dto)
        {
            if (IsEmailTaken(dto.Email))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Email vec postoji");
            else if(IsUserNameTaken(dto.Username))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Username vec postoji");
                
            var user = new User
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Username = dto.Username,
                Email = dto.Email,
                ContactPhone = dto.ContactPhone,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            };
                
            OrganizationDbContext.Users.Add(user);
            OrganizationDbContext.SaveChanges();

            return user;
        }
        
        public SuccessfulLoginDTO UpdateGeneralInformation(int id, UpdateUserDTO dto)
        {
            var user = OrganizationDbContext.Users.FirstOrDefault(x => x.Id == id);
            user.EnsureNotNull("User ne postoji");

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Username = dto.Username;
            user.Email = dto.Email;
            user.ContactPhone = dto.ContactPhone;

            OrganizationDbContext.Update(user);
            OrganizationDbContext.SaveChanges();

            return new SuccessfulLoginDTO {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Username = user.Username,
                Email = user.Email,
                ContactPhone = user.ContactPhone,
                Token = AuthorizationService.GenerateJWT(user)
            };
        }

        public string UpdatePassword(int id, UpdatePasswordDTO dto)
        {
            var user = OrganizationDbContext.Users.FirstOrDefault(x => x.Id == id);
            user.EnsureNotNull("User ne postoji");
            
            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.Password))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Old password incorrect");

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            OrganizationDbContext.Update(user);
            OrganizationDbContext.SaveChanges();
            
            return AuthorizationService.GenerateJWT(user);
        }

        public GridData<UserMovieReviewsDTO> GetReviews(int id, GridParams gridParams)
        {
            var user = OrganizationDbContext.Users
                .Include(x => x.MovieReviews)
                .ThenInclude(x => x.Movie)
                .FirstOrDefault(x => x.Id == id);
            user.EnsureNotNull("User ne postoji");
     
            var rows = user
                .MovieReviews
                .Select(x => new UserMovieReviewsDTO
                {
                    Id = x.Id,
                    MovieId = x.MovieId,
                    MovieTitle = x.Movie.Title,
                    ImageURL = x.Movie.ImageUrl,
                    Title = x.Title,
                    Review = x.Review,
                    Rating = x.Rating,
                    CreatedAt = x.CreatedAt
                });
            
            return new GridData<UserMovieReviewsDTO>(rows.AsQueryable(), gridParams);
        }
       
        public bool Lock(int id)
        {
            User user = OrganizationDbContext.Users.IgnoreQueryFilters().FirstOrDefault(x => x.Id == id);
            user.EnsureNotNull("No user found with the given Id");

            user.IsLocked = true;
            
            OrganizationDbContext.Update(user);
            OrganizationDbContext.SaveChanges();
            
            return true;
        }
      
        public bool Unlock(int id)
        {
            User user = OrganizationDbContext.Users.IgnoreQueryFilters().FirstOrDefault(x => x.Id == id);
            user.EnsureNotNull("No user found with the given Id");
            
            user.IsLocked = false;
            
            OrganizationDbContext.Update(user);
            OrganizationDbContext.SaveChanges();
            
            return true;
        }
        
        public bool Archive(int id)
        {
            User user = OrganizationDbContext.Users.IgnoreQueryFilters().FirstOrDefault(x => x.Id == id);
            user.EnsureNotNull("No user found with the given Id");
            
            OrganizationDbContext.Archive(user, "SYSTEM");
            OrganizationDbContext.SaveChanges();
            
            return true;
        }
      
        public bool Delete(int id)
        {
            User user = OrganizationDbContext.Users.IgnoreQueryFilters().FirstOrDefault(x => x.Id == id);
            user.EnsureNotNull("No user found with the given Id");
            
            OrganizationDbContext.Remove(user);
            OrganizationDbContext.SaveChanges();
            
            return true;
        }
        
        #endregion
        
        #region Private Methods

        private static int GetReservationCount(CinemaluxDbContext dbContext, int userId)
        {
            return dbContext.Reservations.Count(x => x.CreatedBy.Equals(userId.ToString()));
        }
        
        #endregion
    }
    
}