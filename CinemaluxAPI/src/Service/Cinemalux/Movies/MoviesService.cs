using System.Net;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.CinemaluxCatalogue;
using CinemaluxAPI.src.Services.Movies.DTO;
using CinemaluxAPI.DAL.OrganizationDbContext;

namespace CinemaluxAPI.Services.Movies
{
    public class MoviesService
    {

        #region Properties

        private CinemaluxDbContext DbContext { get; }
        private OrganizationDbContext OrganizationDbContext { get; }

        #endregion

        #region Constructor

        public MoviesService(CinemaluxDbContext dbContext, OrganizationDbContext organizationContext)
        {
            DbContext = dbContext;
            OrganizationDbContext = organizationContext;
        }

        #endregion

        #region Action Methods


        
        

        #endregion
    }
}
