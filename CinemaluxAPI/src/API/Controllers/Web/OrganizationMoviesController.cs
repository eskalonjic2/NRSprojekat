using System;
using System.Threading.Tasks;
using CinemaluxAPI.Common;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.OrganizationDbContext.Models;
using Microsoft.AspNetCore.Mvc;
using CinemaluxAPI.Service.Web.Contracts;
using CinemaluxAPI.Service.Web.Movies.DTO;
using CinemaluxAPI.Services;
using CinemaluxAPI.Services.Screenings.DTO;

namespace CinemaluxAPI.API.Controllers.Web
{
    [ApiController]
    [Route("web/movies")]
    public class OrganizationMoviesController : ControllerBase
    {
        #region Properties
        private IOrganizationMoviesService OrganizationMoviesService { get; set; }
        private IScreeningService ScreeningService { get; set; }

        #endregion

        #region Constructor
        
        public OrganizationMoviesController(IOrganizationMoviesService organizationMoviesService, IScreeningService screeningService)
        {
            OrganizationMoviesService = organizationMoviesService;
            ScreeningService = screeningService;
        }
        
        #endregion
        
        #region Routes
           
        [HttpGet("allScreenings")]
        public ActionResult<GridData<FilteredScreeningsDTO>> GetAllScreenings([FromQuery] ScreeningGridParams gridParams)
        {
            return ScreeningService.GetScreenings(gridParams);
        }
        
        [HttpGet("dropdown")]
        public ActionResult<MovieDropdownDTO[]> Dropdown([FromQuery] GridParams gridParams)
        {
            return Ok(OrganizationMoviesService.GetDropdownValues(gridParams));
        }
        
        [HttpGet("all")]
        public ActionResult<OrganizationMovieOverviewDTO[]> GetAll()
        {
            return Ok(OrganizationMoviesService.GetAllMovies());
        }
        
        [HttpGet("verboseAll")]
        public ActionResult<GridData<OrganizationVerboseMoviesDTO>> GetAllVerboseMovies([FromQuery] GridParams gridParams)
        {
            return Ok(OrganizationMoviesService.GetAllVerboseMovies(gridParams));
        }

        [HttpGet("{movieId}")]
        public ActionResult<OrganizationMovieInfoDTO> GetMovie([FromRoute] short movieId)
        {
            return Ok(OrganizationMoviesService.GetMovie(movieId));
        }
        
        [HttpGet("screenings")]
        public ActionResult<GridData<OrganizationMovieInfoDTO>> GetMoviesScreenings([FromRoute] GridParams gridParams)
        {
            return Ok(OrganizationMoviesService.GetMoviesScreenings(gridParams));
        }
        
        [HttpGet("latest")]
        public ActionResult<OrganizationMovieInfoDTO> GetLatestMovies()
        {
            return Ok(OrganizationMoviesService.GetLatestMovies());
        }
        
        [HttpGet("bestRated")]
        public ActionResult<OrganizationMovieInfoDTO> GetBestRatedMovies()
        {
            return Ok(OrganizationMoviesService.GetBestRatedMovies());
        }
        
        [HttpPost("similarGenres/{currentMovieId}")]
        public ActionResult<OrganizationMovieOverviewDTO[]> GetMoviesWithSimilarGenres([FromRoute] int currentMovieId, [FromBody] SimilarMoviesDTO dto)
        {
            return Ok(OrganizationMoviesService.GetMoviesWithSimilarGenres(currentMovieId, dto));
        }
        
        [HttpPost("")]
        public async Task<ActionResult<Movie>> AddMovie([FromForm] AddMovieDTO dto)
        {
            return Ok(await OrganizationMoviesService.AddMovie(dto));
        }
        
        [HttpDelete("archive/{id}")]
        public ActionResult<bool> ArchiveMovie([FromRoute] short id)
        {
            return Ok(OrganizationMoviesService.ArchiveMovie(id));
        }
        
        #endregion
    }
}