using System;
using CinemaluxAPI.Auth;
using CinemaluxAPI.Common;
using CinemaluxAPI.DAL.OrganizationDbContext.Models;
using CinemaluxAPI.Service.Web.Contracts;
using CinemaluxAPI.Service.Web.MovieReviews.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CinemaluxAPI.API.Controllers.Web
{
    [ApiController]
    [Route(("/movieReviews"))]
    public class MovieReviewsController : CinemaluxControllerBase
    {
        #region Properties
        private IMovieReviewsService MovieReviewsService { get; set; }

        #endregion

        #region Constructor
        
        public MovieReviewsController(IMovieReviewsService movieReviewsService)
        {
            MovieReviewsService = movieReviewsService;
        }
        
        #endregion
        
        #region Routes
        
        [HttpGet("all")]
        public ActionResult<GridData<MovieReviewGridDTO>> GetAllReviews([FromQuery] GridParams queryParams)
        {
            return Ok(MovieReviewsService.GetAllReviews(queryParams));
        }

        [HttpGet("web/movie/{movieId}")]
        public ActionResult<GetReviewDTO[]> GetMovieReviews([FromRoute] short movieId)
        {
            return Ok(MovieReviewsService.GetMovieReviews(movieId));
        } 
        
        [HttpPost("")]
        public ActionResult<MovieReview> PostReview([FromBody] PostReviewDTO dto)
        {
            return Ok(MovieReviewsService.PostReview(CurrentIdentity, dto));
        }

        [HttpDelete("archive/{reviewId}")]
        public ActionResult<MovieReview> Archive([FromRoute] int reviewId)
        {
            return Ok(MovieReviewsService.ArchiveReview(reviewId, CurrentIdentity));
        }
        
        #endregion
    }
}