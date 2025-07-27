using BayanPay.UserService.Domain;
using BayanPay.UserService.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateUser
{
    public class Command : IRequest<AppUser>
    {
        public AppUser User { get; set; }

        public Command(AppUser user)
        {
            User = user;
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User.ClerkUserId).NotEmpty().WithMessage("ClerkUserId is required.");
                RuleFor(x => x.User.FirstName).NotEmpty().WithMessage("FirstName is required.");
                RuleFor(x => x.User.LastName).NotEmpty().WithMessage("LastName is required.");
                RuleFor(x => x.User.Email).NotEmpty().WithMessage("Email is required.");
                RuleFor(x => x.User.Address).NotEmpty().WithMessage("Address is required.");
                RuleFor(x => x.User.BirthDate).NotEmpty().WithMessage("BirthDate is required.");
                RuleFor(x => x.User.Role).NotEmpty().WithMessage("Role is required.");
                RuleFor(x => x.User.CreatedDateTime).NotEmpty().WithMessage("CreatedDateTime is required.");
                RuleFor(x => x.User.UpdateDateTime).NotEmpty().WithMessage("UpdateDateTime is required.");
                RuleFor(x => x.User.CreatedBy).NotEmpty().WithMessage("CreatedBy is required.");
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
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == request.User.Id, cancellationToken);

                if (user == null)
                {
                    throw new ArgumentNullException(nameof(request.User), "User cannot be null.");
                }

                var appUser = new AppUser
                {
                    Id = request.User.Id,
                    ClerkUserId = request.User.ClerkUserId,
                    FirstName = request.User.FirstName,
                    LastName = request.User.LastName,
                    Email = request.User.Email,
                    Address = request.User.Address,
                    BirthDate = request.User.BirthDate,
                    Role = request.User.Role,
                    CreatedDateTime = request.User.CreatedDateTime,
                    UpdateDateTime = DateTime.UtcNow, // Assuming this is the current update time
                    CreatedBy = request.User.CreatedBy
                };

                _userDbContext.Users.Update(appUser);
                await _userDbContext.SaveChangesAsync(cancellationToken);
                return request.User;
            }
        }
    } 
}