namespace CinemaluxAPI.Services.Discount.DTO
{
    public class AddDiscountItemDTO
    {
        public long? OrderId { get; set; }
        public string DiscountTypeCode { get; set; }
    }
}
