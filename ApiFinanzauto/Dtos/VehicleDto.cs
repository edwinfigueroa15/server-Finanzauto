namespace ApiFinanzauto.Dtos
{
    public class VehicleDto
    {
        public string Name { get; set; } = null!;
        public string Plate { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Line { get; set; } = null!;
        public string Year { get; set; } = null!;
        public string Kilimetres { get; set; } = null!;
        public string Cost { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int ClientId { get; set; }
        public bool Status { get; set; }
        public string? Observations { get; set; }
    }
}
