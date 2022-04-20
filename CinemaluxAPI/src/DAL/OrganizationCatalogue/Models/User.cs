using System;
using System.Collections.Generic;
using CinemaluxAPI.Auth;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.OrganizationDbContext.Models
{
    public partial class User : Identity, ICreatableEntity, IModifiableEntity, IArchivableEntity
    {
        public User()
        {
            MovieReviews = new HashSet<MovieReview>();
        }
        public bool IsLocked { get; set; }
        public string Password { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ArchivedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }

        public virtual ICollection<MovieReview> MovieReviews { get; set; }
    }
}
