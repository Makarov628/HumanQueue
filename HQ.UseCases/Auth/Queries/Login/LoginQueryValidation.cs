using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQ.UseCases.Auth.Queries.Login
{
    public class LoginQueryValidation: AbstractValidator<LoginQuery>
    {
        public LoginQueryValidation()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Role).NotEmpty();
        }
    }
}
