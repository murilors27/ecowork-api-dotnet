using EcoWork.Api.Models;
using EcoWork.Api.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace EcoWork.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/seed")]
    public class SeedController : ControllerBase
    {
        private readonly EcoWorkDbContext _context;

        public SeedController(EcoWorkDbContext context)
        {
            _context = context;
        }

        [HttpPost("empresa")]
        public async Task<IActionResult> SeedEmpresa()
        {
            // Se já existir, não cria novamente
            if (_context.Empresas.Any())
                return Ok("Já existe empresa cadastrada.");

            var emp = new Empresa
            {
                Nome = "Empresa Teste",
                Cnpj = "12345678000199"
            };

            _context.Empresas.Add(emp);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Empresa criada com sucesso!",
                emp.Id,
                emp.Nome,
                emp.Cnpj
            });
        }
    }
}