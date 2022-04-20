using CinemaluxAPI.Common;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
using CinemaluxAPI.DAL.OrganizationDbContext.Models;
using CinemaluxAPI.Service.Cinemalux.Contracts;
using CinemaluxAPI.Services;
using Microsoft.AspNetCore.Mvc;
using CinemaluxAPI.Services.Discount.DTO;
using CinemaluxAPI.Services.Types;
using CinemaluxAPI.Services.Types.DTO;
using CinemaluxAPI.Services.Types.DTO.Genres;
using DiscountType = CinemaluxAPI.DAL.CinemaluxCatalogue.Models.DiscountType;
using PaymentType = CinemaluxAPI.DAL.CinemaluxCatalogue.Models.PaymentType;
using ReservationType = CinemaluxAPI.DAL.CinemaluxCatalogue.Models.ReservationType;

namespace CinemaluxAPI.API.Controllers
{
    [ApiController]
    [Route("types")]
    public class TypesController : CinemaluxControllerBase
    {
        #region Properties
        private ITypesService TypesService { get; set; }

        #endregion

        #region Constructor
        public TypesController(ITypesService typesService)
        { 
            TypesService = typesService;
        }
        
        #endregion
    
        #region Routes
        
        #region Discount Types
        
        [HttpGet("discount/grid")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult<GridData<DiscountType>> GetAllDiscountTypeGrid([FromQuery] GridParams gridParams)
        {
            return Ok(TypesService.GetAllDiscoutnTypeGrid(gridParams));
        }
        
        [HttpGet("discount/{discountTypeCode}")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult GetDiscountType([FromRoute] string discountTypeCode)
        {
            return Ok(TypesService.GetDiscountType(discountTypeCode));
        }
        
        [HttpPost("discount")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult AddDiscountType([FromBody] AddDiscountTypeDTO dto)
        {
            return Ok(TypesService.AddDiscountType(dto, CurrentIdentity));
        }

        [HttpPut("discount/{discountTypeCode}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult ModifyDiscountType([FromRoute] string discountTypeCode, [FromBody] ModifyDiscountTypeDTO dto)
        {
            return Ok(TypesService.ModifyDiscountType(discountTypeCode, dto, CurrentIdentity));
        }
        
        [HttpDelete("discount/{discountTypeCode}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult ArchiveDiscountType([FromRoute] string discountTypeCode)
        {
            return Ok(TypesService.ArchiveDiscountType(discountTypeCode, CurrentIdentity));
        }
        
        #endregion
        
        #region Order Types
        
        [HttpGet("order/all")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult GetAllOrderTypes()
        {
            return Ok(TypesService.GetAllOrderTypes());
        }
        
        [HttpGet("order/{discountTypeCode}")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult GetOrderType([FromRoute] string discountTypeCode)
        {
            return Ok(TypesService.GetOrderType(discountTypeCode));
        }
        
        [HttpPost("order")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult AddOrderType([FromBody] AddOrderTypeDTO dto)
        {
            return Ok(TypesService.AddOrderType(dto, CurrentIdentity));
        }

        [HttpPut("order/{discountTypeCode}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult ModifyOrderType([FromRoute] string discountTypeCode, [FromBody] ModifyOrderTypeDTO dto)
        {
            return Ok(TypesService.ModifyOrderType(discountTypeCode, dto, CurrentIdentity));
        }
        
        [HttpDelete("order/{discountTypeCode}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult ArchiveOrderType([FromRoute] string discountTypeCode)
        {
            return Ok(TypesService.ArchiveOrderType(discountTypeCode, CurrentIdentity));
        }
        
        #endregion
        
        #region Payment Type
        
        [HttpGet("payment/grid")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult<GridData<PaymentType>> GetAllPaymentGrid([FromQuery] GridParams gridParams)
        {
            return Ok(TypesService.GetAllPaymentTypeGrid(gridParams));
        }
        
        [HttpGet("payment/all")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult<PaymentType[]> GetAllPaymentTypes()
        {
            return Ok(TypesService.GetAllPaymentTypes());
        }
        
        [HttpGet("payment/{discountTypeCode}")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult GetPaymentType([FromRoute] string discountTypeCode)
        {
            return Ok(TypesService.GetPaymentType(discountTypeCode));
        }
        
        [HttpPost("payment")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult AddPaymentType([FromBody] AddPaymentTypeDTO dto)
        {
            return Ok(TypesService.AddPaymentType(dto, CurrentIdentity));
        }

        [HttpPut("payment/{discountTypeCode}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult ModifyPaymentType([FromRoute] string discountTypeCode, [FromBody] ModifyPaymentTypeDTO dto)
        {
            return Ok(TypesService.ModifyPaymentType(discountTypeCode, dto));
        }
        
        [HttpDelete("payment/{discountTypeCode}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult ArchivePaymentType([FromRoute] string discountTypeCode)
        {
            return Ok(TypesService.ArchivePaymentType(discountTypeCode, CurrentIdentity));
        }
        
        #endregion

        #region Genres

        [HttpGet("genre/grid")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult<GridData<GenreDTO>> GetGenresGrid([FromQuery] GridParams gridParams)
        {
            return Ok(TypesService.GetAllGenresGrid(gridParams));
            
        }
        [HttpGet("genre/all")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult GetAll()
        {
            return Ok(TypesService.GetGenres());
        }
        
        [HttpGet("{genreId}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult GetSingleGenre([FromRoute] string code)
        {
            return Ok(TypesService.GetGenre(code));
        }

        [HttpPost("genre")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult AddGenre([FromBody] AddGenreDTO dto)
        {
            return Ok(TypesService.AddGenre(dto));
        }
        
        [HttpDelete("genre/{code}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult DeleteGenre([FromRoute] string code)
        {
            return Ok(TypesService.ArchiveGenre(code));
        }
        
        #endregion
        
        #region Ticket Type
        
        [HttpGet("ticket/grid")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult<GridData<TicketType>> GetAllTicketsGrid([FromQuery] GridParams gridParams)
        {
            return Ok(TypesService.GetAllTicketTypesGrid(gridParams));
        }
        
        [HttpGet("ticket/all")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult GetAllTicketTypes()
        {
            return Ok(TypesService.GetAllTicketTypes());
        }
      
        [HttpPost("ticket")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult AddTicketType([FromBody] AddTicketTypeDTO dto)
        {
            return Ok(TypesService.AddTicketType(dto, CurrentIdentity));
        }
        
        [HttpDelete("ticket/{ticketTypeCode}")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult ArchiveTicketType([FromRoute] string ticketTypeCode)
        {
            return Ok(TypesService.ArchiveTicketType(ticketTypeCode, CurrentIdentity));
        }
        
        #endregion
        
        #region Reservation Type
        
        [HttpGet("reservation/grid")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult<GridData<ReservationType>> GetAllReservationsGrid([FromQuery] GridParams gridParams)
        {
            return Ok(TypesService.GetAllReservationTypeGrid(gridParams));
        }
        
        [HttpPost("reservation")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult AddReservationType([FromBody] AddReservationTypeDTO dto)
        {
            return Ok(TypesService.AddReservationType(dto, CurrentIdentity));
        }
        
        [HttpDelete("reservation/{reservationTypeCode}")]
        [Authority(Roles = "Administrator, Manager, Employee")]
        public ActionResult ArchiveReservationType([FromRoute] string reservationTypeCode)
        {
            return Ok(TypesService.ArchiveReservationType(reservationTypeCode, CurrentIdentity));
        }
        
        #endregion
        
        #endregion
        
    }
}