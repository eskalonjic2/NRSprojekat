using CinemaluxAPI.Auth;
using System.Collections.Generic;
using CinemaluxAPI.Common;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.Services
{
    public interface IOrdersService
    {
        #region Order
        
        public GridData<OrderGridDTO> GetOrders(GridParams gridParams);
        public OrderResponseDTO GetOrder(long orderId);
        public Order CreateOrder(CreateOrderDTO dto, Identity employee);
        public Order FinalizeOrder(long orderId);
        public bool RemoveOrder(long orderId);
        
        #endregion
       
        #region Order Item
        
        public OrderItem GetOrderItem(long orderItemId);
        public OrderItemResponseDTO AddNewOrderItem(long orderId, AddNewOrderItemDTO dto, Identity employee);
        public OrderItemResponseDTO ModifyOrderItem(long orderId, long orderItemId, ModifyOrderItemDTO itemDto);
        public bool RemoveOrderItem(long orderId, long orderItemId);
        
        #endregion
    }
}