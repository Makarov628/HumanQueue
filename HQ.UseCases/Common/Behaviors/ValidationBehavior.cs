using HQ.UseCases.Auth.Queries.Login;

using ErrorOr;

using FluentValidation;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQ.UseCases.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponce> :
        IPipelineBehavior<TRequest, TResponce>
        where TRequest : IRequest<TResponce>
        where TResponce: IErrorOr
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponce> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponce> next, 
            CancellationToken cancellationToken)
        {
            if (_validator == null)
            {
                return await next();
            }

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid)
            {
                return await next();
            }

            var errors = validationResult.Errors
                .ConvertAll(f => Error.Validation(f.PropertyName, f.ErrorMessage));

            return (dynamic)errors;
        }
    }
}
