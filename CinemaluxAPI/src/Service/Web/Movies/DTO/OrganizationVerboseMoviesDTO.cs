using System;
using JetBrains.Annotations;

namespace CinemaluxAPI.Service.Web.Movies.DTO
{
    public class OrganizationVerboseMoviesDTO
    {
        public short Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Genres { get; set; }
        public string BackdropImageURL { get; set; }
        public string ImageURL { get; set; }
        public string OverviewLinks { get; set; }
        public string VideoLinks { get; set; }
        public string ReleaseYear { get; set; }
        public double AverageRating { get; set; }
        public int RunningTimeInMinutes { get; set; }
        public byte? AgeRating { get; set; }
        public float ProfitPercentageShare { get; set; }
        public bool Has3D { get; set; }
        public bool HasLocalAudio { get; set; }
        public bool HasLocalSubtitles { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}