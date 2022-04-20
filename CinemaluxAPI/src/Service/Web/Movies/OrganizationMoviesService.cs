using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CinemaluxAPI.Common;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.CinemaluxCatalogue;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
using Microsoft.EntityFrameworkCore;
using CinemaluxAPI.Service.Web.Contracts;
using CinemaluxAPI.Service.Web.Movies.DTO;
using CinemaluxAPI.DAL.OrganizationDbContext;
using CinemaluxAPI.DAL.OrganizationDbContext.Models;
using CinemaluxAPI.Multimedia;
using Movie = CinemaluxAPI.DAL.OrganizationDbContext.Models.Movie;

namespace CinemaluxAPI.Service.Web.Movies
{
    public class OrganizationMoviesService : IOrganizationMoviesService
    {
        #region Properties
        
        private CinemaluxDbContext DbContext { get; }
        private OrganizationDbContext OrganizationDbContext { get; }
        private IMultimediaService MultimediaService { get; }
        
        #endregion
        
        #region Constructor

        public OrganizationMoviesService(CinemaluxDbContext dbContext, OrganizationDbContext organizationDbContext, IMultimediaService multimediaService)
        {
            DbContext = dbContext;
            OrganizationDbContext = organizationDbContext;
            MultimediaService = multimediaService;
        }
        
        #endregion
        
        #region Action Methods
        
        public MovieDropdownDTO[] GetDropdownValues(GridParams gridParams)
        {
            return OrganizationDbContext.Movies
                .Include(x => x.MovieGenres)
                .Where(x => x.Title.Contains(gridParams.SQ))
                .Select(x => new MovieDropdownDTO
                {
                    Label = x.Title,
                    Value = x.Id
                }).ToArray();
        }
        
        public OrganizationMovieOverviewDTO[] GetAllMovies()
        {
            return
                OrganizationDbContext.Movies
                    .Include(x => x.MovieGenres)
                    .Select(x => new OrganizationMovieOverviewDTO
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        Genres = x.MovieGenres.Select(y => y.GenreCode).ToArray(),
                        BackdropImageURL = x.BackdropImageUrl,
                        ImageURL = x.ImageUrl,
                        AverageRating = x.MovieReviews.Select(y => y.Rating).DefaultIfEmpty().Average(),
                    }).ToArray();
        }

        public GridData<OrganizationVerboseMoviesDTO> GetAllVerboseMovies(GridParams gridParams)
        {
            var rows = OrganizationDbContext.Movies
                .Include(x => x.MovieGenres)
                .Select(x => new OrganizationVerboseMoviesDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Genres = x.MovieGenres.Select(y => y.GenreCode).ToArray(),
                    BackdropImageURL = x.BackdropImageUrl,
                    ImageURL = x.ImageUrl,
                    OverviewLinks = x.OverviewLinks,
                    VideoLinks = x.VideoLinks,
                    ReleaseYear = x.ReleaseYear,
                    AverageRating = x.MovieReviews.Select(y => y.Rating).DefaultIfEmpty().Average(),
                    RunningTimeInMinutes = x.RunningTimeInMinutes,
                    AgeRating = x.AgeRating,
                    ProfitPercentageShare = x.ProfitPercentageShare,
                    Has3D = x.Has3D,
                    HasLocalAudio = x.HasLocalAudio,
                    HasLocalSubtitles = x.HasLocalSubtitles,
                    CreatedAt = x.CreatedAt
                });

            if (gridParams.SQ.IsNotNull())
                rows = rows.Where(x => x.Title.ToLower().Contains(gridParams.SQ) || x.Description.ToLower().Contains(gridParams.SQ));
           
