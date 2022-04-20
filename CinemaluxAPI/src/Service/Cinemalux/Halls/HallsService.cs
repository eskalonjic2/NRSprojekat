using System.Linq;
using System.Net;
using CinemaluxAPI.Auth;
using CinemaluxAPI.Common;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.CinemaluxCatalogue;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
using CinemaluxAPI.Service.Contracts;
using CinemaluxAPI.Service.Halls.DTO;

namespace CinemaluxAPI.Service.Halls
{
    public class HallsService : IHallService
    {
        #region Properties

        private CinemaluxDbContext DbContext { get; }

        #endregion

        #region Constructor

        public HallsService(CinemaluxDbContext dbContext)
        {
            DbContext = dbContext;
        }

        #endregion
        
        #region Action Methods
        
        public GridData<Hall> GetAllGrid(GridParams gridParams)
        {
            var rows = DbContext.Halls.Select(x => x);

            if (gridParams.SQ.IsNotNull())
                rows = rows.Where(x => x.Name.Contains(gridParams.SQ));

            return new GridData<Hall>(rows, gridParams);
        }
        
        public Hall[] GetAll()
        {
            return DbContext.Halls.ToArray();
        }
        
        public Hall GetHall(byte hallId)
        {
            return DbContext.Halls.FirstOrDefault(x => x.Id == hallId);
        }

        public Hall AddHall(AddHallDTO dto, Identity employee)
        {
            var sameHall = DbContext.Halls.FirstOrDefault(x => x.Name.Equals(dto.Name));
            if (sameHall != null)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Ime sale vec postoji");
            
            Hall hall = new Hall
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                SeatValidityRegex = dto.SeatValidityRegex,
                CreatedBy = employee.Id.ToString()
            };

            DbContext.Halls.Add(hall);
            DbContext.SaveChanges();

            return hall;
        }

        public Hall ModifyHall(byte hallId, ModifyHallDTO dto)
        {
            Hall hall = DbContext.Halls.FirstOrDefault(x => x.Id == hallId); 
            hall.EnsureNotNull("Hall not found");

            hall.Name = dto.Name;
            hall.Capacity = dto.Capacity;
            hall.SeatValidityRegex = dto.SeatValidityRegex;

            DbContext.Halls.Update(hall);
            DbContext.SaveChanges();

            return hall;
        }

        public bool ArchiveHall(byte hallId, Identity employee)
        {
            Hall hall = DbContext.Halls.FirstOrDefault(x => x.Id == hallId);
            hall.EnsureNotNull("Hall not found");
        
            DbContext.Halls.Archive(hall, employee.Username);
            DbContext.SaveChanges();

            return true;
        }
        
        #endregion
    }
}