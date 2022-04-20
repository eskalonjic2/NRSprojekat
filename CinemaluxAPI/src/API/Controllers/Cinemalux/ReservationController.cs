using CinemaluxAPI.Common;
using CinemaluxAPI.Services;
using Microsoft.AspNetCore.Mvc;
using CinemaluxAPI.Services.Reservations.DTO;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.API.Controllers
{
    [ApiController]
    [Route("reservations")]
    public class ReservationController : CinemaluxControllerBase
    {
        #region Properties

        private IReservationService ReservationService { get; }

        #endregion
        
        #region Constructor

        public ReservationController(IReservationService reservationService)
        {
            ReservationService = reservationService;
        }
        
        #endregion
        
        #region Routes
        
        [HttpGet("")]
        public ActionResult<GridData<ReservationGridDTO>> GetReservations([FromQuery] ReservationQueryParams queryParams)
        {
            return Ok(ReservationService.GetReservations(queryParams));
        }
        
        [HttpGet("{reservationId}")]
        public ActionResult<Reservation> GetReservation([FromRoute] int reservationId, [FromBody] ReservationQueryParams queryParams)
        {
            return Ok(ReservationService.GetReservation(reservationId, queryParams));
        }

        [HttpPost("")]
        public ActionResult<Reservation> AddReservation([FromBody] AddReservationDTO dto)
        {
            return Ok(ReservationService.AddReservation(dto, CurrentIdentity));
        }

        [HttpPut("{reservationId}")]
        public ActionResult<Reservation> ModifyReservation(int reservationId, [FromBody] ModifyReservationDTO dto)
        {
            return Ok(ReservationService.ModifyReservation(reservationId, dto));
        }

        [HttpPost("/finalize/{reservationId}")]
        public ActionResult<Reservation> FinalizeReservation(int reservationId, [FromBody] ModifyReservationDTO dto)
        {
            return Ok(ReservationService.ModifyReservation(reservationId, dto));
        }
        
        [HttpDelete("cancel/{reservationId}")]
        public ActionResult<bool> CancelReservation(int reservationId)
        {
            return Ok(ReservationService.CancelReservation(reservationId));
        }
        
        #endregion
    }
}