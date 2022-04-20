using System;
using System.Collections.Generic;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.OrganizationDbContext.Models
{
    public partial class MovieReview : ICreatableEntity, IArchivableEntity
    {
        public int Id { get; set; }
        public short MovieId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Review { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ArchivedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual User User { get; set; }
    }
}
