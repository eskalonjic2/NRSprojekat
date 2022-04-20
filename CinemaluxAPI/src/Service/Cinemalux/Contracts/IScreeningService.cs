using CinemaluxAPI.Auth;
using CinemaluxAPI.Common;
using CinemaluxAPI.Services.Screenings.DTO;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.Services
{
    public interface IScreeningService
    {
        public ScreeningDTO GetScreeningById(long screeningId);
        public GridData<FilteredScreeningsDTO> GetScreenings(ScreeningGridParams queryParams);
        public GridData<MovieScreeningsTimelineDTO> GetScreeningsTimeline(ScreeningGridParams queryParams);
        public Screening AddScreening(AddScreeningDTO dto, Identity employee);
        public Screening ModifyScreening(long screeningId, ModifyScreeningDTO dto, Identity employee);
        public bool ArchiveScreening(long screeningId, Identity employee);
        public bool DeleteScreening(long screeningId);
    }
}