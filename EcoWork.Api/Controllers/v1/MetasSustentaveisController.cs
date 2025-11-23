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

            var result = items.Select(meta =>
            {
                var dto = _mapper.Map<MetaSustentavelResponseDto>(meta);
                dto.Links = HateoasHelper.BuildLinks(Url, "MetasSustentaveis", dto.Id);
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

        // GET /api/v1/metassustentaveis/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var meta = await _context.MetasSustentaveis.FindAsync(id);

            if (meta == null)
                return NotFound();

            var dto = _mapper.Map<MetaSustentavelResponseDto>(meta);
            dto.Links = HateoasHelper.BuildLinks(Url, "MetasSustentaveis", dto.Id);

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
            response.Links = HateoasHelper.BuildLinks(Url, "MetasSustentaveis", response.Id);

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
            response.Links = HateoasHelper.BuildLinks(Url, "MetasSustentaveis", response.Id);

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
    }
}