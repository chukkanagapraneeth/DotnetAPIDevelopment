using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private DataContextEFCore _efCore;

        public UserRepository(IConfiguration config)
        {
            _efCore = new DataContextEFCore(config);
        }

        public bool SaveChanges()
        {
            return _efCore.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entity)
        {
            if (entity != null)
            {
                _efCore.Add(entity);
            }
        }

        public void RemoveEntity<T>(T entity)
        {
            if (entity != null)
            {
                _efCore.Remove(entity);
            }
        }

        public IEnumerable<Users> GetUsers()
        {
            IEnumerable<Users> users = _efCore.Users.ToList<Users>();
            return users;
        }

        public Users GetSingleUser(int UserID)
        {
            Users? user = _efCore.Users.Where(u => u.UserId == UserID).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            throw new Exception("Failed to Get User");
        }

        public UserSalary GetSingleUserSalary(int UserId)
        {
            UserSalary? userSalary = _efCore
                .UserSalary.Where(u => u.UserId == UserId)
                .FirstOrDefault();
            if (userSalary != null)
            {
                return userSalary;
            }
            throw new Exception("Failed to get User Salary");
        }

        public UserJobInfo GetSingleUserJobInfo(int UserId)
        {
            UserJobInfo? userJobInfo = _efCore
                .UserJobInfo.Where(u => u.UserId == UserId)
                .FirstOrDefault();
            if (userJobInfo != null)
            {
                return userJobInfo;
            }
            throw new Exception("Failed to get User Job Info");
        }
    }
}
