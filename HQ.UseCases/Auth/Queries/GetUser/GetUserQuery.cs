using HQ.UseCases.Auth.Dtos;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQ.UseCases.Auth.Queries.GetUser
{
    public class GetUserQuery: IRequest<User>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
