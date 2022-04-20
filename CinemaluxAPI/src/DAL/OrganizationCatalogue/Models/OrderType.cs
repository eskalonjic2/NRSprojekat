using System;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.OrganizationDbContext.Models
{
    public partial class OrderType : ICreatableEntity, IModifiableEntity, IArchivableEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }
    }
}
