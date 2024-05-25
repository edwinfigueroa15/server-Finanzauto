namespace ApiFinanzauto.Dtos
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public int? Length { get; set; }
    }
}
