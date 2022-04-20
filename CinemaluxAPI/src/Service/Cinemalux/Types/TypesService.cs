using System;
using System.Net;
using System.Linq;
using CinemaluxAPI.Auth;
using CinemaluxAPI.Common;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.Services.Types.DTO;
using CinemaluxAPI.DAL.CinemaluxCatalogue;
using CinemaluxAPI.DAL.OrganizationDbContext;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
using CinemaluxAPI.DAL.OrganizationDbContext.Models;
using CinemaluxAPI.Service.Cinemalux.Contracts;
using CinemaluxAPI.Services.Types.DTO.Genres;
using DiscountType = CinemaluxAPI.DAL.CinemaluxCatalogue.Models.DiscountType;
using GenreDTO = CinemaluxAPI.Service.Web.Genres.DTO.GenreDTO;
using OrderType = CinemaluxAPI.DAL.CinemaluxCatalogue.Models.OrderType;
using PaymentType = CinemaluxAPI.DAL.CinemaluxCatalogue.Models.PaymentType;
using ReservationType = CinemaluxAPI.DAL.CinemaluxCatalogue.Models.ReservationType;

//TODO Archive By ID 
namespace CinemaluxAPI.Services.Types
{
    public class TypesService : ITypesService
    {
        #region Global Properties

        private CinemaluxDbContext DbContext;
        private OrganizationDbContext OrganizationDbContext;

        #endregion
        
        #region Constructor

        public TypesService(CinemaluxDbContext cinemaluxDbContext, OrganizationDbContext organizationDbContext)
        {
            DbContext = cinemaluxDbContext;
            OrganizationDbContext = organizationDbContext;
        }
        
        #endregion
        
        #region Discount Types

        public GridData<DiscountType> GetAllDiscoutnTypeGrid(GridParams gridParams)
        {
            var rows = DbContext.DiscountTypes.Select(x => x);

            if (gridParams.SQ.IsNotNull())
                rows = rows.Where(x => x.Code.Contains(gridParams.SQ) || x.Description.Contains(gridParams.SQ));

            return new GridData<DiscountType>(rows, gridParams);
        }
        
        public DiscountType GetDiscountType(string discountTypeCode)
        {
            return DbContext.DiscountTypes.FirstOrDefault(x => x.Code == discountTypeCode);
        }
        public DiscountType AddDiscountType(AddDiscountTypeDTO dto, Identity employee)
        {
            if (dto.DiscountPct <= 0) 
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid discount percentage");
     
            var sameType = DbContext.DiscountTypes.FirstOrDefault(x => x.Code.ToLower().Equals(dto.Code.ToLower()));
            if (sameType != null)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Tip placanja postoji");
            
            DiscountType discountType = new DiscountType
            {
                Code = dto.Code.ToUpper(),
                Description = dto.Description,
                DiscountPct = dto.DiscountPct,
                ExpiresOn = dto.ExpiresOn,
                CreatedBy = employee.Name
            };

            DbContext.DiscountTypes.Add(discountType);
            DbContext.SaveChanges();

            if (dto.AddToOrganization)
            {
                // OrganizationDbContext.SharedDiscountTypes.Add(discountType);
                // OrganizationDbContext.SaveChanges();
            }
            
            return discountType;
        }
        public DiscountType ModifyDiscountType(string discountTypeCode, ModifyDiscountTypeDTO dto, Identity employee)
        {
            if (dto.DiscountPct <= 0) 
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid discount percentage");

            var discountType = DbContext.DiscountTypes.FirstOrDefault(x => x.Code == discountTypeCode);
            discountType.EnsureNotNull("Discount type not found");
            
            bool isCodeChanged = discountType.Code != dto.Code;
            
            discountType.Code = dto.Code;
            discountType.Description = dto.Description;
            discountType.DiscountPct = dto.DiscountPct;
            discountType.ExpiresOn = dto.ExpiresOn;
            discountType.ModifiedBy = employee.Name;

            DbContext.DiscountTypes.Update(discountType);
            DbContext.SaveChanges();
            
            if(isCodeChanged)
                foreach (var discountItem in discountType.DiscountItems)
                    discountItem.DiscountTypeCode = discountType.Code;

            DbContext.SaveChanges();

            return discountType;
        }
        public DiscountType ArchiveDiscountType(string discountTypeCode, Identity employee)
        {
            var discountType = DbContext.DiscountTypes.FirstOrDefault(x => x.Code == discountTypeCode);
            discountType.EnsureNotNull("Discount type not found");

            discountType.ArchivedBy = employee.Name;
            
            DbContext.DiscountTypes.Archive(discountType);
            DbContext.SaveChanges();

            return discountType;
        }
        
