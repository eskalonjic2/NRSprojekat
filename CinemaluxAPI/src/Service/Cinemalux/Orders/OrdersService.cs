using System;
using System.Linq;
using CinemaluxAPI.Auth;
using System.Collections.Generic;
using CinemaluxAPI.Common;
using Microsoft.EntityFrameworkCore;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.CinemaluxCatalogue;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
using CinemaluxAPI.src.Services.Ticket.DTO;

namespace CinemaluxAPI.Services
{
    public class OrdersService : IOrdersService
    {
        #region Properties
        private CinemaluxDbContext DbContext { get; }
        
        #endregion
        
        #region Constructor
        public OrdersService(CinemaluxDbContext context)
        {
            DbContext = context;
        } 

        #endregion

        #region Action Methods
        
        #region Order
        
        public GridData<OrderGridDTO> GetOrders(GridParams gridParams)
        {
            var rows = DbContext.Orders.Select(x => new OrderGridDTO
            {
                Id = x.Id,
                EmployeeName = x.Employee.Username,
                WasReserved = x.ReservationId != null,
                PaymentTypeCode = x.PaymentTypeCode,
                TotalTickets = x.Tickets.Count,
                TotalPrice = x.TotalPrice,
                CreatedAt = x.CreatedAt,
                FinalizedAt = x.ArchivedAt
            });

            return new GridData<OrderGridDTO>(rows, gridParams);
        }
        public OrderResponseDTO GetOrder(long orderId)
        {
            return (from order in DbContext.Orders
                    .Include(x => x.Tickets)
                    .ThenInclude(x => x.TicketType)
                    .Include(x => x.OrderItems)
                where order.Id == orderId
                select new OrderResponseDTO
                {
                    order = new OrderResponseDTO.OrderPreview
                    {
                        Id = order.Id,
                        ReservationId = order.ReservationId,
                        PaymentTypeCode = order.PaymentTypeCode,
                        TotalPrice = order.TotalPrice,
                        CreatedAt = order.CreatedAt,
                        CreatedBy = order.CreatedBy,
                        ModifiedAt = order.ModifiedAt,
                        ModifiedBy = order.ModifiedBy
                    },
                    tickets = order.Tickets.Select(ticket => new TicketDTO
                    {
                        Id = ticket.Id,
                        ScreeningId = ticket.ScreeningId,
                        MovieTitle = ticket.Screening.CinemaluxMovie.Title,
                        OrderId = ticket.OrderId,
                        ReservationId = ticket.ReservationId,
                        TicketTypeCode = ticket.TicketTypeCode,
                        TicketPrice = ticket.TicketType.Price,
                        SeatLabel = ticket.SeatLabel
                    }).ToArray(),
                    orderItems = order.OrderItems.Select(orderItem => new OrderItemResponseDTO
                    {
                        Id = orderItem.Id,
                        OrderTypeCode = orderItem.OrderTypeCode,
                        OrderItemName = orderItem.OrderType.Name,
                        OrderItemPrice = orderItem.OrderType.Price,
                        Quantity = orderItem.Quantity,
                        CreatedAt = orderItem.CreatedAt,
                    }).ToArray()
                }).FirstOrDefault();
        }
        public Order CreateOrder(CreateOrderDTO dto, Identity employee)
        {
            
            Order newOrder = new Order
            {
                EmployeeId = employee.Id,
                ReservationId = dto.ReservationId == -1 ? null : dto.ReservationId,
                PaymentTypeCode = dto.PaymentTypeCode,
                CreatedBy = $"{employee.Name} {employee.Surname}"
            };

            DbContext.Add(newOrder);
            DbContext.SaveChanges();
            
            return newOrder;
        }
        public bool RemoveOrder(long orderId)
        {
            Order order = GetFullOrderById(orderId);
            order.EnsureNotNull("Order nije nadjen");
            
            foreach(OrderItem orderItem in order.OrderItems)
                DbContext.OrderItems.Remove(orderItem);
            

            DbContext.Orders.Remove(order);
            DbContext.SaveChanges();
       
            DbContext.SaveChanges();

            return true;
        }
        public Order FinalizeOrder(long orderId)
        {
            Order order = GetFullOrderById(orderId);
            order.EnsureNotNull("Order nije nadjen");
    
            foreach(OrderItem orderItem in order.OrderItems)
                DbContext.OrderItems.Archive(orderItem);

            foreach (Ticket ticket in order.Tickets)
                order.TotalPrice += ticket.TicketType.Price;

            DbContext.Orders.Archive(order);

            if (order.ReservationId != null && order.ReservationId != -1)
                DbContext.Reservations.Archive(DbContext.Reservations.FirstOrDefault(x => x.Id == order.ReservationId));

            DbContext.SaveChanges();
            return order;
        }
        
