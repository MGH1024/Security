using MediatR;
using AutoMapper;
using Security.Domain;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using MGH.Core.Application.Pipelines.Authorization;
using Security.Application.Features.Users.Constants;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;

namespace Security.Application.Features.Users.Queries.GetList;

[Roles(UsersOperationClaims.GetUsers)]
public class GetListUserQuery(PageRequest pageRequest)
    : IRequest<GetListResponse<GetListUserListItemDto>>
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetListUserQuery() : this(new PageRequest { PageIndex = 0, PageSize = 10 })
    {
    }
}

public class GetListUserQueryHandler(IUow uow, IMapper mapper) : IRequestHandler<GetListUserQuery, GetListResponse<GetListUserListItemDto>>
{
    public async Task<GetListResponse<GetListUserListItemDto>> Handle(GetListUserQuery request, CancellationToken cancellationToken)
    {
        var getUserListModel = mapper.Map<GetListModelAsync<User>>(request);
        var users = await uow.User.GetListAsync(getUserListModel,cancellationToken);
        return mapper.Map<GetListResponse<GetListUserListItemDto>>(users);
    }
}