using BayanPay.UserService.Application.Interfaces;
using BayanPay.UserService.Domain;
using BayanPay.UserService.Persistence;

namespace BayanPay.UserService.Application.Services;

public class UserService : IUserService
{
    private readonly UserDbContext _dbContext;

    public UserService(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppUser> CreateUserAsync(AppUser appUser)
    {
        try
        {
            if (appUser == null)
            {
                throw new ArgumentNullException(nameof(appUser), "User is null");
            }

            await _dbContext.Users.AddAsync(appUser);
            return await _dbContext.SaveChangesAsync().ContinueWith(t => appUser);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        try
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            _dbContext.Users.Remove(user); 
            await _dbContext.SaveChangesAsync();
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
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            return user;
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
            if (appUser == null)
            {
                throw new ArgumentNullException(nameof(appUser), "User is null");
            }

            _dbContext.Users.Update(appUser);
            return await _dbContext.SaveChangesAsync().ContinueWith(t => appUser);
        }
        catch (Exception)
        {
            throw;
        }
    }
}