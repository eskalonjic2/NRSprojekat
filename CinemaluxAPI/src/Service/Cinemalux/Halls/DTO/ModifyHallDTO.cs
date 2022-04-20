namespace CinemaluxAPI.Service.Halls.DTO
{
    public class ModifyHallDTO
    {
        public string Name { get; set; }
        public byte Capacity { get; set; }
        public string SeatValidityRegex { get; set; }
    }
}