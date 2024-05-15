using HQ.UseCases.Auth.Dtos;
using MediatR;

namespace HQ.UseCases.Auth.Queries.GetUser
{
    internal class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
    {
        private readonly List<User> _users = new List<User>
        {
            new User { Id = 1, Username = "admin", Password = "password", Role = "Admin" },
            new User { Id = 1, Username = "audit", Password = "password", Role = "Audit" }
        };

        public Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = _users.SingleOrDefault(
                    u => u.Username == request.Login 
                    && u.Password == request.Password);

            return Task.FromResult(user);
        }
    }
}
