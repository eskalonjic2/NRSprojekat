using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.OrganizationDbContext;
using CinemaluxAPI.Multimedia.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CinemaluxAPI.Multimedia
{
    public class MultimediaService : IMultimediaService
    {
        #region Properties
        
        private OrganizationDbContext OrganizationDbContext { get; }
        private IWebHostEnvironment WebHostEnvironment { get; }

        #endregion
        
        #region Constructor

        public MultimediaService(OrganizationDbContext organizationDbContext, IWebHostEnvironment webHostEnvironment)
        {
            OrganizationDbContext = organizationDbContext;
            WebHostEnvironment = webHostEnvironment;
        }
        
        #endregion

        #region ActionMethods
        
        public async Task<SuccessfulUploadDTO> SaveImage(IFormFile image)
        {
            string extenstion = Path.GetExtension(image.FileName);

            if (extenstion.Equals(""))
                extenstion = ".jpg";
            
            string filename = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "") + extenstion;
            
            string filePath = Path.Combine(WebHostEnvironment.ContentRootPath, "src/Resources/Images/", filename);
            
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            
            return new SuccessfulUploadDTO
            {
                Filename = filename,
                Extension = extenstion,
                URL = $"http://localhost:5000/multimedia/images/{filename}"
            };
        }

        public bool DeleteImage(string filename)
        {
            if (!File.Exists(Path.Combine(WebHostEnvironment.ContentRootPath, "src/Resources/Images/", filename)))
                return false;
            
            File.Delete(Path.Combine(WebHostEnvironment.ContentRootPath, "src/Resources/Images/", filename));
            return true;
        }
        
        public async Task<SuccessfulUploadDTO> SaveVideo(IFormFile image)
        {
            string extenstion = Path.GetExtension(image.FileName);
            
            if (extenstion.Equals(""))
                extenstion = ".mp4";
            
            string filename = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "") + extenstion;
            
            string filePath = Path.Combine(WebHostEnvironment.ContentRootPath, "src/Resources/Videos/", filename);
            
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            
            return new SuccessfulUploadDTO
            {
                Filename = filename,
                Extension = extenstion,
                URL = $"http://localhost:5000/multimedia/videos/{filename}"
            };
        }

        public bool DeleteVideo(string filename)
        {
            if (!File.Exists(Path.Combine(WebHostEnvironment.ContentRootPath, "src/Resources/Videos/", filename)))
                return false;
            
            File.Delete(Path.Combine(WebHostEnvironment.ContentRootPath, "src/Resources/Videos/", filename));
            return true;
        }
        
        #endregion
    }
}