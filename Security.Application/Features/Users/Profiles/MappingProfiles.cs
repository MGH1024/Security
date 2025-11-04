using AutoMapper;
using System.Linq.Expressions;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using Security.Application.Features.Users.Commands.Create;
using Security.Application.Features.Users.Commands.Delete;
using Security.Application.Features.Users.Commands.Update;
using Security.Application.Features.Users.Queries.GetById;
using Security.Application.Features.Users.Queries.GetList;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Security.Application.Features.Users.Commands.UpdateFromAuth;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;

namespace Security.Application.Features.Users.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, CreateUserCommand>().ReverseMap();
        CreateMap<User, CreatedUserResponse>().ReverseMap();
        CreateMap<User, UpdateUserCommand>().ReverseMap();
        CreateMap<User, UpdatedUserResponse>().ReverseMap();
        CreateMap<User, UpdateUserFromAuthCommand>().ReverseMap();
        CreateMap<User, UpdatedUserFromAuthResponse>().ReverseMap();
        CreateMap<User, DeleteUserCommand>().ReverseMap();
        CreateMap<User, DeletedUserResponse>().ReverseMap();
        CreateMap<User, GetUserByIdResponse>().ReverseMap();
        CreateMap<User, GetListUserListItemDto>().ReverseMap();
        CreateMap<IPaginate<User>, GetListResponse<GetListUserListItemDto>>().ReverseMap();
        
        CreateMap<DeleteUserCommand, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt
                => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src.Id))).ReverseMap();


        CreateMap<UpdateUserCommand, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt
                => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src.Id))).ReverseMap();


        CreateMap<UpdateUserFromAuthCommand, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt
                => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src.Id))).ReverseMap();

        CreateMap<GetUserByIdQuery, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt
                => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src.Id))).ReverseMap();
    }
}