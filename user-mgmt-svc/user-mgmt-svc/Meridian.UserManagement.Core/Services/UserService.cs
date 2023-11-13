using System;
using System.Collections.Generic;
using Meridian.UserManagement.Core.Interfaces.Services;
using Meridian.UserManagement.Core.Interfaces.Repositories;

using Meridian.UserManagement.Core.Models;

namespace Meridian.UserManagement.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User CreateUser(User user)
        {
            return _userRepository.CreateUser(user);
        }

        public List<User> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public User GetUserDetail(int userId)
        {
            return _userRepository.GetUserDetail(userId);
        }

        public User UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);
        }

        public User DeleteUser(int userId)
        {
            return _userRepository.DeleteUser(userId);
        }
    }
}
