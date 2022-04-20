using System;
using CinemaluxAPI.Common;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
using CinemaluxAPI.Service.Contracts;
using CinemaluxAPI.Service.Halls;
using CinemaluxAPI.Service.Halls.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CinemaluxAPI.API.Controllers
{
    [ApiController]
    [Route("hall")]
    public class HallsController : CinemaluxControllerBase
    {
        #region Properties
        
        private IHallService HallsService { get; }

        #endregion

        #region Constructor

        public HallsController(IHallService hallsService)
        {
            HallsService = hallsService;
        }

        #endregion
        
        #region Routes

        [HttpGet("grid")]
        public GridData<Hall> GetHallsGrid([FromQuery] GridParams gridParams)
        {
            return HallsService.GetAllGrid(gridParams);
        }
        
        [HttpGet("all")]
        public Hall[] GetAllHalls()
        {
            return HallsService.GetAll();
        }
        
        [HttpGet("{hallId}")]
        public Hall GetHall([FromRoute] byte hallId)
        {
            return HallsService.GetHall(hallId);
        }

        [HttpPost("")]
        public Hall AddHall([FromBody] AddHallDTO dto)
        {
            return HallsService.AddHall(dto, CurrentIdentity);
        }

        [HttpPut("")]
        public Hall ModifyHall([FromRoute] byte hallId, [FromBody] ModifyHallDTO dto)
        {
            return HallsService.ModifyHall(hallId, dto);
        }

        [HttpDelete("/archive/{hallId}")]
        public bool ArchiveHall([FromRoute] byte hallId)
        {
            Console.WriteLine(hallId);
            return HallsService.ArchiveHall(hallId, CurrentIdentity);
        }
        
        #endregion
    }
}