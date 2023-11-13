using System;
using System.Collections.Generic;
using System.Linq;
using Meridian.UserManagement.Core.Interfaces.Repositories;
using Meridian.UserManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Meridian.UserManagement.Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User CreateUser(User user)
        {
            _dbContext.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public List<User> GetUsers()
        {
            return _dbContext.User.ToList();
        }

        public User GetUserDetail(int userId)
        {
            return _dbContext.User.Find(userId);
        }

        public User UpdateUser(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return user;
        }

        public User DeleteUser(int UserId)
        {

            var account = _dbContext.User.Find(UserId);
            _dbContext.User.Remove(account);
            _dbContext.SaveChanges();

            return null;

        }
    }
}
