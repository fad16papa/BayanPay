using BayanPay.UserService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BayanPay.UserService.Application.Users.Commands;

public class DeleteUser
{
    public class Command : IRequest<bool>
    {
        public Guid Id { get; set; }  // Primary key of the user to delete

        public Command(Guid id)
        {
            Id = id;
        }
    }

    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly UserDbContext _userDbContext;

        public Handler(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
                return false;

            _userDbContext.Users.Remove(user);
            await _userDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
