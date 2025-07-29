using BayanPay.UserService.Domain;

namespace BayanPay.UserService.Api.Interfaces;
public interface IUserService
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation, containing the user details.</returns>
    Task<AppUser> GetUserByIdAsync(Guid userId);

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation, containing the user details.</returns>
    Task<AppUser> GetUserByClerkIdAsync(string clerkUserId);

    /// <summary>
    /// Creates a new user with the specified details.
    /// </summary>
    /// <param name="appUser">The details of the user to create.</param>
    /// <returns>A task that represents the asynchronous operation, containing the created user's details.</returns>
    Task<AppUser> CreateUserAsync(AppUser appUser);

    /// <summary>
    /// Updates an existing user's details.
    /// </summary>
    /// <param name="appUser">The updated details of the user.</param>
    /// <returns>A task that represents the asynchronous operation, containing the updated user's details.</returns>
    Task<AppUser> UpdateUserAsync(AppUser appUser);

    /// <summary>
    /// Deletes a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<AppUser> DeleteUserAsync(AppUser appUser);
}