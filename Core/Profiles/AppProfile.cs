using AutoMapper;
using Microsoft.AspNetCore.Http;
using FileResponseDto = Core.Dto.File.FileResponseDto;

namespace Core.Profiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<IFormFile, FileResponseDto>()
                .ForMember("Name", opt => 
                    opt.MapFrom(f => f.FileName));
        }
    }
}