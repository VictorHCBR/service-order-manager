using ServiceOrders.Application.Abstractions;
using ServiceOrders.Domain.Common;
using ServiceOrders.Domain.Entities.Users;
using ServiceOrders.Domain.Exceptions;
using ServiceOrders.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceOrders.Application.Auth;

public sealed class AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterUserRequest request,  CancellationToken token)
    {
        var email = Email.Create(request.Email);
        var existing = (await _unitOfWork.Users.GetAsync(u => u.Email == email, token)).SingleOrDefault();

        if (existing is not null)
            return Result<AuthResponse>.Failure("Já existe um usuário cadastrado com esse email.");

        var roles = NormalizeRoles(request.Roles).Select(r => new Role(r)).ToList();
        var user = new User(request.Name, email, _passwordHasher.Hash(request.Password), roles);

        await _unitOfWork.Users.AddAsync(user, token);
        await _unitOfWork.SaveChangesAsync(token);

        var jwt = _jwtTokenGenerator.Generate(user);

        return Result<AuthResponse>.Success(new AuthResponse(
            user.Id,
            user.Name,
            user.Email.Value,
            [..user.Roles.Select(r => r.Name)],
            jwt.AccessToken,
            jwt.ExpiresAt));

    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken token)
    {
        var email = Email.Create(request.Email);
        var user = (await _unitOfWork.Users.GetAsync(u => u.Email == email, token)).SingleOrDefault();

        if (user is null || !user.IsActive || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return Result<AuthResponse>.Failure("Usuário ou Senha Incorretos.");

        var jwt = _jwtTokenGenerator.Generate(user);
        return Result<AuthResponse>.Success(new AuthResponse(
            user.Id, 
            user.Name,
            user.Email.Value,
            [..user.Roles.Select(r => r.Name)],
            jwt.AccessToken,
            jwt.ExpiresAt));
    }

    private static string[] NormalizeRoles(string[]? roles)
    {
        if (roles is null || roles.Length == 0)
            return [RoleName.User];

        var normalized = roles.Select(r => r.Trim()).Where(r => r.Length > 0).Distinct().ToArray();

        foreach (var role in normalized)
            if (!RoleName.Profiles.Contains(role))
                throw new DomainException($"Perfil {role} inválido.");

        return normalized;
    }
}
