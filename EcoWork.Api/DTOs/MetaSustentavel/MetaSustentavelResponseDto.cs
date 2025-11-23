namespace EcoWork.Api.Dtos
{
    public class MetaSustentavelResponseDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        public int EmpresaId { get; set; }
        public int Pontos { get; set; }

        public Dictionary<string, string>? Links { get; set; }
    }
}