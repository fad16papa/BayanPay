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

        var Id = new Guid("ca77d082-e7cd-4947-a63f-60cd0abbd53e");

        var command = new DeleteUser.Command(Id);
        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedUser = await context.Users.FindAsync(Id);
        deletedUser.Should().BeNull();
    }
}