        #endregion
        
        #region Payment Types
        
        public GridData<PaymentType> GetAllPaymentTypeGrid(GridParams gridParams)
        {
            var rows = DbContext.PaymentTypes.Select(x => x);

            if (gridParams.SQ.IsNotNull())
                rows = rows.Where(x => x.Code.Contains(gridParams.SQ) || x.Name.Contains(gridParams.SQ));

            return new GridData<PaymentType>(rows, gridParams);
        }

        public PaymentType[] GetAllPaymentTypes()
        {
            return DbContext.PaymentTypes.ToArray();
        }
        
        public PaymentType GetPaymentType(string paymentTypeCode)
        {
            return DbContext.PaymentTypes.FirstOrDefault(x => x.Code == paymentTypeCode);
        }
        
        public PaymentType AddPaymentType(AddPaymentTypeDTO dto, Identity employee)
        {
            var sameType = DbContext.PaymentTypes.FirstOrDefault(x => x.Code.ToLower().Equals(dto.Code.ToLower()));
            if (sameType != null)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Tip placanja postoji");
            
            PaymentType paymentType = new PaymentType
            {
                Code = dto.Code.ToUpper(),
                Name = dto.Name,
                CreatedBy = employee.Name
            };

            DbContext.PaymentTypes.Add(paymentType);
            DbContext.SaveChanges();

            if (dto.AddToOrganization)
            {
                // OrganizationDbContext.SharedPaymentTypes.Add(orderType);
                // OrganizationDbContext.SaveChanges();
            }
            
            return paymentType;
        }

        public PaymentType ModifyPaymentType(string paymentTypeCode, ModifyPaymentTypeDTO dto)
        {
            var paymentType = DbContext.PaymentTypes.FirstOrDefault(x => x.Code == paymentTypeCode);
            paymentType.EnsureNotNull("Payment type not found");
            
            bool isCodeChanged = paymentType.Code != dto.Code;
            
            paymentType.Code = dto.Code;
            paymentType.Name = dto.Name;

            DbContext.PaymentTypes.Update(paymentType);
            DbContext.SaveChanges();
            
            if(isCodeChanged)
                foreach (var order in paymentType.Orders)
                    order.PaymentTypeCode = paymentType.Code;

            DbContext.SaveChanges();

            return paymentType;
        }
        
        public PaymentType ArchivePaymentType(string paymentTypeCode, Identity employee)
        {
            var paymentType = DbContext.PaymentTypes.FirstOrDefault(x => x.Code == paymentTypeCode);
            paymentType.EnsureNotNull("Payment type not found");

            paymentType.ArchivedBy = employee.Username;
            
            DbContext.PaymentTypes.Archive(paymentType);
            DbContext.SaveChanges();

            return paymentType;
        }
        
        #endregion

        #region Reservation Types

        public GridData<ReservationType> GetAllReservationTypeGrid(GridParams gridParams)
        {
            var rows = DbContext.ReservationTypes.Select(x => x);
            
            if (gridParams.SQ.IsNotNull())
                rows = rows.Where(x => x.Code.Contains(gridParams.SQ) || x.Name.Contains(gridParams.SQ));

            return new GridData<ReservationType>(rows, gridParams);
        }
        
        public ReservationType GetReservationType(string reservationTypeCode)
        {
            return DbContext.ReservationTypes.FirstOrDefault(x => x.Code == reservationTypeCode);
        }
        
        public ReservationType AddReservationType(AddReservationTypeDTO dto, Identity employee)
        {
            var sameType = DbContext.ReservationTypes.FirstOrDefault(x => x.Code.ToLower().Equals(dto.Code.ToLower()));

            if (sameType != null)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Tip rezervacije postoji");

            ReservationType reservationType = new ReservationType
            {
                Code = dto.Code.ToUpper(),
                Name = dto.Name,
                CreatedBy = employee.Name
            };

            DbContext.ReservationTypes.Add(reservationType);
            DbContext.SaveChanges();

            if (dto.AddToOrganization)
            {
                // OrganizationDbContext.SharedReservationTypes.Add(orderType);
                // OrganizationDbContext.SaveChanges();
            }
            
            return reservationType;
        }

