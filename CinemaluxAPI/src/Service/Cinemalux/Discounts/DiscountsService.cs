using System.Net;
using System.Linq;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.Services.Discount.DTO;
using CinemaluxAPI.DAL.CinemaluxCatalogue;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.Services
{
    public class DiscountService : IDiscountService
    {
        #region Properties

        private CinemaluxDbContext DbContext { get; }

        #endregion

        #region Constructor

        public DiscountService(CinemaluxDbContext dbContext)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Action Methods
        public DiscountItem AddDiscountItem(AddDiscountItemDTO dto)
        {
            var discountItem = DbContext.DiscountItems.FirstOrDefault(x =>  x.DiscountTypeCode == dto.DiscountTypeCode);

            if(discountItem != null) 
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Discount item already exists");

            DiscountItem newDiscountItem = new DiscountItem
            {
                OrderId = dto.OrderId,
                DiscountTypeCode = dto.DiscountTypeCode
            };

            DbContext.DiscountItems.Add(newDiscountItem);
            DbContext.SaveChanges();


            return newDiscountItem;
        }
        public DiscountItem ModifyDiscountItem(int discountItemId, ModifyDiscountItemDTO dto)
        {
            var discountItem = DbContext.DiscountItems.FirstOrDefault(x => x.Id == discountItemId);
            discountItem.EnsureNotNull("Discount item not found");

            discountItem.DiscountTypeCode = dto.DiscountTypeCode;
            
            DbContext.DiscountItems.Update(discountItem);
            DbContext.SaveChanges();

            return discountItem;
        }
        public DiscountItem ArchiveDiscountItem(int discountTypeId)
        {
            var discountItem = DbContext.DiscountItems.FirstOrDefault(x => x.Id == discountTypeId);
            discountItem.EnsureNotNull("Discount item not found");

            DbContext.DiscountItems.Archive(discountItem);
            DbContext.SaveChanges();

            return discountItem;
        }
        public DiscountItem DeleteDiscountItem(int discountTypeId)
        {
            var discountItem = DbContext.DiscountItems.FirstOrDefault(x => x.Id == discountTypeId);
            discountItem.EnsureNotNull("Discount item not found");

            DbContext.Remove(discountItem);
            DbContext.SaveChanges();

            return discountItem;
        }

        #endregion
    }
}
