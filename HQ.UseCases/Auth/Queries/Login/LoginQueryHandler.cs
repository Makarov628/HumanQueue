using HQ.Application.Common.Interfaces;
using HQ.UseCases.Auth.Dtos;

using ErrorOr;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQ.UseCases.Auth.Queries.Login
{

    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<string>>
    {
        private readonly IJwtTokenGeneration _jwtTokenGeneration;
        public LoginQueryHandler(IJwtTokenGeneration jwtTokenGeneration)
        {
            _jwtTokenGeneration = jwtTokenGeneration;
        }

        public async Task<ErrorOr<string>> Handle(LoginQuery auth, CancellationToken cancellationToken)
        {
            var accessToken = _jwtTokenGeneration.GenerateToken(auth.UserName, auth.Role);
            return accessToken;
        }
    }
}
