namespace CinemaluxAPI.Services
{
    public class AddNewOrderItemDTO
    {
        public string OrderTypeCode { get; set; }
        public byte Quantity { get; set; } = 1;
        public string Creator { get; set; }
    }
}