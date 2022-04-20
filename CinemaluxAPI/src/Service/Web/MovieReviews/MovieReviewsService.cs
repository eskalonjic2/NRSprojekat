using System.Linq;
using System.Net;
using CinemaluxAPI.Auth;
using CinemaluxAPI.Common;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
using CinemaluxAPI.DAL.OrganizationDbContext;
using CinemaluxAPI.DAL.OrganizationDbContext.Models;
using CinemaluxAPI.Service.Web.Contracts;
using CinemaluxAPI.Service.Web.MovieReviews.DTO;
using Movie = CinemaluxAPI.DAL.OrganizationDbContext.Models.Movie;

namespace CinemaluxAPI.Service.Web.MovieReviews
{
    public class MovieReviewsService : IMovieReviewsService
    {
        #region Properties
        
        private OrganizationDbContext OrganizationDbContext { get; }

        #endregion
    
        #region Constructor
        
        public MovieReviewsService(OrganizationDbContext organizationDbContext)
        {
            OrganizationDbContext = organizationDbContext;
        }
        
        #endregion
    
        #region Action Methods

        public GridData<MovieReviewGridDTO> GetAllReviews(GridParams queryParams)
        {
            IQueryable<MovieReview> query = OrganizationDbContext.MovieReviews;

            var rows = query.Select(x => new MovieReviewGridDTO
            {
                Id = x.Id,
                MovieId = x.MovieId,
                MovieTitle = x.Movie.Title,
                UserId = x.UserId,
                UserFullName = $"{x.User.Name} {x.User.Surname} [{x.User.Surname}]",
                Title = x.Title,
                Review = x.Review,
                Rating = x.Rating,
                CreatedAt = x.CreatedAt
            }).ToList();
            
            if (queryParams.SQ.IsNotNull())
                rows = rows.Where(x => x.MovieTitle.ToLower().Contains(queryParams.SQ) 
                                       || x.UserFullName.ToLower().Contains(queryParams.SQ)
                                       || x.Title.ToLower().Contains(queryParams.SQ)).ToList();

            return new GridData<MovieReviewGridDTO>(rows.AsQueryable(), queryParams);
        }

        public GetReviewDTO[] GetMovieReviews(short movieId)
        {
            return OrganizationDbContext.MovieReviews
                .Where(x => x.MovieId == movieId)
                .Select(x => new GetReviewDTO
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    UserFullName = $"{x.User.Name} {x.User.Surname}",
                    UserUsername = $"{x.User.Username}",
                    Title = x.Title,
                    Review = x.Review,
                    Rating = x.Rating,
                    CreatedAt = x.CreatedAt
                }).OrderByDescending(x => x.CreatedAt).ToArray();
        }

        public MovieReview PostReview(Identity identity, PostReviewDTO dto)
        {
            Movie movie = OrganizationDbContext.Movies.FirstOrDefault(x => x.Id == dto.MovieId);
            movie.EnsureNotNull("Movie not found");

            if (OrganizationDbContext.MovieReviews
                .FirstOrDefault(x =>
                    x.MovieId == dto.MovieId &&
                    x.UserId == identity.Id) != null)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "User already has a post");


            MovieReview movieReview = new MovieReview
            {
                MovieId = movie.Id,
                UserId = identity.Id,
                Title = dto.Title,
                Review = dto.Review,
                Rating = dto.Rating,
                CreatedBy = identity.Username
            };

            OrganizationDbContext.Add(movieReview);
            OrganizationDbContext.SaveChanges();

            return movieReview;
        }

        public bool ArchiveReview(int reviewId, Identity employee)
        {
            MovieReview review = OrganizationDbContext.MovieReviews.FirstOrDefault(x => x.Id == reviewId);
            review.EnsureNotNull("Review nije nadjen");

            OrganizationDbContext.MovieReviews.Archive(review, employee.Name);
            OrganizationDbContext.SaveChanges();

            return true;
        }
        
        #endregion
    }
}