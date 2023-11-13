using Meridian.NotificationManagement.Core.Interfaces.Services;
using Meridian.NotificationManagement.Core.Models;
using Meridian.NotificationManagement.Infrastructure.Data;
using Meridian.NotificationManagement.Model;
using Meridian.NotificationManagement.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Meridian.NotificationManagement.Controllers
{
    
    [Route("api/[controller]")]
    public  class EmailNotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public EmailNotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        //public static TemplateDbcontext _dbContext;

        //public EmailNotificationController(TemplateDbcontext db)
        //{
        //    _dbContext = db;
        //}

        [HttpGet("test")]
        public string Get()
        {
            return "Hello Service";
        }

        [HttpPost("SendEmail")]
        public IActionResult Post([FromBody] NotificationMessage emailProperties)
        {

            try
            {
                //validate input parameters
                InputRequestValidation inputRequest = new InputRequestValidation();
                inputRequest.ValidateInputParameter(emailProperties);
                if (inputRequest.ErrorCode != 200)
                {
                    return StatusCode(inputRequest.ErrorCode, inputRequest.ErrorMessage);

                }

                EmailSend email = new EmailSend();

                bool bsend = email.SendEmail(emailProperties);
                //bool bsend = true;
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json").Build();


                if (bsend)
                    return StatusCode(200, "Email Sent");
                else
                    return StatusCode(500, "Error in sending email");
                //return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Unknown");
                //log the error in a file

            }
        }

        //[HttpPost("SendEmail")]
        //public IActionResult Post([FromBody] EmailProperties emailProperties)
        //{

        //    try
        //    {
        //        //validate input parameters
        //        InputRequestValidation inputRequest = new InputRequestValidation();
        //        inputRequest.ValidateInputParameter(emailProperties);
        //        if (inputRequest.ErrorCode != 200)
        //        {
        //            return StatusCode(inputRequest.ErrorCode, inputRequest.ErrorMessage);

        //        }

        //        EmailSend email = new EmailSend();

        //        bool bsend = email.SendEmail(emailProperties);

        //        var config = new ConfigurationBuilder()
        //            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        //            .AddJsonFile("appsettings.json").Build();



        //        if (bsend)
        //            return StatusCode(200, "Email Sent");
        //        else
        //            return StatusCode(500, "Error in sending email");
        //        //return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Unknown");
        //        //log the error in a file

        //    }
        //}
    }
}
