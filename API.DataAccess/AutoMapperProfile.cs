using API.DataAccess.Application;
using API.Dto;
using AutoMapper;

namespace API.DataAccess
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Connection, ConnectionDto>().ReverseMap();
            CreateMap<ConnectionList, ConnectionListDto>().ReverseMap();
            CreateMap<ConnectionDetail, ConnectionDetailDto>().ReverseMap();
        }
    }
}
