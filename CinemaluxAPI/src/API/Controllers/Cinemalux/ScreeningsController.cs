using System;
using CinemaluxAPI.Common;
using CinemaluxAPI.Services;
using Microsoft.AspNetCore.Mvc;
using CinemaluxAPI.Services.Screenings.DTO;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.API.Controllers
{
    [ApiController]
    [Route("screenings")]
    public class ScreeningsController : CinemaluxControllerBase
    {
        #region Properties

        private IScreeningService ScreeningService;

        #endregion
        
        #region Constructor

        public ScreeningsController(IScreeningService screeningService)
        {
            ScreeningService = screeningService;
        }
        
        #endregion
        
        #region Routes

        [HttpGet("{screeningId}")]
        public ActionResult<ScreeningDTO> GetScreening([FromRoute] int screeningId)
        {
            return ScreeningService.GetScreeningById(screeningId);
        }
        
        [HttpGet("all")]
        public ActionResult<GridData<FilteredScreeningsDTO>> GetAllScreenings([FromQuery] ScreeningGridParams gridParams)
        {
            return ScreeningService.GetScreenings(gridParams);
        }
        
        [HttpGet("timeline")]
        public ActionResult<GridData<MovieScreeningsTimelineDTO>> GetScreeningsTimeline([FromQuery] ScreeningGridParams gridParams)
        {
            return ScreeningService.GetScreeningsTimeline(gridParams);
        }

        [HttpPost("")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult<Screening> AddScreening([FromBody] AddScreeningDTO dto)
        {
            return ScreeningService.AddScreening(dto, CurrentIdentity);
        }
        
        [HttpDelete("archive/{screeningId}")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult<bool> ArchiveScreening([FromRoute] long screeningId)
        {
            return ScreeningService.ArchiveScreening(screeningId, CurrentIdentity);
        }
        
        #endregion
    }
}