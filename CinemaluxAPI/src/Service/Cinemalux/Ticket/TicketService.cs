using System;
using System.Net;
using System.Linq;
using CinemaluxAPI.Auth;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using CinemaluxAPI.Common;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.CinemaluxCatalogue;
using CinemaluxAPI.src.Services.Ticket.DTO;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
using CinemaluxAPI.DAL.OrganizationDbContext;

namespace CinemaluxAPI.Services
{
    public class TicketService : ITicketService
    {
        #region Properties

        private CinemaluxDbContext DbContext { get; }
        private OrganizationDbContext OrganizationDbContext { get; }

        #endregion

        #region Constructor

        public TicketService(CinemaluxDbContext dbContext, OrganizationDbContext organizationContext)
        {
            DbContext = dbContext;
            OrganizationDbContext = organizationContext;
        }

        #endregion

        #region Action Methods

        public GridData<TicketDTO> GetTickets(GridParams gridParams)
        {
            var rows = DbContext.Tickets.Select(x => new TicketDTO
            {
                Id = x.Id,
                ScreeningId = x.ScreeningId,
                MovieTitle = x.Screening.CinemaluxMovie.Title,
                OrderId = x.OrderId,
                ReservationId = x.ReservationId,
                TicketTypeCode = x.TicketTypeCode,
                IsUsed = x.IsUsed,
                TicketPrice = x.TicketType.Price,
                SeatLabel = x.SeatLabel,
                CreatedAt = x.CreatedAt
            }).ToList();

            if (gridParams.SQ.IsNotNull())
                rows = rows.Where(x => x.MovieTitle.ToLower().Contains(gridParams.SQ.ToLower())).ToList();

            return new GridData<TicketDTO>(rows.AsQueryable(), gridParams);
        }
        
        public Ticket GetTicket(long ticketId)
        {
            Ticket ticket = DbContext.Tickets.FirstOrDefault(x => x.Id == ticketId);
            ticket.EnsureNotNull("No ticket found with the given Id");

            return ticket;
        }

        public Ticket AddTicket(TicketDTO dto, Identity employee)
        {
            var screening = DbContext.Screenings.Include(x => x.Hall).FirstOrDefault(x => dto.ScreeningId == x.Id);
            screening.EnsureNotNull("Screening doesn't exist");

            var order = DbContext.Orders.FirstOrDefault(x => x.Id == dto.OrderId);
            order.EnsureNotNull("Order not found");

            var reservation = DbContext.Reservations.FirstOrDefault(x => x.Id == dto.ReservationId);
            reservation.EnsureNotNull("Reservation not found");

            var ticketTypeCode = DbContext.TicketTypes.FirstOrDefault(x => x.Code == dto.TicketTypeCode);
            ticketTypeCode.EnsureNotNull("Invalid ticket type code");

            string[] occupiedSeatLabels = screening.Tickets.Select(x => x.SeatLabel).ToArray();

            if (screening.Tickets.Count >= screening.Hall.Capacity)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Screening is full. No available seats.");

            if (!Regex.Match(dto.SeatLabel, screening.Hall.SeatValidityRegex).Success)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid seat number");

            if (occupiedSeatLabels.Contains(dto.SeatLabel))
                throw new HttpResponseException(HttpStatusCode.BadRequest, $"Mjesto {dto.SeatLabel} zauzeto");

            Ticket newTicket = new Ticket
            {
                ScreeningId = dto.ScreeningId,
                OrderId = dto.OrderId,
                ReservationId = dto.ReservationId,
                TicketTypeCode = dto.TicketTypeCode,
                SeatLabel = dto.SeatLabel,
                CreatedBy = employee.Name
            };

            DbContext.Tickets.Add(newTicket);
            DbContext.SaveChanges();

            return newTicket;
        }

        public bool AddTickets(AddTicketsDTO dto, Identity employee)
        {
            var screening = DbContext.Screenings.Include(x => x.Hall).
                Include(x => x.Tickets).FirstOrDefault(x => dto.ScreeningId == x.Id);
            screening.EnsureNotNull("Screening doesn't exist");

            var order = DbContext.Orders.FirstOrDefault(x => x.Id == dto.OrderId);
            order.EnsureNotNull("Order not found");
            
            if (dto.ReservationId != -1) {
                var reservation = DbContext.Reservations.FirstOrDefault(x => x.Id == dto.ReservationId);
                reservation.EnsureNotNull("Reservation not found");
            }
            
            var ticketTypeCode = DbContext.TicketTypes.FirstOrDefault(x => x.Code == dto.TicketTypeCode);
            ticketTypeCode.EnsureNotNull("Invalid ticket type code");
            
            string[] occupiedSeatLabels = screening.Tickets.Select(x => x.SeatLabel).ToArray();

            foreach (string seatLable in dto.SeatLabels)
            {
                if (screening.Tickets.Count >= screening.Hall.Capacity)
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "Screening is full. No available seats.");

                if (occupiedSeatLabels.Contains(seatLable))
                    throw new HttpResponseException(HttpStatusCode.BadRequest, $"Mjesto {seatLable} zauzeto");

                Ticket newTicket = new Ticket
                {
                    ScreeningId = dto.ScreeningId,
                    OrderId = dto.OrderId,
                    ReservationId = dto.ReservationId == -1 ? null : dto.ReservationId,
                    TicketTypeCode = dto.TicketTypeCode,
                    SeatLabel = seatLable,
                    CreatedBy = employee.Name
                };

                order.TotalPrice += ticketTypeCode.Price;
                DbContext.Tickets.Add(newTicket);
            }

            DbContext.Update(order);
            DbContext.SaveChanges();
            return true;
        }

        public bool DeleteTicket(long ticketId)
        {
            var ticket = DbContext.Tickets.Include(x => x.TicketType).FirstOrDefault(x => x.Id == ticketId);
            ticket.EnsureNotNull("Ticket not found");

            var order = DbContext.Orders.FirstOrDefault(x => x.Id == ticket.OrderId);
            order.EnsureNotNull("Narudzba ne postoji");

            order.TotalPrice -= ticket.TicketType.Price;
            
            DbContext.Tickets.Remove(ticket);
            DbContext.SaveChanges();
            
            return true;
        }

        public Ticket ArchiveTicket(long ticketId)
        {
            var ticket = DbContext.Tickets.FirstOrDefault(x => x.Id == ticketId);

            ticket.EnsureNotNull("Ticket not found");

           DbContext.Tickets.Archive(ticket);
           DbContext.SaveChanges();

            return ticket;
        }

        #endregion

    }
}
