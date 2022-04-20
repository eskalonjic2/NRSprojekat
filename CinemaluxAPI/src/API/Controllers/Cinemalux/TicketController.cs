using System;
using CinemaluxAPI.Common;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
using CinemaluxAPI.Services;
using CinemaluxAPI.src.Services.Ticket.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CinemaluxAPI.API.Controllers
{
    [ApiController]
    [Route("tickets")]
    public class TicketController: CinemaluxControllerBase
    {
        #region Properties

        private ITicketService TicketService { get; }

        #endregion

        #region Constructor

        public TicketController(ITicketService ticketService)
        {
            TicketService = ticketService;
        }

        #endregion

        #region Routes

        [HttpGet("grid")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult<GridData<TicketDTO>> GetAllTickets([FromQuery] GridParams gridParams)
        {
            return Ok(TicketService.GetTickets(gridParams));
        }
        
        [HttpGet("{ticketId}")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult GetTicket([FromRoute] int ticketId)
        {
            return Ok(TicketService.GetTicket(ticketId));
        }

        [HttpPost("add")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult AddTicket([FromBody] TicketDTO dto)
        {
            return Ok(TicketService.AddTicket(dto, CurrentIdentity));
        }

        [HttpPost("multiple")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult AddTickets([FromBody] AddTicketsDTO dto)
        {
            return Ok(TicketService.AddTickets(dto, CurrentIdentity));
        }
        
        [HttpDelete("delete/{ticketId}")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult<bool> DeleteTicket([FromRoute] int ticketId)
        {
            return Ok(TicketService.DeleteTicket(ticketId));
        }

        [HttpPost("archive/{ticketId}")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult ArchiveTicket([FromRoute] int ticketId)
        {
            return Ok(TicketService.ArchiveTicket(ticketId));
        }

        #endregion

    }
}
