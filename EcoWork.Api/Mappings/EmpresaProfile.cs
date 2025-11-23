using AutoMapper;
using EcoWork.Api.Dtos;
using EcoWork.Api.Models;

namespace EcoWork.Api.Mappings
{
    public class EmpresaProfile : Profile
    {
        public EmpresaProfile()
        {
            CreateMap<Empresa, EmpresaResponseDto>();
            CreateMap<EmpresaCreateDto, Empresa>();
        }
    }
}