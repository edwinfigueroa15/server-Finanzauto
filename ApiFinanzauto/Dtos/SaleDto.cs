namespace ApiFinanzauto.Dtos
{
    public class SaleDto
    {
        public string Name { get; set; } = null!;
        public string Document { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int VehicleId { get; set; }
        public bool Status { get; set; }
    }
}