using CinemaluxAPI.Services.Discount.DTO;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.Services
{
    public interface IDiscountService
    {
        public DiscountItem AddDiscountItem(AddDiscountItemDTO dto);
        public DiscountItem ModifyDiscountItem(int discountItemId, ModifyDiscountItemDTO dto);
        public DiscountItem ArchiveDiscountItem(int discountTypeId);
        public DiscountItem DeleteDiscountItem(int discountTypeId);
    }
}
