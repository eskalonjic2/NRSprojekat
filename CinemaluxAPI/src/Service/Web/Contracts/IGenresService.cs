using CinemaluxAPI.DAL.OrganizationDbContext.Models;

namespace CinemaluxAPI.Service.Web.Contracts
{
    public interface IGenresService
    {
        public string[] GetAll();
        public Genre[] GetAllGenres();
    }
}