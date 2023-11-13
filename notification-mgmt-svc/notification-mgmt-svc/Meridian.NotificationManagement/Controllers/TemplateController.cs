using Meridian.NotificationManagement.Core.Interfaces.Services;
using Meridian.NotificationManagement.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meridian.NotificationManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {

        private readonly INotificationService _notificationService;
        public TemplateController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet("GetTemplate")]
        public Template Get(string templateid)
        {
            //List<TemplateProp> lstprop = new List<TemplateProp>();
            Template data =  _notificationService.GetTemplateDetail(templateid);
            //add subject and email

            return data;
        }

        [HttpPost("/template")]
        public IActionResult CreateTemplate([FromBody] Template template)
        {

            //_logger.LogInformation("New request for CreateTemplate");

            try
            {
                //using (var scope = new TransactionScope())
                {
                    var response = _notificationService.CreateTemplate(template);
                    //scope.Complete();
                    return new OkObjectResult(response);
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                throw;
            }
        }

    }
   
    
}
