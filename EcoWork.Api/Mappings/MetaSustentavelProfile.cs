using AutoMapper;
using EcoWork.Api.Dtos;
using EcoWork.Api.Models;

namespace EcoWork.Api.Mappings
{
    public class MetaSustentavelProfile : Profile
    {
        public MetaSustentavelProfile()
        {
            CreateMap<MetaSustentavel, MetaSustentavelResponseDto>();
            CreateMap<MetaSustentavelCreateDto, MetaSustentavel>();
        }
    }
}