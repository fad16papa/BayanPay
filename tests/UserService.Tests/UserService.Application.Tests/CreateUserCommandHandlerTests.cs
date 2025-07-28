using BayanPay.UserService.Domain;
using BayanPay.UserService.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class CreateUserCommandHandlerTests
{
    private UserDbContext GetPostgresDbContext()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=userdb;Username=postgres;Password=yourpassword")
            .Options;

        var context = new UserDbContext(options);

        // var test = Dns.GetHostEntry("postgres"); // Ensure the DNS resolution works
        // // For test isolation
        // context.Database.EnsureDeleted(); // Optional: resets schema
        // context.Database.EnsureCreated(); // Recreates schema
        return context;
    }

    [Fact]
    public async Task Handle_ShouldAddUser_WhenUserIsValid()
    {
        // Arrange
        var context = GetPostgresDbContext();
        var handler = new CreateUser.Command.Handler(context);

        var user = new AppUser
        {
            ClerkUserId = "clerk_002",
            FirstName = "Mary",
            LastName = "Decena",
            Email = "mary@example.com",
            Address = "Manila",
            BirthDate = new DateOnly(1987, 11, 27),
            Role = "Admin",
            CreatedDateTime = DateTime.UtcNow,
            CreatedBy = "System"
        };

        var command = new CreateUser.Command(user);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Mary", result.FirstName);

        var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "mary@example.com");
        Assert.NotNull(savedUser);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentNullException_WhenUserIsNull()
    {
        // Arrange
        var context = GetPostgresDbContext();
        var handler = new CreateUser.Command.Handler(context);
        var command = new CreateUser.Command(null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
    }
}
