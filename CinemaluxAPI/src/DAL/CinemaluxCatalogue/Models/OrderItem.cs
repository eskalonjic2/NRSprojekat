using System;
using System.Collections.Generic;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.CinemaluxCatalogue.Models
{
    public partial class OrderItem : ICreatableEntity, IModifiableEntity, IArchivableEntity
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string OrderTypeCode { get; set; }
        public byte Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }

        public virtual Order Order { get; set; }
        public virtual OrderType OrderType { get; set; }
    }
}
