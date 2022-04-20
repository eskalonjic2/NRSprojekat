using System;
using System.Collections.Generic;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.OrganizationDbContext.Models
{
    public partial class Movie : ICreatableEntity, IModifiableEntity, IArchivableEntity
    {
        public Movie()
        {
            MovieGenres = new HashSet<MovieGenre>();
            MovieReviews = new HashSet<MovieReview>();
        }

        public short Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BackdropImageUrl { get; set; }
        public string ImageUrl { get; set; }
        public string OverviewLinks { get; set; }
        public string VideoLinks { get; set; }
        public string ReleaseYear { get; set; }
        public short RunningTimeInMinutes { get; set; }
        public byte? AgeRating { get; set; }
        public byte ProfitPercentageShare { get; set; }
        public bool Has3D { get; set; }
        public bool HasLocalAudio { get; set; }
        public bool HasLocalSubtitles { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; } = "SYSTEM";

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<MovieReview> MovieReviews { get; set; }
    }
}