            return new GridData<OrganizationVerboseMoviesDTO>(rows, gridParams);
        }
        
        public OrganizationMovieInfoDTO GetMovie(short movieId)
        {
            IQueryable<Movie> movie = OrganizationDbContext.Movies.Where(x => x.Id == movieId);
            movie.EnsureNotNull("Movie not found");

            var screenings = DbContext.Screenings
                .Include(x => x.Reservations)
                .Include(x => x.Hall)
                .Include(x => x.Tickets)
                .Where(x =>
                    x.CinemaluxMovie.OMovieId == movieId)
                .ToArray();

            return movie
                .Include(x => x.MovieGenres)
                .Select(x => new OrganizationMovieInfoDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Genres = x.MovieGenres.Select(x => x.GenreCode).ToArray(),
                    BackdropImageURL = x.BackdropImageUrl,
                    ImageURL = x.ImageUrl,
                    OverviewLinks = x.OverviewLinks,
                    VideoLinks = x.VideoLinks,
                    ReleaseYear = x.ReleaseYear,
                    AverageRating = x.MovieReviews.Select(y => y.Rating).DefaultIfEmpty().Average(),
                    RunningTimeInMinutes = x.RunningTimeInMinutes,
                    AgeRating = x.AgeRating,
                    Has3D = x.Has3D,
                    HasLocalAudio = x.HasLocalAudio,
                    HasLocalSubtitles = x.HasLocalSubtitles,
                    Screenings = screenings.Select(y => new OrganizationMovieInfoDTO.ScreeningsInfo
                    {
                        Id = y.Id,
                        Date = y.Date,
                        Time = y.ScreeningTime,
                        Hall = y.Hall.Name,
                        Is3d = y.Is3D,
                        HasLocalAudio = y.HasLocalAudio,
                        HasSubtitles = y.HasLocalSubtitles,
                        Capacity = y.Hall.Capacity,
                        Reserved = y.Reservations.Count,
                        Booked = y.Tickets.Count
                    }).ToArray()
                }).FirstOrDefault();
        }

        public GridData<OrganizationMovieInfoDTO> GetMoviesScreenings(GridParams gridParams)
        {
            var movies = OrganizationDbContext.Movies
                .Select(x => x.Id).ToArray();

            var movieData = new List<OrganizationMovieInfoDTO> {};

            foreach (var movieId in movies)
                movieData.Add(this.GetMovie(movieId));

            return new GridData<OrganizationMovieInfoDTO>(movieData.AsQueryable(), gridParams);
        }
        public OrganizationMovieOverviewDTO[] GetLatestMovies()
        {
            return OrganizationDbContext.Movies.Select(x => new OrganizationMovieOverviewDTO
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Genres = x.MovieGenres.Select(x => x.GenreCode).ToArray(),
                ImageURL = x.ImageUrl,
                BackdropImageURL = x.BackdropImageUrl,
                AverageRating = x.MovieReviews.Select(y => y.Rating).DefaultIfEmpty().Average(),
                CreatedAt = x.CreatedAt
            }).OrderByDescending(x => x.CreatedAt).Take(6).ToArray();
        }
        
        public OrganizationMovieOverviewDTO[] GetBestRatedMovies()
        {
            return OrganizationDbContext.Movies.Select(x => new OrganizationMovieOverviewDTO
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Genres = x.MovieGenres.Select(x => x.GenreCode).ToArray(),
                ImageURL = x.ImageUrl,
                BackdropImageURL = x.BackdropImageUrl,
                AverageRating = x.MovieReviews.Select(y => y.Rating).DefaultIfEmpty().Average()
            }).OrderByDescending(x => x.AverageRating).Take(6).ToArray();
        }
        
        public OrganizationMovieOverviewDTO[] GetMoviesWithSimilarGenres(int currentMovieId, SimilarMoviesDTO dto)
        {

            var formattedGenres = new List<string>();
            
            foreach (var genre in dto.Genres)
                formattedGenres.Add(genre.ToLower());
            
            return OrganizationDbContext.Movies
                .Include(x => x.MovieGenres)
                .Where(x => 
                    x.Id != currentMovieId &&
                    x.MovieGenres.Select(y => y.GenreCode.ToLower()).ToArray().Any(z => formattedGenres.Contains(z)))
                .Select(x => new OrganizationMovieOverviewDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Genres = x.MovieGenres.Select(y => y.GenreCode).ToArray(),
                    BackdropImageURL = x.BackdropImageUrl,
                    ImageURL = x.ImageUrl,
                    AverageRating = x.MovieReviews.Select(y => y.Rating).DefaultIfEmpty().Average(),
                }).OrderBy(r => Guid.NewGuid()).Take(6).ToArray();
        } 
        
        public async Task<Movie> AddMovie(AddMovieDTO dto)
        {
            if (dto.AgeRating > 4) 
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid age rating");

            var coverImageUrl = "";
            if(dto.CoverImage != null) 
                coverImageUrl = (await MultimediaService.SaveImage(dto.CoverImage)).URL;
            
            var backdropImageUrl = "";
            if(dto.BackdropImage != null) 
                backdropImageUrl = (await MultimediaService.SaveImage(dto.BackdropImage)).URL;
            
            Movie movie = new Movie
            {
                Title = dto.Title,
                Description = dto.Description,
                OverviewLinks = dto.OverviewLinks,
                BackdropImageUrl = backdropImageUrl,
                ImageUrl = coverImageUrl,
                VideoLinks = dto.VideoLinks,
                ReleaseYear = dto.ReleaseYear,
                RunningTimeInMinutes = dto.RunningTimeInMinutes,
                AgeRating = dto.AgeRating,
                ProfitPercentageShare = dto.ProfitPercentageShare,
                Has3D = dto.Has3D,
                HasLocalAudio = dto.HasLocalAudio,
                HasLocalSubtitles = dto.HasLocalSubtitles,
            };
            
            OrganizationDbContext.Movies.Add(movie);
            await OrganizationDbContext.SaveChangesAsync();

            CinemaluxMovie cinemaluxMovieMirror = new CinemaluxMovie
            {
                OMovieId = movie.Id,
                Title = dto.Title,
                Description = dto.Description,
                OverviewLinks = dto.OverviewLinks,
                BackdropImageURL = backdropImageUrl,
                ImageURL = coverImageUrl,
                VideoLinks = dto.VideoLinks,
                ReleaseYear = dto.ReleaseYear,
                RunningTimeInMinutes = dto.RunningTimeInMinutes,
                AgeRating = dto.AgeRating.ToString(),
                ProfitPercentageShare = dto.ProfitPercentageShare,
                Has3D = dto.Has3D,
                HasLocalAudio = dto.HasLocalAudio,
                HasLocalSubtitles = dto.HasLocalSubtitles,
                Genres = dto.Genres
            };
            
            DbContext.Movies.Add(cinemaluxMovieMirror);
            await DbContext.SaveChangesAsync();

            if (dto.Genres.Length > 0)
            {
                try
                {
                    foreach (var genre in dto.Genres.Split(","))
                    {
                        var dbGenre = OrganizationDbContext.Genres.FirstOrDefault(x => x.Code == genre);
                        dbGenre.EnsureNotNull("Genre not found");

                        movie.MovieGenres.Add(new MovieGenre
                        {
                            MovieId = movie.Id,
                            GenreCode = dbGenre.Code
                        });
                    }

                    OrganizationDbContext.Update(movie);
                    await OrganizationDbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    OrganizationDbContext.Movies.Remove(movie);
                    await OrganizationDbContext.SaveChangesAsync();
                    DbContext.Movies.Remove(cinemaluxMovieMirror);
                    await OrganizationDbContext.SaveChangesAsync();
                    throw;
                }
            }

            return movie;
        }
        
        // public Movie UpdateMovie(short movieId, MovieDTO dto)
        // {
        //     var movie = DbContext.Movies.FirstOrDefault(x => x.Id == movieId);
        //     movie.EnsureNotNull("Error. Tried to update a non existing movie.");
        //
        //     if (dto.AgeRating < 0 || dto.AgeRating > 4) throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid age rating");
        //
        //     movie.Title = dto.Title;
        //     movie.Description = dto.Description;
        //     movie.OverviewLinks = dto.OverviewLinks;
        //     movie.ReleaseYear = dto.ReleaseYear;
        //     movie.RunningTimeInMinutes = dto.RunningTimeInMinutes;
        //     movie.AgeRating = dto.AgeRating;
        //     movie.HasLocalAudio = dto.HasLocalAudio;
        //     movie.HasLocalSubtitles = dto.HasLocalSubtitles;
        //     movie.ProfitPercentageShare = dto.ProfitPercentageShare;
        //     movie.VideoLinks = dto.VideoLinks;
        //     movie.BackdropImageURL = dto.BackdropImageURL;
        //     movie.ImageURL = dto.ImageURL;
        //     movie.Has3D = dto.Has3D;
        //    
        //     DbContext.Movies.Update(movie);
        //     DbContext.SaveChanges();
        //
        //     return movie;
        // }
        
        public bool ArchiveMovie(short movieId)
        {
            var movie = OrganizationDbContext.Movies.FirstOrDefault(x => x.Id == movieId);
            movie.EnsureNotNull("Movie not found");
        
            OrganizationDbContext.Archive(movie);
            OrganizationDbContext.SaveChanges();
        
            return true;
        }
        
        // public Movie DeleteMovie(short movieId)
        // {
        //     var movie = DbContext.Movies.FirstOrDefault(x => x.Id == movieId);
        //     movie.EnsureNotNull("Movie not found");
        //
        //     DbContext.Movies.Remove(movie);
        //     DbContext.SaveChanges();
        //
        //     return movie;
        // }
        
        #endregion
    }
}