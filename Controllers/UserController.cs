using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private DataContextDapper _dapper;

        public UserController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("testconnection")]
        public DateTime TestConnection()
        {
            return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
        }

        [HttpGet("GetUsers/{UserId}")]
        public Users GetSingleUser(int UserId)
        {
            string sql = $"SELECT * FROM TutorialAppSchema.Users WHERE UserID = {UserId}";
            return _dapper.LoadDataSingle<Users>(sql);
        }

        [HttpGet("GetUsers")]
        public IEnumerable<Users> GetUsers()
        {
            string sql = $"SELECT * FROM TutorialAppSchema.Users";
            return _dapper.LoadData<Users>(sql);
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(Users user)
        {
            string firstname = EscapeSingleQuote(user.FirstName);
            string lastname = EscapeSingleQuote(user.LastName);

            string sql =
                @"
                UPDATE TutorialAppSchema.Users
                 SET [FirstName] = '"
                + firstname
                + "', [LastName] = '"
                + lastname
                + "', [Email] = '"
                + user.Email
                + "', [Gender] = '"
                + user.Gender
                + "', [Active] = '"
                + user.Active
                + "' WHERE UserId = "
                + user.UserId;

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("User was not updated.");
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserToAddDTO user)
        {
            string sql =
                $@"
                INSERT INTO TutorialAppSchema.Users(
                        [FirstName],
                        [LastName],
                        [Email],
                        [Gender],
                        [Active]
                    )VALUES(
                        '{user.FirstName}',
                        '{user.LastName}',
                        '{user.Email}',
                        '{user.Gender}',
                        '{user.Active}'
                    )
            ";

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Unable to add user to db");
        }

        [HttpDelete("DeleteUser/{UserId}")]
        public IActionResult DeleteUser(int UserId)
        {
            string sql = $@"DELETE FROM TutorialAppSchema.Users WHERE UserID = {UserId.ToString()}";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Unable to delete user");
        }

        static string EscapeSingleQuote(string input)
        {
            string output = input.Replace("'", "''");

            return output;
        }
    }
}
