using System.Threading.Tasks;
using CinemaluxAPI.Common;
using CinemaluxAPI.DAL.OrganizationDbContext.Models;
using CinemaluxAPI.Service.Web.Movies.DTO;

namespace CinemaluxAPI.Service.Web.Contracts
{
    public interface IOrganizationMoviesService
    {
        public MovieDropdownDTO[] GetDropdownValues(GridParams gridParams);
        public OrganizationMovieOverviewDTO[] GetAllMovies();
        public GridData<OrganizationVerboseMoviesDTO> GetAllVerboseMovies(GridParams gridParams);
        public OrganizationMovieInfoDTO GetMovie(short movieId);
        public GridData<OrganizationMovieInfoDTO> GetMoviesScreenings(GridParams gridParams);
        public OrganizationMovieOverviewDTO[] GetLatestMovies();
        public OrganizationMovieOverviewDTO[] GetBestRatedMovies();
        public OrganizationMovieOverviewDTO[] GetMoviesWithSimilarGenres(int currentMovieId, SimilarMoviesDTO dto);
        public Task<Movie> AddMovie(AddMovieDTO dto);
        public bool ArchiveMovie(short movieId);
    }
}