using AutoMapper;
using ReactorTwinAPI.Domain.Entities;
using ReactorTwinAPI.Features.ReactorTwins.Dtos;

namespace ReactorTwinAPI.Features.ReactorTwins.Mapping
{
    public class ReactorTwinProfile : Profile
    {
        public ReactorTwinProfile()
        {
            CreateMap<CreateReactorTwinDto, ReactorTwin>();
            CreateMap<UpdateReactorTwinDto, ReactorTwin>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            CreateMap<ReactorTwin, ReactorTwinDto>();
        }
    }
}
