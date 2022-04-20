using CinemaluxAPI.Auth;
using CinemaluxAPI.Common;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
using CinemaluxAPI.Service.Halls.DTO;

namespace CinemaluxAPI.Service.Contracts
{
    public interface IHallService
    {
        public GridData<Hall> GetAllGrid(GridParams gridParams);
        public Hall[] GetAll();
        public Hall GetHall(byte hallId);
        public Hall AddHall(AddHallDTO dto, Identity employee);
        public Hall ModifyHall(byte hallId, ModifyHallDTO dto);
        public bool ArchiveHall(byte hallId, Identity employee);
    }
}