using System;
using System.Collections.Generic;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.OrganizationDbContext.Models
{
    public partial class Cinema : ICreatableEntity, IModifiableEntity, IArchivableEntity
    {
        public byte Id { get; set; }
        public byte CityId { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
        public string ContactPhone { get; set; }
        public string LocationAddress { get; set; }
        public byte? NumberOfHalls { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }

        public virtual City City { get; set; }
    }
}
