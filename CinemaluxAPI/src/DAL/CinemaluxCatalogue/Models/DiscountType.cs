using System;
using System.Collections.Generic;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.CinemaluxCatalogue.Models
{
    public partial class DiscountType : ICreatableEntity, IModifiableEntity, IArchivableEntity
    {
        public DiscountType()
        {
            DiscountItems = new HashSet<DiscountItem>();
        }

        public string Code { get; set; }
        public string Description { get; set; }
        public double DiscountPct { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }

        public virtual ICollection<DiscountItem> DiscountItems { get; set; }
    }
}
