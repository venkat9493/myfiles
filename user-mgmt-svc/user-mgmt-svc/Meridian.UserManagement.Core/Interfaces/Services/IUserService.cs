using System;
using System.Collections.Generic;

using Meridian.UserManagement.Core.Models;

namespace Meridian.UserManagement.Core.Interfaces.Services
{
    public interface IUserService
    {
        User CreateUser(User user);
        User GetUserDetail(int userId);
        List<User> GetUsers();
        User UpdateUser(User user);
        User DeleteUser(int userId);
    }
}
