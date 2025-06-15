using BayanPay.UserService.Api.Interfaces;
using BayanPay.UserService.Application.Users.Commands;
using BayanPay.UserService.Application.Users.Queries;
using BayanPay.UserService.Domain;
using BayanPay.UserService.Persistence;
using MediatR;

namespace BayanPay.UserService.Api.Services;

public class UserService : IUserService
{
    private readonly UserDbContext _dbContext;
    private readonly IMediator _mediator;

    public UserService(UserDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<AppUser> CreateUserAsync(AppUser appUser)
    {
        try
        {
            var result = await _mediator.Send(new CreateUser.Command(appUser));

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        try
        {
            var result = await _mediator.Send(new DeleteUser.Command(userId));

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<AppUser> GetUserByClerkIdAsync(string clerkUserId)
    {
        try
        {
            var result = await _mediator.Send(new GetUserByClerkId.Query(clerkUserId));

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<AppUser> GetUserByIdAsync(Guid userId)
    {
        try
        {
            var result = await _mediator.Send(new GetUserById.Query(userId));

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<AppUser> UpdateUserAsync(AppUser appUser)
    {
        try
        {
            var result = await _mediator.Send(new UpdateUser.Command(appUser));

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
}