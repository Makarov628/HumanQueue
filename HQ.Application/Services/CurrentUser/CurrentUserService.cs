using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace HQ.Application.Services.CurrentUser
{
    public class CurrentUserService
    {
        public string? Login { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        private IEnumerable<Claim> Claims { get; set; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;

            Claims = user.Claims;

            Login = Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).FirstOrDefault() ?? string.Empty;
            Role = Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault() ?? string.Empty;
        }

    }
}
