using CinemaluxAPI.Common;
using CinemaluxAPI.Services;
using CinemaluxAPI.src.Services.Movies.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CinemaluxAPI.API.Controllers
{
    [ApiController]
    [Route("movies")]
    public class MoviesController: CinemaluxControllerBase
    {
       
        #region Properties

        private IMoviesService MoviesService { get; }

        #endregion

        #region Constructor

        public MoviesController(IMoviesService moviesService)
        {
            MoviesService = moviesService;
        }

        #endregion

        #region Routes

        [HttpGet("{movieId}")]
        public ActionResult GetMovie([FromRoute] short movieId)
        {
            return Ok(MoviesService.GetMovie(movieId));
        }
        
        [HttpGet("all")]
        public ActionResult GetAllMovies([FromQuery] AllMoviesQuery query)
        {
            return Ok(MoviesService.GetAllMovies(query));
        }

        [HttpPost("add")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult AddMovie([FromBody] MovieDTO dto)
        {
            return Ok(MoviesService.AddMovie(dto));
        }
        
        [HttpPut("update/{movieId}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult UpdateMovie([FromRoute] short movieId, [FromBody] MovieDTO dto)
        {
            return Ok(MoviesService.UpdateMovie(movieId, dto));
        }
        
        [HttpPost("archive/{movieId}")]
        public ActionResult ArchiveMovie([FromRoute] short movieId)
        {
            return Ok(MoviesService.ArchiveMovie(movieId));
        }
        
        [HttpDelete("delete/{movieId}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult DeleteMovie([FromRoute] short movieId)
        {
            return Ok(MoviesService.DeleteMovie(movieId));
        }
        
        #endregion
    }
}
