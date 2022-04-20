using System;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.CinemaluxCatalogue.Models
{
    public partial class DiscountItem : ICreatableEntity, IModifiableEntity, IArchivableEntity
    {
        public int Id { get; set; }
        public long? OrderId { get; set; }
        public string DiscountTypeCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }

        public virtual DiscountType DiscountType { get; set; }
        public virtual Order Order { get; set; }
    }
}
