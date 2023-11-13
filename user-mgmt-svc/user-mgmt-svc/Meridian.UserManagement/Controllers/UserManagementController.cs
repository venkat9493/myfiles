using Meridian.UserManagement.Core.Interfaces.Services;
using Meridian.UserManagement.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Meridian.UserManagement.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserManagementController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost(Name = "CreateUser")]
        public IActionResult CreateUser(UserAndRoleInfo userRoleInfo)
        {
           // _logger.LogInformation("New request for CreateUser");

            try
            {
                using (var scope = new TransactionScope())
                {
                    var response = _userService.CreateUser(userRoleInfo.User);
                    scope.Complete();
                    return new OkObjectResult(response);
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("/users")]
        public IActionResult GetUsers()
        {
            //_logger.LogInformation("New request for GetUsers");

            try
            {
                var response = _userService.GetUsers();
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("/user/{userId}")]
        public IActionResult GetUserDetail(int userId)
        {
            //_logger.LogInformation("New request for GetUserDetail");

            try
            {
                var response = _userService.GetUserDetail(userId);
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPut(Name = "UpdateUser")]
        public IActionResult UpdateUser(User user)
        {
            //_logger.LogInformation("New request for UpdateUser");

            try
            {
                using (var scope = new TransactionScope())
                {
                    var response = _userService.UpdateUser(user);
                    scope.Complete();
                    return new OkObjectResult(response);
                }
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpDelete("/user/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            //_logger.LogInformation("New request for DeleteUser");

            try
            {
                _userService.DeleteUser(userId);
                return new OkResult();

            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
