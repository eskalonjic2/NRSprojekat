using CinemaluxAPI.Common;
using CinemaluxAPI.Service.Web.Contracts;
using CinemaluxAPI.Service.Web.MovieReviews.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CinemaluxAPI.API.Controllers.Web
{
    [ApiController]
    [Route(("/web/genres"))]
    public class GenresController : ControllerBase
    {
        #region Properties
        private IGenresService GenresService { get; set; }

        #endregion

        #region Constructor
        
        public GenresController(IGenresService genresService)
        {
            GenresService = genresService;
        }
        
        #endregion
        
        [HttpGet("all")]
        public ActionResult<string[]> GetAll()
        {
            return Ok(GenresService.GetAll());
        }
        
        [HttpGet("allGenres")]
        public ActionResult<string[]> GetAllGenres()
        {
            return Ok(GenresService.GetAllGenres());
        }
    }
}