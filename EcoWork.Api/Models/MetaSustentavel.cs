namespace EcoWork.Api.Models
{
    public class MetaSustentavel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        public int EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }

        public int Pontos { get; set; }
    }
}