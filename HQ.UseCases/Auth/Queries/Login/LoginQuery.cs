using ErrorOr;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQ.UseCases.Auth.Queries.Login
{
    public class LoginQuery: IRequest<ErrorOr<string>>
    {
        public string UserName { get; set; } = null;
        public string Role { get; set; } = null;
    }
}
