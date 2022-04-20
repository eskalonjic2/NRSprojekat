using CinemaluxAPI.Auth.DTO;
using CinemaluxAPI.Common.Enumerations;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.Auth
{
    public interface IAuthorizationService
    {
        public string GenerateJWT(Identity identity);
        public Identity ValidateToken(string token, Role role);
    }
}