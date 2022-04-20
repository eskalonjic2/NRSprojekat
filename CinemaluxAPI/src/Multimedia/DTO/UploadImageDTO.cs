using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaluxAPI.Multimedia.DTO
{
    public class UploadFileDTO
    {
        [FromForm(Name="file")]
        public IFormFile File { get; set; }
    }
}