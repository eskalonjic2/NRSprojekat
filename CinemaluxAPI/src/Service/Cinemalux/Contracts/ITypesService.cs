using CinemaluxAPI.Auth;
using CinemaluxAPI.Common;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
using CinemaluxAPI.DAL.OrganizationDbContext.Models;
using CinemaluxAPI.Services.Types.DTO;
using CinemaluxAPI.Services.Types.DTO.Genres;
using DiscountType = CinemaluxAPI.DAL.CinemaluxCatalogue.Models.DiscountType;
using GenreDTO = CinemaluxAPI.Service.Web.Genres.DTO.GenreDTO;
using OrderType = CinemaluxAPI.DAL.CinemaluxCatalogue.Models.OrderType;
using PaymentType = CinemaluxAPI.DAL.CinemaluxCatalogue.Models.PaymentType;
using ReservationType = CinemaluxAPI.DAL.CinemaluxCatalogue.Models.ReservationType;

namespace CinemaluxAPI.Service.Cinemalux.Contracts
{
    public interface ITypesService
    {
        #region Discount Type
        
        public GridData<DiscountType> GetAllDiscoutnTypeGrid(GridParams gridParams);
        public DiscountType GetDiscountType(string discountTypeCode);
        public DiscountType AddDiscountType(AddDiscountTypeDTO dto, Identity employee);
        public DiscountType ModifyDiscountType(string discountTypeCode, ModifyDiscountTypeDTO dto, Identity employee);
        public DiscountType ArchiveDiscountType(string discountTypeCode, Identity employee);
        
        #endregion
        
        #region Payment Type
        
        public GridData<PaymentType> GetAllPaymentTypeGrid(GridParams gridParams);
        public PaymentType[] GetAllPaymentTypes();
        public PaymentType GetPaymentType(string paymentTypeCode);
        public PaymentType AddPaymentType(AddPaymentTypeDTO dto, Identity employee);
        public PaymentType ModifyPaymentType(string paymentTypeCode, ModifyPaymentTypeDTO dto);
        public PaymentType ArchivePaymentType(string paymentTypeCode, Identity employee);
        
        #endregion
        
        #region Reservation Types
        
        public GridData<ReservationType> GetAllReservationTypeGrid(GridParams gridParams);
        public ReservationType GetReservationType(string reservationTypeCode);
        public ReservationType AddReservationType(AddReservationTypeDTO dto, Identity employee);
        public ReservationType ModifyReservationType(string reservationTypeCode, ModifyReservationTypeDTO dto, Identity employee);
        public ReservationType ArchiveReservationType(string reservationTypeCode, Identity employee);
        
        #endregion
        
        #region Order Types

        public OrderType[] GetAllOrderTypes();
        // public GridData<TicketType> GetAllTicketTypesGrid(GridParams gridParams);
        public OrderType GetOrderType(string orderTypeCode);
        public OrderType AddOrderType(AddOrderTypeDTO dto, Identity employee);
        public OrderType ModifyOrderType(string orderTypeCode, ModifyOrderTypeDTO dto, Identity employee);
        public OrderType ArchiveOrderType(string orderTypeCode, Identity employee);
        
        #endregion
        
        #region Ticket Types

        public GridData<TicketType> GetAllTicketTypesGrid(GridParams gridParams);
        public TicketType[] GetAllTicketTypes();
        public TicketType GetTicketTypes(string ticketTypeCode);
        public TicketType AddTicketType(AddTicketTypeDTO dto, Identity employee);
        public TicketType ModifyTicketType(string ticketTypeCode, ModifyTicketTypeDTO dto, Identity employee);
        public TicketType ArchiveTicketType(string ticketTypeCode, Identity employee);

        #endregion
        
        #region Genres

        public GridData<GenreDTO> GetAllGenresGrid(GridParams gridParams);
        public GenreDTO[] GetGenres();
        public GenreDTO GetGenre(string genreCode);
        public GenreDTO AddGenre(AddGenreDTO dto);
        public bool ArchiveGenre(string genreCode);

        #endregion
    }
}