using CinemaluxAPI.Auth;
using CinemaluxAPI.Common;
using CinemaluxAPI.DAL.OrganizationDbContext.Models;
using CinemaluxAPI.Service.Web.MovieReviews.DTO;

namespace CinemaluxAPI.Service.Web.Contracts
{
    public interface IMovieReviewsService
    {
        public GridData<MovieReviewGridDTO> GetAllReviews(GridParams queryParams);
        public GetReviewDTO[] GetMovieReviews(short movieId);
        public MovieReview PostReview(Identity identity, PostReviewDTO dto);
        public bool ArchiveReview(int reviewId, Identity Employee);
    }
}