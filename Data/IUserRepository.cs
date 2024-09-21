using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entity);
        public void RemoveEntity<T>(T entity);
        public IEnumerable<Users> GetUsers();
        public Users GetSingleUser(int UserID);
        public UserSalary GetSingleUserSalary(int UserId);
        public UserJobInfo GetSingleUserJobInfo(int UserId);
    }
}
