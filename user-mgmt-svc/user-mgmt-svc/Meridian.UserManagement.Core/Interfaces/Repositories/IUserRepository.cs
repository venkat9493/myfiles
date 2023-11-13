using System;
using System.Collections.Generic;

using Meridian.UserManagement.Core.Models;

namespace Meridian.UserManagement.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        User CreateUser(User user);
        User GetUserDetail(int userid);
        List<User> GetUsers();
        User UpdateUser(User user);
        User DeleteUser(int userId);
    }
}
