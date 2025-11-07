using Security.Application.Features.Auth.Commands.Login;
using Security.Test.Application.Base;

namespace Security.Test.Application.Features.Auth.Commands.Login;

public static class LoginCommandHandlerFactory
{
    public static  LoginCommandHandler GetLoginCommandHandler(HandlerTestsFixture fixture)
    {
        return new LoginCommandHandler(fixture.MockUnitOfWork.Object,fixture.MockAuthService.Object,
            fixture.MockAuthBusinessRules.Object);
    }
}