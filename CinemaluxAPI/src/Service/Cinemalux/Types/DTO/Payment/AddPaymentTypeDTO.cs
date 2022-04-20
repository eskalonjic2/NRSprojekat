namespace CinemaluxAPI.Services.Types.DTO
{
    public class AddPaymentTypeDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool AddToOrganization { get; set; } = false;
    }
}