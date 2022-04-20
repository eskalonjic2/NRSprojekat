using CinemaluxAPI.Auth;
using CinemaluxAPI.Common;
using CinemaluxAPI.Services.Reservations.DTO;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.Services
{
    public interface IReservationService
    {
        public GridData<ReservationGridDTO>  GetReservations(ReservationQueryParams queryParams);
        public Reservation GetReservation(int reservationId, ReservationQueryParams queryParams);
        public Reservation AddReservation(AddReservationDTO dto, Identity currentIdentity);
        public Reservation ModifyReservation(int reservationId, ModifyReservationDTO dto);
        public bool CancelReservation(int reservationId);
    }
}