using System;
using CinemaluxAPI.Auth;
using CinemaluxAPI.Common;
using CinemaluxAPI.Services;
using Microsoft.AspNetCore.Mvc;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.API.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : CinemaluxControllerBase
    {
        #region Properties
        private IOrdersService OrdersService { get; }

        #endregion
        
        #region Constructor
        
        public OrderController(IOrdersService ordersService)
        {
            OrdersService = ordersService;
        } 

        #endregion
        
        #region Routes
        
        [HttpGet("grid")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult<OrderResponseDTO> GetOrders([FromQuery] GridParams gridParams)
        {
            return Ok(OrdersService.GetOrders(gridParams));
        }
        
        [HttpGet("{orderId}")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult<OrderResponseDTO> GetOrder([FromRoute] long orderId)
        {
            return Ok(OrdersService.GetOrder(orderId));
        }
        
        [HttpPost("")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult<Order> CreateOrder([FromBody] CreateOrderDTO dto)
        {
            return Ok(OrdersService.CreateOrder(dto, CurrentIdentity));
        }
        
        [HttpDelete("finalize/{orderId}")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult<Order> FinalizeOrder([FromRoute] long orderId)
        {
            return Ok(OrdersService.FinalizeOrder(orderId));
        }
        
        [HttpDelete("{orderId}")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult<Order> RemoveOrder([FromRoute] int orderId)
        {
            return Ok(OrdersService.RemoveOrder(orderId));
        }

        [HttpGet("{orderId}/orderItem/{orderItemId}")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult<OrderItem> GetOrderItem([FromRoute] int orderItemId)
        {
            return Ok(OrdersService.GetOrderItem(orderItemId));
        }
        
        [HttpPost("{orderId}/orderItem")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult<OrderItem> AddOrderItem([FromRoute] int orderId, [FromBody] AddNewOrderItemDTO dto)
        {
            return Created("Uspjesno kreirano",OrdersService.AddNewOrderItem(orderId, dto, CurrentIdentity));
        }
        
        [HttpPut("{orderId}/orderItem/{orderItemId}")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult<OrderItem> ModifyOrderItem([FromRoute] int orderId, [FromRoute] int orderItemId, [FromBody] ModifyOrderItemDTO itemDto)
        {
            return Ok(OrdersService.ModifyOrderItem(orderId, orderItemId, itemDto));
        }
        
        [HttpDelete("{orderId}/orderItem/{orderItemId}")]
        [Authority(Roles = "Administrator, Manager, Employee, Volunteer")]
        public ActionResult<bool> RemoveOrderItem([FromRoute] int orderId, [FromRoute] int orderItemId)
        {
            return Ok(OrdersService.RemoveOrderItem(orderId, orderItemId));
        }
        
        #endregion
    }
}