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

        static string EscapeSingleQuote(string input)
        {
            string output = input.Replace("'", "''");

            return output;
        }
    }
}
