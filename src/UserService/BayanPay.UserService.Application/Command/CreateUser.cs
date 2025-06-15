using BayanPay.UserService.Domain;
using BayanPay.UserService.Persistence;
using FluentValidation;
using MediatR;

public class CreateUser
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
                RuleFor(x => x.User.ClerkUserId).NotEmpty().WithMessage("ClerkUserIdis required.");
                RuleFor(x => x.User.FirstName).NotEmpty().WithMessage("FirstName is required.");
                RuleFor(x => x.User.LastName).NotEmpty().WithMessage("LastName is required.");
                RuleFor(x => x.User.Email).NotEmpty().WithMessage("Email is required.");
                RuleFor(x => x.User.Address).NotEmpty().WithMessage("Address is required.");
                RuleFor(x => x.User.BirthDate).NotEmpty().WithMessage("BirthDate is required.");
                RuleFor(x => x.User.Role).NotEmpty().WithMessage("Role is required.");
                RuleFor(x => x.User.CreatedDateTime).NotEmpty().WithMessage("CreatedDateTime is required.");
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
                if (request.User == null)
                {
                    throw new ArgumentNullException(nameof(request.User), "User cannot be null.");
                }

                await _userDbContext.Users.AddAsync(request.User);
                await _userDbContext.SaveChangesAsync(cancellationToken);
                return request.User;
            }
        }
    } 
}