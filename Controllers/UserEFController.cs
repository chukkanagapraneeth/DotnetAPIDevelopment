using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserEFController : ControllerBase
    {
        private DataContextEFCore _efCore;
        private IMapper _mapper;

        public UserEFController(IConfiguration config)
        {
            _efCore = new DataContextEFCore(config);
            _mapper = new Mapper(
                new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserToAddDTO, Users>();
                    cfg.CreateMap<UserJobInfo, UserJobInfo>().ReverseMap();
                    cfg.CreateMap<UserSalary, UserSalary>().ReverseMap();
                })
            );
        }

        [HttpGet("GetUsers/{UserId}")]
        public Users GetSingleUser(int UserId)
        {
            Users? user = _efCore.Users.Where(x => x.UserId == UserId).FirstOrDefault<Users>();
            if (user != null)
            {
                return user;
            }
            throw new Exception("Unable to find the user.");
        }

        [HttpGet("GetUsers")]
        public IEnumerable<Users> GetUsers()
        {
            return _efCore.Users.ToList<Users>();
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(Users user)
        {
            string firstname = EscapeSingleQuote(user.FirstName);
            string lastname = EscapeSingleQuote(user.LastName);

            Users? userDb = _efCore
                .Users.Where(x => x.UserId == user.UserId)
                .FirstOrDefault<Users>();
            if (userDb != null)
            {
                userDb.FirstName = firstname;
                userDb.LastName = lastname;
                userDb.Active = user.Active;
                userDb.Email = user.Email;
                userDb.Gender = user.Gender;
                if (_efCore.SaveChanges() > 0)
                {
                    return Ok();
                }
                throw new Exception("Failed to update user");
            }
            throw new Exception("Unable to find the user.");
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserToAddDTO user)
        {
            string firstname = EscapeSingleQuote(user.FirstName);
            string lastname = EscapeSingleQuote(user.LastName);

            Users userDb = _mapper.Map<Users>(user);

            _efCore.Add(userDb);
            if (_efCore.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed to update user");
        }

        [HttpDelete("DeleteUser/{UserId}")]
        public IActionResult DeleteUser(int UserId)
        {
            Users? userDb = _efCore.Users.Where(x => x.UserId == UserId).FirstOrDefault<Users>();
            if (userDb != null)
            {
                _efCore.Users.Remove(userDb);
                if (_efCore.SaveChanges() > 0)
                {
                    return Ok();
                }
                throw new Exception("Failed to delete user");
            }
            throw new Exception("Unable to find the user.");
        }

        [HttpGet("UserSalary/{userId}")]
        public IEnumerable<UserSalary> GetUserSalaryEF(int userId)
        {
            return _efCore.UserSalary.Where(u => u.UserId == userId).ToList();
        }

        [HttpPost("UserSalary")]
        public IActionResult PostUserSalaryEf(UserSalary userForInsert)
        {
            _efCore.UserSalary.Add(userForInsert);
            if (_efCore.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Adding UserSalary failed on save");
        }

        [HttpPut("UserSalary")]
        public IActionResult PutUserSalaryEf(UserSalary userForUpdate)
        {
            UserSalary? userToUpdate = _efCore
                .UserSalary.Where(u => u.UserId == userForUpdate.UserId)
                .FirstOrDefault();

            if (userToUpdate != null)
            {
                _mapper.Map(userForUpdate, userToUpdate);
                if (_efCore.SaveChanges() > 0)
                {
                    return Ok();
                }
                throw new Exception("Updating UserSalary failed on save");
            }
            throw new Exception("Failed to find UserSalary to Update");
        }

        [HttpDelete("UserSalary/{userId}")]
        public IActionResult DeleteUserSalaryEf(int userId)
        {
            UserSalary? userToDelete = _efCore
                .UserSalary.Where(u => u.UserId == userId)
                .FirstOrDefault();

            if (userToDelete != null)
            {
                _efCore.UserSalary.Remove(userToDelete);
                if (_efCore.SaveChanges() > 0)
                {
                    return Ok();
                }
                throw new Exception("Deleting UserSalary failed on save");
            }
            throw new Exception("Failed to find UserSalary to delete");
        }

        [HttpGet("UserJobInfo/{userId}")]
        public IEnumerable<UserJobInfo> GetUserJobInfoEF(int userId)
        {
            return _efCore.UserJobInfo.Where(u => u.UserId == userId).ToList();
        }

        [HttpPost("UserJobInfo")]
        public IActionResult PostUserJobInfoEf(UserJobInfo userForInsert)
        {
            _efCore.UserJobInfo.Add(userForInsert);
            if (_efCore.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Adding UserJobInfo failed on save");
        }

        [HttpPut("UserJobInfo")]
        public IActionResult PutUserJobInfoEf(UserJobInfo userForUpdate)
        {
            UserJobInfo? userToUpdate = _efCore
                .UserJobInfo.Where(u => u.UserId == userForUpdate.UserId)
                .FirstOrDefault();

            if (userToUpdate != null)
            {
                _mapper.Map(userForUpdate, userToUpdate);
                if (_efCore.SaveChanges() > 0)
                {
                    return Ok();
                }
                throw new Exception("Updating UserJobInfo failed on save");
            }
            throw new Exception("Failed to find UserJobInfo to Update");
        }

        [HttpDelete("UserJobInfo/{userId}")]
        public IActionResult DeleteUserJobInfoEf(int userId)
        {
            UserJobInfo? userToDelete = _efCore
                .UserJobInfo.Where(u => u.UserId == userId)
                .FirstOrDefault();

            if (userToDelete != null)
            {
                _efCore.UserJobInfo.Remove(userToDelete);
                if (_efCore.SaveChanges() > 0)
                {
                    return Ok();
                }
                throw new Exception("Deleting UserJobInfo failed on save");
            }
            throw new Exception("Failed to find UserJobInfo to delete");
        }

        public static string EscapeSingleQuote(string input)
        {
            string output = input.Replace("'", "''");

            return output;
        }
    }
}
