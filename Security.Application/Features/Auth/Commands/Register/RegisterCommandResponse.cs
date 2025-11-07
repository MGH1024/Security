namespace Security.Application.Features.Auth.Commands.Register;

public record RegisterCommandResponse(
    string Token,
    DateTime TokenExpiry,
    string RefreshToken,
    DateTime RefreshTokenExpiry);

