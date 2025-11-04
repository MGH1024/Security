using AutoMapper;
using System.Linq.Expressions;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using Security.Application.Features.OperationClaims.Commands.Create;
using Security.Application.Features.OperationClaims.Commands.Delete;
using Security.Application.Features.OperationClaims.Commands.Update;
using Security.Application.Features.OperationClaims.Queries.GetById;
using Security.Application.Features.OperationClaims.Queries.GetList;

namespace Security.Application.Features.OperationClaims.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<OperationClaim, CreateOperationClaimCommand>().ReverseMap();
        CreateMap<OperationClaim, CreatedOperationClaimResponse>().ReverseMap();
        CreateMap<OperationClaim, UpdateOperationClaimCommand>().ReverseMap();
        CreateMap<OperationClaim, UpdatedOperationClaimResponse>().ReverseMap();
        CreateMap<OperationClaim, DeleteOperationClaimCommand>().ReverseMap();
        CreateMap<OperationClaim, DeletedOperationClaimResponse>().ReverseMap();
        CreateMap<OperationClaim, GetByIdOperationClaimResponse>().ReverseMap();
        CreateMap<OperationClaim, GetListOperationClaimListItemDto>().ReverseMap();
        CreateMap<IPaginate<OperationClaim>, GetListResponse<GetListOperationClaimListItemDto>>().ReverseMap();
        
        CreateMap<DeleteOperationClaimCommand, GetModel<OperationClaim>>()
            .ForMember(dest => dest.Predicate, opt => opt.MapFrom(src => (Expression<Func<OperationClaim, bool>>)(u => u.Id == src.Id)))
            .ReverseMap();
    }
}
