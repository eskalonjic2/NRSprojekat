using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaluxAPI.Service.Web.Movies.DTO
{
    public class AddMovieDTO
    {
        [FromForm(Name="title")]
        public string Title { get; set; }
        
        [FromForm(Name="description")]
        public string Description { get; set; }
        
        [FromForm(Name="genres")]
        public string Genres { get; set; }
        
        [FromForm(Name="coverImage")]
        public IFormFile CoverImage { get; set; }
        
        [FromForm(Name="backdropImage")]
        public IFormFile BackdropImage { get; set; }
        
        [FromForm(Name="videoLinks")]
        public string VideoLinks { get; set; }
        
        [FromForm(Name="overviewLinks")]
        public string OverviewLinks { get; set; }
        
        [FromForm(Name="releaseYear")]
        public string? ReleaseYear { get; set; }
        
        [FromForm(Name="runningTimeInMinutes")]
        public short RunningTimeInMinutes { get; set; }
        
        [FromForm(Name="ageRating")]
        public byte AgeRating { get; set; }
        
        [FromForm(Name="profitPercentageShare")]
        public byte ProfitPercentageShare { get; set; }

        [FromForm(Name = "has3D")] public bool Has3D { get; set; } = false;

        [FromForm(Name = "hasLocalAudio")] public bool HasLocalAudio { get; set; } = false;

        [FromForm(Name = "hasLocalSubtitles")] public bool HasLocalSubtitles { get; set; } = false;
    }
}