        public ReservationType ModifyReservationType(string reservationTypeCode, ModifyReservationTypeDTO dto, Identity employee)
        {
            var reservationType = DbContext.ReservationTypes.FirstOrDefault(x => x.Code == reservationTypeCode);
            reservationType.EnsureNotNull("Discount type not found");
            
            bool isCodeChanged = reservationType.Code != dto.Code;
            
            reservationType.Code = dto.Code;
            reservationType.Name = dto.Name;

            DbContext.ReservationTypes.Update(reservationType);
            DbContext.SaveChanges();
            
            if(isCodeChanged)
                foreach (var reservation in reservationType.Reservations)
                    reservation.ReservationTypeCode = reservationType.Code;

            DbContext.SaveChanges();

            return reservationType;
        }
        
        public ReservationType ArchiveReservationType(string reservationTypeCode, Identity employee)
        {
            var reservationType = DbContext.ReservationTypes.FirstOrDefault(x => x.Code == reservationTypeCode);
            reservationType.EnsureNotNull("Reservation type not found");

            reservationType.ArchivedBy = employee.Username;
            DbContext.ReservationTypes.Archive(reservationType);
            DbContext.SaveChanges();

            return reservationType;
        }
        
        #endregion
        
        #region Order Types

        public OrderType[] GetAllOrderTypes()
        {
            return DbContext.OrderTypes.ToArray();
        }
        
        public OrderType GetOrderType(string orderTypeCode)
        {
            return DbContext.OrderTypes.FirstOrDefault(x => x.Code == orderTypeCode);
        }
        
        public OrderType AddOrderType(AddOrderTypeDTO dto, Identity employee)
        {
            OrderType orderType = new OrderType
            {
                Code = dto.Code,
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                CreatedBy = employee.Name
            };

            DbContext.OrderTypes.Add(orderType);
            DbContext.SaveChanges();

            if (dto.AddToOrganization)
            {
                // OrganizationDbContext.SharedOrderTypes.Add(orderType);
                // OrganizationDbContext.SaveChanges();
            }
            
            return orderType;
        }

        public OrderType ModifyOrderType(string orderTypeCode, ModifyOrderTypeDTO dto, Identity employee)
        {
            if (dto.Price <= 0) 
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid price");

            var orderType = DbContext.OrderTypes.FirstOrDefault(x => x.Code == orderTypeCode);
            orderType.EnsureNotNull("Discount type not found");
            
            bool isCodeChanged = orderType.Code != dto.Code;
            
            orderType.Code = dto.Code;
            orderType.Name = dto.Name;
            orderType.Description = dto.Description;
            orderType.Price = dto.Price;
            orderType.ModifiedBy = employee.Name;
            
            DbContext.OrderTypes.Update(orderType);
            DbContext.SaveChanges();
            
            if(isCodeChanged)
                foreach (var orderItem in orderType.OrderItems)
                    orderItem.OrderTypeCode = orderType.Code;

            DbContext.SaveChanges();

            return orderType;
        }
        
        public OrderType ArchiveOrderType(string orderTypeCode, Identity employee)
        {
            var orderType = DbContext.OrderTypes.FirstOrDefault(x => x.Code == orderTypeCode);
            orderType.EnsureNotNull("Order type not found");

            orderType.ArchivedBy = employee.Name;
            
            DbContext.DiscountTypes.Archive(orderType);
            DbContext.SaveChanges();

            return orderType;
        }
        
        #endregion
          
        #region Ticket Types
        
        public GridData<TicketType> GetAllTicketTypesGrid(GridParams gridParams)
        {
            var rows = DbContext.TicketTypes.Select(x => x);

            if (gridParams.SQ.IsNotNull())
                rows = rows.Where(x => x.Code.Contains(gridParams.SQ) || x.Description.Contains(gridParams.SQ));

            return new GridData<TicketType>(rows, gridParams);
        }
        
