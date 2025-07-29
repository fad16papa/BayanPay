using BayanPay.UserService.Domain;
using BayanPay.UserService.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BayanPay.UserService.Application.Commands;

public class DeleteUser
{
    public class Command : IRequest<AppUser>
    {
        public AppUser User { get; set; } // Primary key of the user to delete

        public Command(AppUser user)
        {
            User = user;
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User.Id).NotEmpty().WithMessage("Id is required.");
            }
        }

        public class Handler : IRequestHandler<Command, AppUser>
        {
            private readonly UserDbContext _userDbContext;

            public Handler(UserDbContext userDbContext)
            {
                _userDbContext = userDbContext;
            }


            public async Task<AppUser> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userDbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == request.User.Id, cancellationToken);

                if (user == null)
                {
                    throw new ArgumentNullException(nameof(request.User), "User cannot be null.");
                }

                _userDbContext.Users.Remove(user);
                await _userDbContext.SaveChangesAsync(cancellationToken);

                return user;
            }
        }
    }
}
