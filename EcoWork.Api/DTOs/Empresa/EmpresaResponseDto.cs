namespace EcoWork.Api.Dtos
{
    public class EmpresaResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;

        public Dictionary<string, string>? Links { get; set; }
    }
}