        public TicketType[] GetAllTicketTypes()
        {
            return DbContext.TicketTypes.ToArray();
        }
        public TicketType GetTicketTypes(string ticketTypeCode)
        {
            return DbContext.TicketTypes.FirstOrDefault(x => x.Code == ticketTypeCode);
        }
        public TicketType AddTicketType(AddTicketTypeDTO dto, Identity employee)
        {
            var sameType = DbContext.TicketTypes.FirstOrDefault(x => x.Code.ToLower().Equals(dto.Code.ToLower()));
            if (sameType != null)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Tip karte postoji");

            TicketType ticketType = new TicketType
            {
                Code = dto.Code.ToUpper(),
                Price = dto.Price,
                Description = dto.Description,
                CreatedBy = employee.Name
            };

            DbContext.TicketTypes.Add(ticketType);
            DbContext.SaveChanges();

            if (dto.AddToOrganization)
            {
                // OrganizationDbContext.SharedTicketTypes.Add(orderType);
                // OrganizationDbContext.SaveChanges();
            }
            
            return ticketType;
        }
        public TicketType ModifyTicketType(string ticketTypeCode, ModifyTicketTypeDTO dto, Identity employee)
        {
            var ticketType = DbContext.TicketTypes.FirstOrDefault(x => x.Code == ticketTypeCode);
            ticketType.EnsureNotNull("Ticket type not found");
            
            bool isCodeChanged = ticketType.Code != dto.Code;
            
            ticketType.Code = dto.Code;
            ticketType.Price = dto.Price;
            ticketType.Description = dto.Description;

            DbContext.TicketTypes.Update(ticketType);
            DbContext.SaveChanges();

            if (isCodeChanged)
            {
                foreach (var ticket in ticketType.Tickets)
                    ticket.TicketTypeCode = ticketType.Code;
                
                foreach (var ticket in ticketType.Screenings)
                    ticket.DefaultTicketTypeCode = ticketType.Code;
            }

            DbContext.SaveChanges();

            return ticketType;
        }
        public TicketType ArchiveTicketType(string ticketTypeCode, Identity employee)
        {
            var ticketType = DbContext.TicketTypes.FirstOrDefault(x => x.Code == ticketTypeCode);
            ticketType.EnsureNotNull("Ticket type not found");

            ticketType.ArchivedBy = employee.Username;
            DbContext.TicketTypes.Archive(ticketType);
            DbContext.SaveChanges();

            return ticketType;
        }
        
        #endregion
        
        #region Genres

        public GridData<GenreDTO> GetAllGenresGrid(GridParams gridParams)
        {
            var rows = OrganizationDbContext.Genres.Select(x => new GenreDTO
            {
                Code = x.Code,
                Name = x.Name,
                MoviesWithThisGenres = x.MovieGenres.Count
            });

            if (gridParams.SQ.IsNotNull())
                rows = rows.Where(x => x.Code.Contains(gridParams.SQ) || x.Name.Contains(gridParams.SQ));

            return new GridData<GenreDTO>(rows, gridParams);
        }
        
        public GenreDTO[] GetGenres()
        {
            return OrganizationDbContext.Genres.Select(x => new GenreDTO
            {
                Code = x.Code,
                Name = x.Name
            }).ToArray();
        }
        
        public GenreDTO GetGenre(string genreCode)
        {
            return GetGenres().FirstOrDefault(x => x.Code == genreCode);
        }

        public GenreDTO AddGenre(AddGenreDTO dto)
        {
            var sameGenre = OrganizationDbContext.Genres.FirstOrDefault(x => x.Code.ToLower().Equals(dto.Code.ToLower()));

            if (sameGenre != null)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Žanr postoji");

            Genre genre = new Genre
            {
                Code = dto.Code.ToUpper(),
                Name = dto.Name,
            };

            OrganizationDbContext.Add(genre);
            OrganizationDbContext.SaveChanges();

            return new GenreDTO
            {
                Code = dto.Code,
                Name = dto.Name
            };
        }

        public bool ArchiveGenre(string genreCode)
        {
            var genre = OrganizationDbContext.Genres.FirstOrDefault(x => x.Code == genreCode);
            genreCode.EnsureNotNull("Genre not found");
           
            OrganizationDbContext.Genres.Remove(genre);
            OrganizationDbContext.SaveChanges();

            return true;
        }
        
        #endregion
    }
}