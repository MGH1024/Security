using AutoMapper;
using MGH.Core.Application.Requests;
using Security.Application.Features.Auth.Commands.Login;
using Security.Application.Features.Users.Queries.GetById;
using Security.Application.Features.Users.Queries.GetList;
using Security.Application.Features.Auth.Commands.Register;
using Security.Application.Features.Auth.Commands.RevokeToken;
using Security.Application.Features.Auth.Commands.RefreshToken;

namespace Security.Endpoint.Api.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<LoginCommandDto, LoginCommand>()
            .ForCtorParam("LoginCommandDto", opt =>
                opt.MapFrom(src => src));

        CreateMap<PageRequest, GetListUserQuery>()
            .ForCtorParam("PageRequest", opt =>
                opt.MapFrom(src => src));

        CreateMap<string, RefreshTokenCommand>()
            .ConstructUsing(src => new RefreshTokenCommand(src));

        CreateMap<RegisterCommandDto, RegisterCommand>()
            .ForCtorParam("RegisterCommandDto", opt
                => opt.MapFrom(src => src));

        CreateMap<int, GetUserByIdQuery>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

        CreateMap<string, RevokeTokenCommand>()
            .ConstructUsing(src => new RevokeTokenCommand(src));
    }
}