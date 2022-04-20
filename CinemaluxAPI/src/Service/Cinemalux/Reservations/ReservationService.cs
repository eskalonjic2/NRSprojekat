using System;
using System.Net;
using System.Linq;
using CinemaluxAPI.Auth;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using CinemaluxAPI.Common;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.CinemaluxCatalogue;
using CinemaluxAPI.Services.Reservations.DTO;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.Services.Reservations
{
    public class ReservationService : IReservationService
    {
        #region Properties

        private CinemaluxDbContext DbContext { get; }

        #endregion

        #region Constructor

        public ReservationService(CinemaluxDbContext context)
        {
            DbContext = context;
        }
        
        #endregion

        #region Action Methods
        
        public GridData<ReservationGridDTO> GetReservations(ReservationQueryParams queryParams)
        {
            IQueryable<Reservation> query = DbContext.Reservations;
       
            if (queryParams.IncludeTypeCode.HasValue)
                query.Include(x => x.ReservationType);

            if (queryParams.IncludeScreening.HasValue)
                query.Include(x => x.Screening).ThenInclude(x => x.CinemaluxMovie);
            
            if (queryParams.IncludeScreening.HasValue)
                query.Include(x => x.Screening);

            if (queryParams.ScreeningId.HasValue)
                query = query.Where(x => x.ScreeningId == queryParams.ScreeningId);

            if (queryParams.TypeCode != null)
                query = query.Where(x => x.ReservationTypeCode == queryParams.TypeCode);

            var rows = query.Select(x => new ReservationGridDTO
            {
                Id = x.Id,
                FullName = $"{x.Name} {x.Surname}",
                ReservationTypeCode = x.ReservationTypeCode,
                ContactPhone = x.ContactPhone,
                IsPaid = x.IsPaid,
                CreatedAt = x.CreatedAt,
                Screening = new ReservationGridDTO.ReservationGridScreeningDTO
                {
                    Id = x.Screening.Id,
                    MovieTitle = x.Screening.CinemaluxMovie.Title,
                    HallName = x.Screening.Hall.Name,
                    Time = x.Screening.ScreeningTime,
                    Date = x.Screening.Date
                }
            }).ToList();
            
            if (queryParams.SQ.IsNotNull())
                rows = rows.Where(x => x.FullName.ToLower().Contains(queryParams.SQ)).ToList();

            return new GridData<ReservationGridDTO>(rows.AsQueryable(), queryParams);
        }
        
        public Reservation GetReservation(int reservationId, ReservationQueryParams queryParams)
        {
            IQueryable<Reservation> query = DbContext.Reservations.Where(x => x.Id == reservationId);

            if (queryParams.IncludeTypeCode.HasValue)
                query.Include(x => x.ReservationType);

            if (queryParams.IncludeScreening.HasValue)
                query.Include(x => x.Screening);

            if (queryParams.ScreeningId.HasValue)
                query = query.Where(x => x.ScreeningId == queryParams.ScreeningId);

            if (queryParams.TypeCode != null)
                query = query.Where(x => x.ReservationTypeCode == queryParams.TypeCode);
                
            return query.FirstOrDefault();
        }
        
        public Reservation AddReservation(AddReservationDTO dto, Identity currentIdentity)
        {
            ReservationType type = DbContext.ReservationTypes.FirstOrDefault(x => x.Code.Equals(dto.ReservationTypeCode));
            type.EnsureNotNull("tajp nije nadjen");
            
            Screening screening = GetFullScreening(dto.ScreeningId);
            screening.EnsureNotNull("Projekcija ne postoji");
            
            Reservation reservation = new Reservation
            {
                ReservationTypeCode = dto.ReservationTypeCode,
                Name = dto.Name,
                Surname = dto.Surname,
                ContactPhone = dto.ContactPhone,
                IsPaid = dto.IsPaid,
                CreatedBy = currentIdentity.Id.ToString()
            };

            DbContext.Add(reservation);
            DbContext.SaveChanges();
            
            try
            {
                ValidateAndAddTickets(screening, dto.Tickets, reservation.Id, reservation.IsPaid);
                DbContext.SaveChanges();
            }
            catch 
            {
                DbContext.Remove(reservation);
                DbContext.SaveChanges();
                throw;
            }
            
            return reservation;
        }
        public Reservation ModifyReservation(int reservationId, ModifyReservationDTO dto)
        {
            Reservation reservation = DbContext.Reservations.FirstOrDefault(x => x.Id == reservationId);
            reservation.EnsureNotNull("rezervacija ne postoji");
            
            if (reservation.ReservationTypeCode != dto.ReservationTypeCode)
            {
                ReservationType type = DbContext.ReservationTypes.FirstOrDefault(x => x.Code.Equals(dto.ReservationTypeCode));
                type.EnsureNotNull("tajp nije nadjen");
                reservation.ReservationTypeCode = dto.ReservationTypeCode;
            }
            
            reservation.Name = dto.Name;
            reservation.Surname = dto.Surname;
            reservation.ContactPhone = dto.ContactPhone;
            reservation.IsPaid = dto.IsPaid;
            
            DbContext.Update(reservation);
            DbContext.SaveChanges();

            if (dto.Tickets.Length > 0)
                ValidateAndAddTickets(GetFullScreening(reservation.ScreeningId), dto.Tickets, reservation.Id, reservation.IsPaid);

            return reservation;
        }
        public void FinalizeReservation(int reservationId, FinalizeReservationDTO dto, Identity employee)
        {
            Reservation reservation = DbContext.Reservations.FirstOrDefault(x => x.Id == reservationId);
            reservation.EnsureNotNull("rezervacija ne postoji");

            Order order = new Order
            {
                EmployeeId = employee.Id,
                ReservationId = reservation.Id,
                PaymentTypeCode = dto.PaymentTypeCode,
                CreatedBy = employee.Name
            };

            DbContext.Orders.Add(order);
            DbContext.SaveChanges();

            foreach (Ticket ticket in reservation.Tickets)
            {
                ticket.OrderId = order.Id;
                ticket.CreatedBy = employee.Name;
            }
        }
        public bool CancelReservation(int reservationId)
        {
            Reservation reservation = DbContext.Reservations.FirstOrDefault(x => x.Id == reservationId);
            reservation.EnsureNotNull("rezervacija ne postoji");

            DbContext.Reservations.Remove(reservation);
            DbContext.SaveChanges();

            foreach (Ticket ticket in reservation.Tickets)
                DbContext.Tickets.Remove(ticket);
            
            return true;
        }
        
        #endregion
        
        #region Private Methods

        private void ValidateAndAddTickets(Screening screening, TicketReservationDTO[] Tickets, int reservationId, bool isPaid)
        {
            string[] availableTicketTypes = DbContext.TicketTypes.Select(x => x.Code).ToArray();
            
            foreach (var ticket in Tickets)
            {
                string[] occupiedSeatLabels = screening.Tickets.Select(x => x.SeatLabel).ToArray();
                
                if(ticket.ScreeningId != screening.Id)
                    throw new HttpResponseException(HttpStatusCode.BadRequest, $"Karta {ticket.SeatLabel} ne pripada navedenoj projekciji");

                if (!Regex.Match(ticket.SeatLabel, screening.Hall.SeatValidityRegex).Success)
                    throw new HttpResponseException(HttpStatusCode.BadRequest, $"Ime sjedista {ticket.SeatLabel} nije validno");

                if(occupiedSeatLabels.Contains(ticket.SeatLabel))
                    throw new HttpResponseException(HttpStatusCode.BadRequest, $"Mjesto {ticket.SeatLabel} zauzeto");

                if (availableTicketTypes.NotContains(ticket.TicketTypeCode))
                    throw new HttpResponseException(HttpStatusCode.BadRequest, $"Tip karte {ticket.TicketTypeCode} nije pronadjen");
                
                DbContext.Tickets.Add(new Ticket
                {
                    ScreeningId = screening.Id,
                    ReservationId = reservationId,
                    TicketTypeCode = ticket.TicketTypeCode,
                    SeatLabel = ticket.SeatLabel,
                    CreatedBy = isPaid ? "ORDER" : "RESERVATION"
                });
            }
        }

        private Screening GetFullScreening(long screeningId)
        { 
            return DbContext.Screenings
                .Include(x => x.Hall)
                .Include(x => x.Tickets)
                .FirstOrDefault(x => x.Id == screeningId);
        }
        
        #endregion
    }
}