namespace CinemaluxAPI.Services.Types.DTO
{
    public class AddTicketTypeDTO
    {
        public string Code { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool AddToOrganization { get; set; } = false;
    }
}