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

        private IUserRepository _userRepository;

        public UserEFController(IConfiguration config, IUserRepository userRepository)
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
            _userRepository = userRepository;
        }

        [HttpGet("GetUsers/{UserId}")]
        public Users GetSingleUser(int UserId)
        {
            Users? user = _userRepository.GetSingleUser(UserId);
            if (user != null)
            {
                return user;
            }
            throw new Exception("Unable to find the user.");
        }

        [HttpGet("GetUsers")]
        public IEnumerable<Users> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(Users user)
        {
            string firstname = EscapeSingleQuote(user.FirstName);
            string lastname = EscapeSingleQuote(user.LastName);

            Users? userDb = _userRepository.GetSingleUser(user.UserId);
            if (userDb != null)
            {
                userDb.FirstName = firstname;
                userDb.LastName = lastname;
                userDb.Active = user.Active;
                userDb.Email = user.Email;
                userDb.Gender = user.Gender;
                if (_userRepository.SaveChanges())
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

            _userRepository.AddEntity<Users>(userDb);
            if (_userRepository.SaveChanges())
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
                _userRepository.RemoveEntity<Users>(userDb);
                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("Failed to delete user");
            }
            throw new Exception("Unable to find the user.");
        }

        [HttpGet("UserSalary/{userId}")]
        public UserSalary GetUserSalaryEF(int userId)
        {
            return _userRepository.GetSingleUserSalary(userId);
        }

        [HttpPost("UserSalary")]
        public IActionResult PostUserSalaryEf(UserSalary userForInsert)
        {
            _userRepository.AddEntity<UserSalary>(userForInsert);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Adding UserSalary failed on save");
        }

        [HttpPut("UserSalary")]
        public IActionResult PutUserSalaryEf(UserSalary userForUpdate)
        {
            UserSalary? userToUpdate = _userRepository.GetSingleUserSalary(userForUpdate.UserId);

            if (userToUpdate != null)
            {
                _mapper.Map(userForUpdate, userToUpdate);
                if (_userRepository.SaveChanges())
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
            UserSalary? userToDelete = _userRepository.GetSingleUserSalary(userId);

            if (userToDelete != null)
            {
                _userRepository.RemoveEntity<UserSalary>(userToDelete);
                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("Deleting UserSalary failed on save");
            }
            throw new Exception("Failed to find UserSalary to delete");
        }

        [HttpGet("UserJobInfo/{userId}")]
        public UserJobInfo GetUserJobInfoEF(int userId)
        {
            return _userRepository.GetSingleUserJobInfo(userId);
        }

        [HttpPost("UserJobInfo")]
        public IActionResult PostUserJobInfoEf(UserJobInfo userForInsert)
        {
            _userRepository.AddEntity<UserJobInfo>(userForInsert);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Adding UserJobInfo failed on save");
        }

        [HttpPut("UserJobInfo")]
        public IActionResult PutUserJobInfoEf(UserJobInfo userForUpdate)
        {
            UserJobInfo? userToUpdate = _userRepository.GetSingleUserJobInfo(userForUpdate.UserId);

            if (userToUpdate != null)
            {
                _mapper.Map(userForUpdate, userToUpdate);
                if (_userRepository.SaveChanges())
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
            UserJobInfo? userToDelete = _userRepository.GetSingleUserJobInfo(userId);

            if (userToDelete != null)
            {
                _userRepository.RemoveEntity<UserJobInfo>(userToDelete);
                if (_userRepository.SaveChanges())
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