        #endregion

        #region Order Items

        public OrderItem GetOrderItem(long orderItemId)
        {
            return DbContext.OrderItems.Include(x => x.OrderType)
                .FirstOrDefault(x => x.Id == orderItemId);
        }
        public OrderItemResponseDTO AddNewOrderItem(long orderId, AddNewOrderItemDTO dto, Identity employee)
        {
            Order order = DbContext.Orders.FirstOrDefault(x => x.Id == orderId);
            order.EnsureNotNull("Order ne postoji");
            
            OrderType orderType = DbContext.OrderTypes.FirstOrDefault(x => x.Code.Equals(dto.OrderTypeCode));
            orderType.EnsureNotNull("Order ne postoji");

            OrderItem orderItem = new OrderItem
            {
                OrderId = orderId,
                OrderTypeCode = dto.OrderTypeCode,
                Quantity = dto.Quantity,
                CreatedBy = employee.Username,
                ModifiedBy = dto.Creator
            };

            order.OrderItems.Add(orderItem);
            order.ModifiedBy = employee.Username;
            order.TotalPrice += orderType.Price * orderItem.Quantity;

            DbContext.Orders.Update(order);
            DbContext.SaveChanges();

            return new OrderItemResponseDTO
            {
                Id = orderItem.Id,
                OrderTypeCode = orderItem.OrderTypeCode,
                OrderItemName = orderType.Name,
                OrderItemPrice = orderType.Price,
                CreatedAt = orderItem.CreatedAt
            };
        }
        public OrderItemResponseDTO ModifyOrderItem(long orderId, long orderItemId, ModifyOrderItemDTO dto)
        {
            Order order = GetFullOrderById(orderId);
            order.EnsureNotNull("Order nije nadjen");

            OrderItem orderItem = order.OrderItems.FirstOrDefault(x => x.Id == orderItemId);
            orderItem.EnsureNotNull("order nema");
            
            OrderType orderType = DbContext.OrderTypes.FirstOrDefault(x => x.Code.Equals(dto.OrderTypeCode));
            orderType.EnsureNotNull("order nema");

            orderItem.OrderTypeCode = dto.OrderTypeCode ?? orderItem.OrderTypeCode;
            orderItem.Quantity = dto.Quantity ?? orderItem.Quantity;
            orderItem.ModifiedBy = dto.ModifiedBy ?? orderItem.ModifiedBy;

            DbContext.SaveChanges();

            order.ModifiedBy = orderItem.ModifiedBy;
            order.TotalPrice = CalculateNewTotalPrice(order.OrderItems, order.Tickets, order.DiscountItems);

            DbContext.Orders.Update(order);
            DbContext.SaveChanges();

            return new OrderItemResponseDTO
            {
                Id = orderItem.Id,
                OrderTypeCode = orderItem.OrderTypeCode,
                OrderItemName = orderItem.OrderType.Name,
                OrderItemPrice = orderItem.OrderType.Price,
                Quantity = orderItem.Quantity,
                CreatedAt = orderItem.CreatedAt
            };
        }
        public bool RemoveOrderItem(long orderId, long orderItemId)
        {
            Order order = GetFullOrderById(orderId);
            order.EnsureNotNull("Order nije nadjen");
            
            OrderItem orderItem = order.OrderItems.FirstOrDefault(x => x.Id == orderItemId);
            orderItem.EnsureNotNull("Id itema nevalja");

            order.TotalPrice -= orderItem.OrderType.Price * orderItem.Quantity;
            
            DbContext.Remove(orderItem);
            DbContext.Orders.Update(order);
            DbContext.SaveChanges();

            return true;
        }

        #endregion

        #endregion
        
        #region Private Methods

        private Order GetFullOrderById(long orderId)
        {
            return DbContext.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.OrderType)
                .Include(x => x.Tickets)
                .ThenInclude(x => x.TicketType)
                .FirstOrDefault(x => x.Id == orderId);    
        }
        
        private double CalculateNewTotalPrice(ICollection<OrderItem> orderItems, ICollection<Ticket> tickets, ICollection<DiscountItem> discountItems)
        {
            double priceSum = orderItems.Sum(x => x.OrderType.Price * x.Quantity) + tickets.Sum(x => x.TicketType.Price);
            double discountPct = discountItems.Sum(x => x.DiscountType.DiscountPct);
            
            return priceSum - ((discountPct / priceSum) * 100);
        }
        
        #endregion
    }
}