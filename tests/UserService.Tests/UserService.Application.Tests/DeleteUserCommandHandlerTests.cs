using BayanPay.UserService.Application.Commands;
using BayanPay.UserService.Application.Users.Queries;
using BayanPay.UserService.Domain;
using BayanPay.UserService.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class DeleteUserCommandHandlerTests
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
    public async Task Handle_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var context = GetPostgresDbContext();
        var handler = new DeleteUser.Command.Handler(context);

        var Id = new Guid("3ea22ceb-096e-4459-81dc-b7ac441530d8");

        var command = new DeleteUser.Command(Id);
        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedUser = await context.Users.FindAsync(Id);
        deletedUser.Should().BeNull();
    }
}