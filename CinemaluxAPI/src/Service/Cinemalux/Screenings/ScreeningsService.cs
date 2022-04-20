using System;
using System.Linq;
using System.Net;
using CinemaluxAPI.Auth;
using System.Globalization;
using CinemaluxAPI.Common;
using Microsoft.EntityFrameworkCore;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.CinemaluxCatalogue;
using CinemaluxAPI.Services.Screenings.DTO;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.Services.Screenings
{
    public class ScreeningsService : IScreeningService
    {
        #region Properties
        private CinemaluxDbContext DbContext { get; }
        
        #endregion

        #region Constructor

        public ScreeningsService(CinemaluxDbContext context)
        {
            DbContext = context;
        }
        
        #endregion
        
        #region Action Methods
        
        public GridData<FilteredScreeningsDTO> GetScreenings(ScreeningGridParams queryParams)
        {
            IQueryable<Screening> query = DbContext.Screenings;

            if (queryParams.StartDate.IsNotNull())
            {
                if (!queryParams.EndDate.IsNotNull())
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "End date not specified");
                
                DateTime startDate = DateTime.ParseExact(queryParams.StartDate, queryParams.DateFormat, CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(queryParams.EndDate, queryParams.DateFormat, CultureInfo.InvariantCulture);
          
                query = query.Where(x => x.Date >= startDate && x.Date <= endDate);
            } else if (queryParams.EndDate.IsNotNull())
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Start date not specified");
            
            if (queryParams.SQ.IsNotNull())
            {
                query = query.Where(x => x.CinemaluxMovie.Title.Contains(queryParams.SQ));
            }
            
            IQueryable<FilteredScreeningsDTO> rows = 
                from screening in query
                select new FilteredScreeningsDTO
                {
                    Id = screening.Id,
                    MovieTitle = screening.CinemaluxMovie.Title,
                    HallName = screening.Hall.Name,
                    DefaultTicketTypeCode = screening.DefaultTicketTypeCode,
                    Capacity = screening.Hall.Capacity,
                    BookedSeats = screening.Tickets.Select(x => x.SeatLabel).ToArray(),
                    Date = screening.Date,
                    Time = screening.ScreeningTime,
                    Is3D = screening.Is3D,
                    HasLocalAudio = screening.HasLocalAudio,
                    HasLocalSubtitles = screening.HasLocalSubtitles,
                    CreatedAt = screening.CreatedAt
                };
            
            return new GridData<FilteredScreeningsDTO>(rows, queryParams);
        }
        
        public ScreeningDTO GetScreeningById(long screeningId)
        {
            return DbContext.Screenings.Where(x => x.Id == screeningId).Select(x => new ScreeningDTO
            {
                Id = x.Id,
                MovieId = x.MovieId,
                MovieTitle = x.CinemaluxMovie.Title,
                HallId = x.HallId,
                DefaultTicketTypeCode = x.DefaultTicketTypeCode,
                Date = x.Date,
                ScreeningTime = x.ScreeningTime,
                HasLocalAudio = x.HasLocalAudio,
                HasLocalSubtitles = x.HasLocalSubtitles,
                Is3D = x.Is3D
            }).FirstOrDefault();
        }

        public GridData<MovieScreeningsTimelineDTO> GetScreeningsTimeline(ScreeningGridParams queryParams)
        {
            IQueryable<Screening> query = DbContext.Screenings;

            if (queryParams.StartDate.IsNotNull())
            {
                if (!queryParams.EndDate.IsNotNull())
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "End date not specified");
                
                DateTime startDate = DateTime.ParseExact(queryParams.StartDate, queryParams.DateFormat, CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(queryParams.EndDate, queryParams.DateFormat, CultureInfo.InvariantCulture);
                
                query = query.Where(x => x.Date >= startDate && x.Date <= endDate);
            } else if (queryParams.EndDate.IsNotNull())
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Start date not specified");

            var halls = query.Select(x => x.Hall);
            
            IQueryable<MovieScreeningsTimelineDTO> rows = 
                from hall in halls
                select new MovieScreeningsTimelineDTO
                {
                    HallId = hall.Id,
                    HallName = hall.Name,
                    Screenings = query.Where(x => x.HallId == hall.Id)
                        .Select(screening => new MovieScreeningsTimelineDTO.ScreeningTimelineDTO
                    {
                        Id = screening.Id,
                        MovieTitle = screening.CinemaluxMovie.Title,
                        Date = screening.Date,
                        StartTime = screening.ScreeningTime,
                        EndTime = screening.ScreeningTime.Add(TimeSpan.FromMinutes(screening.CinemaluxMovie.RunningTimeInMinutes))
                    }).ToArray()
                };
      
            return new GridData<MovieScreeningsTimelineDTO>(rows.AsEnumerable().DistinctBy(x => x.HallId).AsQueryable(), queryParams);
        }
        
        public Screening AddScreening(AddScreeningDTO dto, Identity employee)
        {
            // DbContext.Halls.FirstOrDefault(x => x.Id == dto.HallId).EnsureNotNull("hala ne postoji");
            // DbContext.Movies.FirstOrDefault(x => x.Id == dto.MovieId).EnsureNotNull("film ne postoji");
            // DbContext.TicketTypes.FirstOrDefault(x => x.Code.Equals(dto.DefaultTicketTypeCode)).EnsureNotNull("karta ne postoji");

            Screening screening = new Screening
            {
                MovieId = dto.MovieId,
                HallId = dto.HallId,
                DefaultTicketTypeCode = dto.DefaultTicketTypeCode,
                Date = dto.Date,
                HasLocalAudio = dto.HasLocalAudio,
                HasLocalSubtitles = dto.HasLocalSubtitles,
                Is3D = dto.Has3D,
                ScreeningTime = dto.ScreeningTime,
                CreatedBy = employee.Name
            };

            DbContext.Screenings.Add(screening);
            DbContext.SaveChanges();

            return screening;
        }
   
        public Screening ModifyScreening(long screeningId, ModifyScreeningDTO dto, Identity employee)
        {
            Screening screening = DbContext.Screenings.FirstOrDefault(x => x.Id == screeningId);
            screening.EnsureNotNull("screening ne postoji");

            if (screening.HallId != dto.HallId)
            {
                DbContext.Halls.FirstOrDefault(x => x.Id == dto.HallId).EnsureNotNull("hala ne postoji");
                screening.HallId = dto.HallId;
            }

            if (screening.MovieId != dto.MovieId)
            {
                DbContext.Movies.FirstOrDefault(x => x.Id == dto.MovieId).EnsureNotNull("film ne postoji");
                screening.MovieId = dto.MovieId;
            }

            if (screening.DefaultTicketTypeCode != dto.DefaultTicketTypeCode)
            {
                DbContext.TicketTypes.FirstOrDefault(x => x.Code.Equals(dto.DefaultTicketTypeCode)).EnsureNotNull("karta ne postoji");
                screening.DefaultTicketTypeCode = dto.DefaultTicketTypeCode;
            }

            screening.Date = dto.Date;
            screening.ScreeningTime = dto.ScreeningTime;
            screening.ModifiedBy = employee.Name;

            DbContext.Screenings.Update(screening);
            DbContext.SaveChanges();

            return screening;
        }
     
        public bool ArchiveScreening(long screeningId, Identity employee)
        {
            Screening screening = DbContext.Screenings.FirstOrDefault(x => x.Id == screeningId);
            screening.EnsureNotNull("screening ne postoji");

            DbContext.Screenings.Archive(screening, employee.Name);
            DbContext.SaveChanges();
            
            return true;
        }
   
        public bool DeleteScreening(long screeningId)
        {
            Screening screening = DbContext.Screenings.FirstOrDefault(x => x.Id == screeningId);
            screening.EnsureNotNull("screening ne postoji");

            DbContext.Screenings.Remove(screening);
            DbContext.SaveChanges();
            
            return true;
        }
        
        #endregion
    }
}