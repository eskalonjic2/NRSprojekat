using System.Threading.Tasks;
using CinemaluxAPI.Multimedia.DTO;
using Microsoft.AspNetCore.Http;

namespace CinemaluxAPI.Multimedia
{
    public interface IMultimediaService
    {
        public Task<SuccessfulUploadDTO> SaveImage(IFormFile image);
        public bool DeleteImage(string filename);
        public Task<SuccessfulUploadDTO> SaveVideo(IFormFile video);
        public bool DeleteVideo(string filename);
    }
}