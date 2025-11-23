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
    public class MetasSustentaveisController : ControllerBase
    {
        private readonly EcoWorkDbContext _context;
        private readonly IMapper _mapper;

        public MetasSustentaveisController(EcoWorkDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET /api/v1/metassustentaveis?page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            var query = _context.MetasSustentaveis.AsNoTracking();

            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = items
                .Select(meta =>
                {
                    var dto = _mapper.Map<MetaSustentavelResponseDto>(meta);
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

        // GET /api/v1/metassustentaveis/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var meta = await _context.MetasSustentaveis.FindAsync(id);

            if (meta == null)
                return NotFound();

            var dto = _mapper.Map<MetaSustentavelResponseDto>(meta);
            dto = AddHateoasLinks(dto);

            return Ok(dto);
        }

        // POST /api/v1/metassustentaveis
        [HttpPost]
        public async Task<IActionResult> Create(MetaSustentavelCreateDto dto)
        {
            var entity = _mapper.Map<MetaSustentavel>(dto);

            _context.MetasSustentaveis.Add(entity);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<MetaSustentavelResponseDto>(entity);
            response = AddHateoasLinks(response);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, response);
        }

        // PUT /api/v1/metassustentaveis/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MetaSustentavelCreateDto dto)
        {
            var meta = await _context.MetasSustentaveis.FindAsync(id);

            if (meta == null)
                return NotFound();

            meta.Titulo = dto.Titulo;
            meta.Descricao = dto.Descricao;
            meta.EmpresaId = dto.EmpresaId;
            meta.Pontos = dto.Pontos;

            await _context.SaveChangesAsync();

            var response = _mapper.Map<MetaSustentavelResponseDto>(meta);
            response = AddHateoasLinks(response);

            return Ok(response);
        }

        // DELETE /api/v1/metassustentaveis/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var meta = await _context.MetasSustentaveis.FindAsync(id);

            if (meta == null)
                return NotFound();

            _context.MetasSustentaveis.Remove(meta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // HATEOAS
        private MetaSustentavelResponseDto AddHateoasLinks(MetaSustentavelResponseDto dto)
        {
            var id = dto.Id;

            return new MetaSustentavelResponseDto
            {
                Id = dto.Id,
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                EmpresaId = dto.EmpresaId,
                Pontos = dto.Pontos,
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