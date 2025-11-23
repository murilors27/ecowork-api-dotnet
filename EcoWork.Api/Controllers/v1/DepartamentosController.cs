using AutoMapper;
using EcoWork.Api.Dtos;
using EcoWork.Api.Models;
using EcoWork.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoWork.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DepartamentosController : ControllerBase
    {
        private readonly EcoWorkDbContext _context;
        private readonly IMapper _mapper;

        public DepartamentosController(EcoWorkDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET /api/v1/departamentos?page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            var query = _context.Departamentos.AsNoTracking();

            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = items
                .Select(d =>
                {
                    var dto = _mapper.Map<DepartamentoResponseDto>(d);
                    dto = AddHateoasLinks(dto);
                    return dto;
                })
                .ToList();

            return Ok(new
            {
                currentPage = page,
                pageSize,
                totalItems = total,
                totalPages = (int)Math.Ceiling(total / (double)pageSize),
                data = result
            });
        }

        // GET /api/v1/departamentos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dep = await _context.Departamentos.FindAsync(id);

            if (dep == null)
                return NotFound();

            var dto = _mapper.Map<DepartamentoResponseDto>(dep);
            dto = AddHateoasLinks(dto);

            return Ok(dto);
        }

        // POST /api/v1/departamentos
        [HttpPost]
        public async Task<IActionResult> Create(DepartamentoCreateDto dto)
        {
            var entity = _mapper.Map<Departamento>(dto);

            _context.Departamentos.Add(entity);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<DepartamentoResponseDto>(entity);
            response = AddHateoasLinks(response);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, response);
        }

        // PUT /api/v1/departamentos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DepartamentoCreateDto dto)
        {
            var dep = await _context.Departamentos.FindAsync(id);

            if (dep == null)
                return NotFound();

            dep.Nome = dto.Nome;

            await _context.SaveChangesAsync();

            var response = _mapper.Map<DepartamentoResponseDto>(dep);
            response = AddHateoasLinks(response);

            return Ok(response);
        }

        // DELETE /api/v1/departamentos/{id}
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

        // MÉTODO AUXILIAR DE HATEOAS
        private DepartamentoResponseDto AddHateoasLinks(DepartamentoResponseDto dto)
        {
            var id = dto.Id;

            return new DepartamentoResponseDto
            {
                Id = dto.Id,
                Nome = dto.Nome,

                Links = new Dictionary<string, string>
                {
                    { "self", Url.Action(nameof(GetById), new { id })! },
                    { "update", Url.Action(nameof(Update), new { id })! },
                    { "delete", Url.Action(nameof(Delete), new { id })! }
                }
            };
        }
    }
}