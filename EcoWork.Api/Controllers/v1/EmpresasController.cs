using AutoMapper;
using EcoWork.Api.Dtos;
using EcoWork.Api.Models;
using EcoWork.Api.Persistence;
using EcoWork.Api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoWork.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/empresas")]
    public class EmpresasController : ControllerBase
    {
        private readonly EcoWorkDbContext _context;
        private readonly IMapper _mapper;

        public EmpresasController(EcoWorkDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Max(pageSize, 1);

            var query = _context.Empresas.AsNoTracking();

            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = items.Select(emp =>
            {
                var dto = _mapper.Map<EmpresaResponseDto>(emp);
                dto.Links = HateoasHelper.BuildLinks(HttpContext, "api/v1/empresas", dto.Id);
                return dto;
            }).ToList();

            return Ok(new
            {
                currentPage = page,
                pageSize,
                totalItems = total,
                totalPages = (int)Math.Ceiling(total / (double)pageSize),
                data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null)
                return NotFound();

            var dto = _mapper.Map<EmpresaResponseDto>(empresa);
            dto.Links = HateoasHelper.BuildLinks(HttpContext, "api/v1/empresas", dto.Id);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmpresaCreateDto dto)
        {
            var entity = _mapper.Map<Empresa>(dto);

            _context.Empresas.Add(entity);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<EmpresaResponseDto>(entity);
            response.Links = HateoasHelper.BuildLinks(HttpContext, "api/v1/empresas", response.Id);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EmpresaCreateDto dto)
        {
            var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null)
                return NotFound();

            empresa.Nome = dto.Nome;
            empresa.Cnpj = dto.Cnpj;

            await _context.SaveChangesAsync();

            var response = _mapper.Map<EmpresaResponseDto>(empresa);
            response.Links = HateoasHelper.BuildLinks(HttpContext, "api/v1/empresas", response.Id);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null)
                return NotFound();

            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}