using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DotnetAPI.Data;
using DotnetAPI.DTOs;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly DataContextDapper _dapper;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            _dapper = new DataContextDapper(configuration);
        }

        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlUserCheck =
                    @$"SELECT * FROM TutorialAppSchema.Auth WHERE Email = '{userForRegistration.Email}'";

                IEnumerable<UserForRegistrationDto> userCount =
                    _dapper.LoadData<UserForRegistrationDto>(sqlUserCheck);

                if (userCount.Count() == 0)
                {
                    byte[] passwordSalt = new byte[128 / 8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }
                    ;

                    byte[] passwordHash = GetPasswordHash(
                        userForRegistration.Password,
                        passwordSalt
                    );

                    string sqlAddAuth =
                        @$"INSERT INTO TutorialAppSchema.Auth ([Email], 
                        [PasswordSalt], [PasswordHash]) VALUES ('{userForRegistration.Email}', @PasswordSalt, @PasswordHash)";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter passwordSaltParameter = new SqlParameter(
                        "@PasswordSalt",
                        SqlDbType.VarBinary
                    );
                    passwordSaltParameter.Value = passwordSalt;

                    SqlParameter passwordHashParameter = new SqlParameter(
                        "@PasswordHash",
                        SqlDbType.VarBinary
                    );
                    passwordHashParameter.Value = passwordHash;

                    sqlParameters.Add(passwordSaltParameter);
                    sqlParameters.Add(passwordHashParameter);

                    if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                    {
                        return Ok();
                    }
                    throw new Exception("Unable to register the user.");
                }
                throw new Exception("User Already Exists!");
            }
            throw new Exception("Passwords Do Not Match!");
        }

        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            return Ok();
        }

        private byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            string passwordSaltPlusString =
                _configuration.GetSection("AppSettings:PasswordKey").Value
                + Convert.ToBase64String(passwordSalt);

            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000000,
                numBytesRequested: 256 / 8
            );
        }
    }
}
