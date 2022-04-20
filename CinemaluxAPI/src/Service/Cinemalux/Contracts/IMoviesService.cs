using CinemaluxAPI.src.Services.Movies.DTO;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.Services
{
    public interface IMoviesService
    {
        public CinemaluxMovie GetMovie(short movieId);
        public CinemaluxMovie[] GetAllMovies(AllMoviesQuery query);
        public CinemaluxMovie AddMovie(MovieDTO dto);
        public CinemaluxMovie UpdateMovie(short movieId, MovieDTO dto);
        public CinemaluxMovie ArchiveMovie(short movieId);
        public CinemaluxMovie DeleteMovie(short movieId);
    }
}
