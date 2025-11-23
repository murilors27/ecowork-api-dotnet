using AutoMapper;
using EcoWork.Api.Dtos;
using EcoWork.Api.Models;

namespace EcoWork.Api.Mappings
{
    public class DepartamentoProfile : Profile
    {
        public DepartamentoProfile()
        {
            CreateMap<Departamento, DepartamentoResponseDto>();
            CreateMap<DepartamentoCreateDto, Departamento>();
        }
    }
}