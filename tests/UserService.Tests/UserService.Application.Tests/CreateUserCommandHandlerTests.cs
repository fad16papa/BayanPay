using BayanPay.UserService.Domain;
using BayanPay.UserService.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class CreateUserCommandHandlerTests
{
    private UserDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Isolated DB for each test
            .Options;

        return new UserDbContext(options);
    }

    [Fact]
    public async Task Handle_ShouldAddUser_WhenUserIsValid()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var handler = new CreateUser.Command.Handler(context);

        var user = new AppUser
        {
            ClerkUserId = "clerk_001",
            FirstName = "Francis",
            LastName = "Decena",
            Email = "francis@example.com",
            Address = "Manila",
            BirthDate = new DateOnly(1990, 1, 1),
            Role = "Admin",
            CreatedDateTime = DateTime.UtcNow,
            CreatedBy = "System"
        };

        var command = new CreateUser.Command(user);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Francis", result.FirstName);

        var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "francis@example.com");
        Assert.NotNull(savedUser);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentNullException_WhenUserIsNull()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var handler = new CreateUser.Command.Handler(context);
        var command = new CreateUser.Command(null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
    }
}
