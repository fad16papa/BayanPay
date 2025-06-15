using BayanPay.UserService.Domain;
using BayanPay.UserService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BayanPay.UserService.Application.Users.Queries;

public class GetUserById
{
    public class Query : IRequest<AppUser>
    {
        public Guid Id { get; set; }

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public class Handler : IRequestHandler<Query, AppUser>
    {
        private readonly UserDbContext _context;

        public Handler(UserDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        }
    }
}
