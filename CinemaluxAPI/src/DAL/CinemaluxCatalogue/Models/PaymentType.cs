using System;
using System.Collections.Generic;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.CinemaluxCatalogue.Models
{
    public partial class PaymentType : ICreatableEntity, IArchivableEntity
    {
        public PaymentType()
        {
            Orders = new HashSet<Order>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
