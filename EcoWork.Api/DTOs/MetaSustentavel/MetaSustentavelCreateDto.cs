namespace EcoWork.Api.Dtos
{
    public class MetaSustentavelCreateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int EmpresaId { get; set; }
        public int Pontos { get; set; }
    }
}