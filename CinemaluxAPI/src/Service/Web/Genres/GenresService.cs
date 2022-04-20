using System.Linq;
using CinemaluxAPI.DAL.OrganizationDbContext;
using CinemaluxAPI.DAL.OrganizationDbContext.Models;
using CinemaluxAPI.Service.Web.Contracts;

namespace CinemaluxAPI.Service.Web.Genres
{
    public class GenresService : IGenresService
    {
        #region Properties
        
        private OrganizationDbContext OrganizationDbContext { get; }

        #endregion
    
        #region Constructor
        
        public GenresService(OrganizationDbContext organizationDbContext)
        {
            OrganizationDbContext = organizationDbContext;
        }
        
        #endregion
    
        #region Action Methods
        
        public string[] GetAll()
        {
            return OrganizationDbContext.Genres.Select(x => x.Code).ToArray();
        }
        
        public Genre[] GetAllGenres()
        {
            return OrganizationDbContext.Genres.ToArray();
        }
        
        #endregion
    }
}