using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQ.Application.Common.Interfaces
{
    public interface IJwtTokenGeneration
    {
        string GenerateToken(string userName, string role);
    }
}
