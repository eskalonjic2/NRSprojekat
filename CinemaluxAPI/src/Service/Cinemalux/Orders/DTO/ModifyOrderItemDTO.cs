#nullable enable

namespace CinemaluxAPI.Services
{
    public class ModifyOrderItemDTO
    {
        public string? OrderTypeCode { get; set; }
        public byte? Quantity { get; set; } = 1;
        public string? ModifiedBy { get; set; }
    }
}