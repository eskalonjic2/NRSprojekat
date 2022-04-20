using System;
using System.Collections.Generic;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.CinemaluxCatalogue.Models
{
    public partial class CinemaluxMovie : ICreatableEntity, IModifiableEntity, IArchivableEntity
    {
        public CinemaluxMovie()
        {
            Screenings = new HashSet<Screening>();
        }

        public short Id { get; set; }
        public short OMovieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genres { get; set; }
        public string BackdropImageURL { get; set; }
        public string ImageURL { get; set; }
        public string OverviewLinks { get; set; }
        public string VideoLinks { get; set; }
        public string ReleaseYear { get; set; }
        public short RunningTimeInMinutes { get; set; }
        public string AgeRating { get; set; }
        public byte ProfitPercentageShare { get; set; }
        public double AverageRating { get; set; }
        public bool? Has3D { get; set; }
        public bool? HasLocalAudio { get; set; }
        public bool HasLocalSubtitles { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }

        public virtual ICollection<Screening> Screenings { get; set; } 
    }
}
