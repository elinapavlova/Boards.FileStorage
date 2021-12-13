using AutoMapper;
using Microsoft.AspNetCore.Http;
using FileResponseDto = Boards.FileStorageService.Core.Dto.File.FileResponseDto;

namespace Boards.FileStorageService.Core.Profiles
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