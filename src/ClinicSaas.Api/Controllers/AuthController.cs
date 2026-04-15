using ClinicSaas.Api.Contracts;
using ClinicSaas.Application.Auth.Commands;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSaas.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController(
    RegisterUserCommandHandler registerUserCommandHandler,
    LoginCommandHandler loginCommandHandler) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(request.Email, request.Password, request.FullName, request.Role);
        var result = await registerUserCommandHandler.HandleAsync(command, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(ApiResponseFactory.Failure(result.Errors));
        }

        return Ok(ApiResponseFactory.Success(new { userId = result.Value }));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await loginCommandHandler.HandleAsync(command, cancellationToken);
        if (!result.IsSuccess || result.Value is null)
        {
            return Unauthorized(ApiResponseFactory.Failure(result.Errors.Select(x => x.Message).ToArray()));
        }

        return Ok(ApiResponseFactory.Success(result.Value));
    }
}

public sealed record RegisterUserRequest(string Email, string Password, string FullName, string Role);
public sealed record LoginRequest(string Email, string Password);
