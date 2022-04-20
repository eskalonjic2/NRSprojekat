using System;
using System.IO;
using System.Net;
using CinemaluxAPI.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CinemaluxAPI.Multimedia.DTO;
using Microsoft.AspNetCore.Hosting;
using CinemaluxAPI.Common.Extensions;

namespace CinemaluxAPI.Multimedia
{
    [ApiController]
    [Route("multimedia")]
    public class MultimediaController : CinemaluxControllerBase
    {
        #region Properties

        private IMultimediaService MultimediaService { get; }
        private IWebHostEnvironment WebHostEnvironment { get; }
        
        #endregion
        
        #region Constructor

        public MultimediaController(IMultimediaService multimediaService, IWebHostEnvironment webHostEnvironment)
        {
            MultimediaService = multimediaService;
            WebHostEnvironment = webHostEnvironment;
        }
        
        #endregion
        
        #region Routes
        
        [HttpGet("images/{filename}")]
        public ActionResult GetImage([FromRoute] string fileName)
        {
            string path = Path.Combine(WebHostEnvironment.ContentRootPath, "src/Resources/Images/", fileName);
            return File(System.IO.File.ReadAllBytes(path), MimeTypes.GetMimeType(fileName));
        }
        
        [HttpPost("images")]
        public async Task<ActionResult<SuccessfulUploadDTO>> UploadImage([FromForm] UploadFileDTO dto)
        { 
            if (MimeTypes.GetMimeType(dto.File.FileName).StartsWith("image/"))
            {
                return Ok(await MultimediaService.SaveImage(dto.File));
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "File is not a image or it's mime type is bad");
            }
        }
        
        [HttpDelete("images/{filename}")]
        public ActionResult<bool> DeleteImage([FromRoute] string fileName)
        { 
            if (HttpContext.Request.Headers["Content-Type"].ToString().StartsWith("image/"))
            {
                return MultimediaService.DeleteImage(fileName);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "File is not a image or it's mime type is bad");
            }
        }
        
        [HttpGet("videos/{filename}")]
        public ActionResult GetVideo([FromRoute] string fileName)
        {
            string path = Path.Combine(WebHostEnvironment.ContentRootPath, "src/Resources/Images/", fileName);
            return File(System.IO.File.ReadAllBytes(path), MimeTypes.GetMimeType(fileName));
        }
        
        [HttpPost("videos")]
        public async Task<ActionResult<SuccessfulUploadDTO>> UploadVideo([FromForm] UploadFileDTO dto)
        { 
            Console.WriteLine(dto.File);
            if (MimeTypes.GetMimeType(dto.File.FileName).StartsWith("video/"))
            {
                return Ok(await MultimediaService.SaveVideo(dto.File));
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "File is not a video or it's mime type is bad");
            }
        }

        [HttpDelete("videos/{filename}")] 
        public ActionResult<bool> DeleteVideo([FromRoute] string fileName)
        { 
            if (HttpContext.Request.Headers["Content-Type"].ToString().StartsWith("video/"))
            {
                return MultimediaService.DeleteVideo(fileName);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "File is not a video or it's mime type is bad");
            }
        }
        
        #endregion
        
    }
}