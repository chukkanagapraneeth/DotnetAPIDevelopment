using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.DTOs
{
    public partial class UserForRegistrationDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string PasswordConfirm { get; set; } = "";
    }
}
