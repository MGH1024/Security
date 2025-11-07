using AutoMapper;
using System.Linq.Expressions;
using Security.Application.Features.Auth.Commands.Register;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Security.Application.Features.Auth.Commands.RevokeToken;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;

namespace Security.Application.Features.Auth.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RefreshToken, RevokedTokenResponse>().ReverseMap();

        CreateMap<RegisterCommand, User>()
            .ForMember(d => d.Email, src =>
                src.MapFrom(a => a.RegisterCommandDto.Email))
            .ForMember(d => d.FirstName, src =>
                src.MapFrom(a => a.RegisterCommandDto.FirstName))
            .ForMember(d => d.LastName, src =>
                src.MapFrom(a => a.RegisterCommandDto.LastName))
            .ReverseMap();

        CreateMap<string, GetBaseModel<User>>()
            .ForMember(dest => dest.Predicate, opt => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Email == src)))
            .ForMember(dest => dest.EnableTracking, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.WithDeleted, opt => opt.Ignore());

        CreateMap<int, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src)))
            .ForMember(dest => dest.EnableTracking, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.WithDeleted, opt => opt.Ignore());
    }
}