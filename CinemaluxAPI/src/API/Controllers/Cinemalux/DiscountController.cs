using CinemaluxAPI.Common;
using CinemaluxAPI.Services;
using Microsoft.AspNetCore.Mvc;
using CinemaluxAPI.Services.Discount.DTO;

namespace CinemaluxAPI.API.Controllers
{
    [ApiController]
    [Route("discounts")]
    public class DiscountController: CinemaluxControllerBase
    {
        #region Properties

        private IDiscountService DiscountService { get; }

        #endregion

        #region Constructor

        public DiscountController(IDiscountService discountService)
        {
            DiscountService = discountService;
        }

        #endregion
        
        #region Routes

        [HttpPost("addDiscountItem")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult AddDiscountItem([FromBody] AddDiscountItemDTO dto)
        {
            return Ok(DiscountService.AddDiscountItem(dto));
        }

        [HttpPut("modifyDiscountItem/{discountItemId}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult ModifyDiscountItem([FromRoute] int discountItemId, [FromBody] ModifyDiscountItemDTO dto)
        {
            return Ok(DiscountService.ModifyDiscountItem(discountItemId, dto));
        }
        
        [HttpPost("archiveDiscountItem/{discountItemId}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult ArchiveDiscountItem([FromRoute] int discountItemId)
        {
            return Ok(DiscountService.ArchiveDiscountItem(discountItemId));
        }

        [HttpDelete("deleteDiscountItem/{discountItemId}")]
        [Authority(Roles = "Administrator, Manager")]
        public ActionResult DeleteDiscountItem([FromRoute] int discountItemId)
        {
            return Ok(DiscountService.DeleteDiscountItem(discountItemId));
        }

        #endregion
    }
}
