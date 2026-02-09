using System.Linq;
using AutoMapper;
using DatApp.Dtos;
using DatApp.Models;

namespace DatApp.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Photo, PhotoForDetailDto>();

            // Mapping for the list view
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl,
                    opt => opt.MapFrom(src =>
                        src.Photos != null
                            ? src.Photos.FirstOrDefault(p => p.IsMain) != null
                                ? src.Photos.FirstOrDefault(p => p.IsMain).Url
                                : null
                            : null))
                .ForMember(dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            // Mapping for the detailed view
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl,
                    opt => opt.MapFrom(src =>
                        src.Photos != null
                            ? src.Photos.FirstOrDefault(p => p.IsMain) != null
                                ? src.Photos.FirstOrDefault(p => p.IsMain).Url
                                : null
                            : null))
                .ForMember(dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            // Mapping for profile updates
            CreateMap<UserForUpdateDto, User>();
        }
    }
}

