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

        // User Job Info Endpoints

        [HttpGet("UserJobInfo")]
        public IEnumerable<UserJobInfo> GetUserJobInfo()
        {
            string sql = $"SELECT * FROM TutorialAppSchema.UserJobInfo";
            return _dapper.LoadData<UserJobInfo>(sql);
        }

        [HttpGet("UserJobInfo/{UserId}")]
        public UserJobInfo GetUserJobInfo(int UserId)
        {
            string sql = @$"SELECT * FROM TutorialAppSchema.UserJobInfo WHERE UserId = {UserId} ";
            return _dapper.LoadDataSingle<UserJobInfo>(sql);
        }

        [HttpPut("EditUserJobInfo")]
        public IActionResult PutUserJobInfo(UserJobInfo userjobinfo)
        {
            string sql =
                @$"UPDATE TutorialAppSchema.UserJobInfo SET [JobTitle] = '{userjobinfo.JobTitle}', [Department] = '{userjobinfo.Department}' WHERE UserId = {userjobinfo.UserId}";
            int effectedRows;
            Console.WriteLine(sql);
            var x = _dapper.ExecuteSql(sql, out effectedRows);
            Console.WriteLine(effectedRows);
            if (x)
            {
                return Ok();
            }
            throw new Exception("Didn't change nothing.");
        }

        [HttpPost("AddUserJobInfo")]
        public IActionResult AddUserJobInfo(UserJobInfo userjobinfo)
        {
            string sql =
                $@"
                INSERT INTO TutorialAppSchema.UserJobInfo(
                        [UserId],
                        [JobTitle],
                        [Department]                
                    ) VALUES(
                        '{userjobinfo.UserId}',
                        '{userjobinfo.JobTitle}',
                        '{userjobinfo.Department}'
                    )
            ";
            Console.WriteLine(sql);
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Didn't add the user");
        }

        [HttpDelete("DeleteUserJobInfo/{UserId}")]
        public IActionResult DeleteUserJobInfo(int UserId)
        {
            string sql =
                @$"DELETE FROM TutorialAppSchema.UserJobInfo WHERE UserId = {UserId.ToString()}";
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Unable to Delete the User");
        }

        // User Salary Endpoints

        [HttpGet("GetUserSalary/{UserId}")]
        public UserSalary GetUserSalary(int UserId)
        {
            string sql = @$"SELECT * FROM TutorialAppSchema.UserSalary WHERE UserId = {UserId}";
            return _dapper.LoadDataSingle<UserSalary>(sql);
        }

        [HttpPut("EditUserSalary")]
        public IActionResult EditUserSalary(UserSalary userSalary)
        {
            string sql =
                @$"UPDATE TutorialAppSchema.UserSalary SET [Salary] = '{userSalary.Salary}', [AvgSalary] = '{userSalary.AvgSalary}' WHERE UserId = '{userSalary.UserId.ToString()}'";
            if (_dapper.ExecuteSql(sql))
            {
                return Ok(userSalary);
            }
            throw new Exception("Updating User Salary failed on save");
        }

        [HttpDelete("DeleteUserSalary/{UserId}")]
        public IActionResult DeleteUserSalary(int UserId)
        {
            string sql =
                @$"DELETE FROM TutorialAppSchema.UserSalary WHERE UserId = {UserId.ToString()}";
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Unable to Delete the record.");
        }

        [HttpPost("AddUserSalary")]
        public IActionResult AddUserSalary(UserSalary userSalaryForInsert)
        {
            string sql =
                @"
            INSERT INTO TutorialAppSchema.UserSalary (
                UserId,
                Salary
            ) VALUES ("
                + userSalaryForInsert.UserId.ToString()
                + ", "
                + userSalaryForInsert.Salary
                + ")";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok(userSalaryForInsert);
            }
            throw new Exception("Adding User Salary failed on save");
        }

        public static string EscapeSingleQuote(string input)
        {
            string output = input.Replace("'", "''");

            return output;
        }
    }
}
