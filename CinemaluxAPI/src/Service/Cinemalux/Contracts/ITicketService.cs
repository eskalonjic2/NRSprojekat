using CinemaluxAPI.Auth;
using CinemaluxAPI.Common;
using CinemaluxAPI.src.Services.Ticket.DTO;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.Services
{
    public interface ITicketService
    {
        public GridData<TicketDTO> GetTickets(GridParams gridParams);
        public Ticket GetTicket(long ticketId);
        public Ticket AddTicket(TicketDTO dto, Identity employee);
        public bool AddTickets(AddTicketsDTO dto, Identity employee);
        public Ticket ArchiveTicket (long ticketId);
        public bool DeleteTicket(long ticketId);
    }
}