namespace CinemaluxAPI.Services.Discount.DTO
{
    public class ModifyDiscountItemDTO
    {
        public long? OrderId { get; set; }
        public string DiscountTypeCode { get; set; }
    }
}