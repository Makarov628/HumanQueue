using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQ.UseCases.Auth.Dtos
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null;
        public string Password { get; set; } = null;
        public string Role { get; set; } = null;
    }
}
