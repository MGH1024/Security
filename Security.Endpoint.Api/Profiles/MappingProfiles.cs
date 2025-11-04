using AutoMapper;
using MGH.Core.Application.Requests;
using Security.Application.Features.Users.Queries.GetById;
using Security.Application.Features.Users.Queries.GetList;
using Security.Application.Features.Auth.Commands.UserLogin;
using Security.Application.Features.Auth.Commands.RevokeToken;
using Security.Application.Features.Auth.Commands.RegisterUser;
using Security.Application.Features.Auth.Commands.RefreshToken;

namespace Security.Endpoint.Api.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserLoginCommandDto, UserLoginCommand>()
            .ForCtorParam("UserLoginCommandDto", opt =>
                opt.MapFrom(src => src));

        CreateMap<PageRequest, GetListUserQuery>()
            .ForCtorParam("PageRequest", opt =>
                opt.MapFrom(src => src));

        CreateMap<string, RefreshTokenCommand>()
            .ConstructUsing(src => new RefreshTokenCommand(src));

        CreateMap<RegisterUserCommandDto, RegisterUserCommand>()
            .ForCtorParam("RegisterUserCommandDto", opt
                => opt.MapFrom(src => src));

        CreateMap<int, GetUserByIdQuery>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

        CreateMap<string, RevokeTokenCommand>()
            .ConstructUsing(src => new RevokeTokenCommand(src));
    }
}