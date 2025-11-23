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
    [Route("api/v1/departamentos")]
    public class DepartamentosController : ControllerBase
    {
        private readonly EcoWorkDbContext _context;
        private readonly IMapper _mapper;

        public DepartamentosController(EcoWorkDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Max(pageSize, 1);

            var query = _context.Departamentos.AsNoTracking();

            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = items.Select(dep =>
            {
                var dto = _mapper.Map<DepartamentoResponseDto>(dep);
                dto.Links = HateoasHelper.BuildLinks(HttpContext, "api/v1/departamentos", dto.Id);
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
            var dep = await _context.Departamentos.FindAsync(id);

            if (dep == null)
                return NotFound();

            var dto = _mapper.Map<DepartamentoResponseDto>(dep);
            dto.Links = HateoasHelper.BuildLinks(HttpContext, "api/v1/departamentos", dto.Id);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartamentoCreateDto dto)
        {
            var entity = _mapper.Map<Departamento>(dto);

            _context.Departamentos.Add(entity);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<DepartamentoResponseDto>(entity);
            response.Links = HateoasHelper.BuildLinks(HttpContext, "api/v1/departamentos", response.Id);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DepartamentoCreateDto dto)
        {
            var dep = await _context.Departamentos.FindAsync(id);

            if (dep == null)
                return NotFound();

            dep.Nome = dto.Nome;

            await _context.SaveChangesAsync();

            var response = _mapper.Map<DepartamentoResponseDto>(dep);
            response.Links = HateoasHelper.BuildLinks(HttpContext, "api/v1/departamentos", response.Id);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dep = await _context.Departamentos.FindAsync(id);

            if (dep == null)
                return NotFound();

            _context.Departamentos.Remove(dep);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
