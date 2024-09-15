using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpGet("register")]
        public string Register()
        {
            return "Registered";
        }

        [HttpGet("login/{admin}")]
        public string Login(string admin)
        {
            return $"Logged In as {admin}";
        }
    }
}
