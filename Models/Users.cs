using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Gender { get; set; } = "";
        public bool Active { get; set; }
    }
}
