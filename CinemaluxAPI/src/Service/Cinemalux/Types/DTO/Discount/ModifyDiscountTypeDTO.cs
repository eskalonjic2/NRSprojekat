using System;

namespace CinemaluxAPI.Services.Types.DTO
{
    public class ModifyDiscountTypeDTO
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public double DiscountPct { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}