using BayanPay.UserService.Domain;
using BayanPay.UserService.Persistence;
using Microsoft.EntityFrameworkCore;

public class UpdateUserCommandHandlerTests
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
        var handler = new UpdateUser.Command.Handler(context);

        var user = new AppUser
        {
            Id = new Guid("737c2c42-e843-49df-a196-69f80880cc6b"),
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

        var command = new UpdateUser.Command(user);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Mary", result.FirstName);

        var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "mary@example.com");
        Assert.NotNull(savedUser);
    }
}