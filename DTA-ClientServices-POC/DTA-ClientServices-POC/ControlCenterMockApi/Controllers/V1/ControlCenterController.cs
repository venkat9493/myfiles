using ControlCenterMockApi.Models;
using Microsoft.AspNetCore.Mvc;
using Nanostring.Common.Logging;
using Newtonsoft.Json;
using System;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ControlCenterMockApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [ControllerName("control-center")]
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    public class ControlCenterController : ControllerBase
    {
        private readonly ILogger<ControlCenterController> _logger;
        public ControlCenterController(ILogger<ControlCenterController> logger)
        {
            this._logger = logger;
        }

        [HttpPost("data-transfer-state/batch-status")]
        public ActionResult DataTransferState([FromBody] BatchStatus batchStatus)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.WriteInfo(message: $"{"StatusCode:" + HttpStatusCode.BadRequest + "," + "ErrorMessage:"+ Constants.BadRequestErrorMessage}");
                    return sendErrorResponse(HttpStatusCode.BadRequest, Constants.BadRequestErrorMessage);
                }
                var batchStatusResult = JsonConvert.SerializeObject(batchStatus);
                this._logger.WriteInfo(message: $"{"StatusCode:" + HttpStatusCode.OK + "," + "Result:" + batchStatusResult }");
                return StatusCode((int)HttpStatusCode.OK,true);

            }
            catch(Exception ex)
            {
                this._logger.WriteError(Constants.InternalServerError,ex);
                return sendErrorResponse(HttpStatusCode.InternalServerError, Constants.InternalServerError + "," + ex);

            }
        }

        [NonAction]
        private ActionResult sendErrorResponse(HttpStatusCode statusCode, string errorMessage)
        {
            var Response = new ApiErrorResponse
            {
                error = new Error
                {
                    message = errorMessage,
                    httpStatusCode = (int)statusCode
               }
            };
            this._logger.WriteInfo(message: $"{"StatusCode:" + HttpStatusCode.BadRequest + "," + "ErrorMessage:" + errorMessage}");
            return StatusCode((int)HttpStatusCode.BadRequest, Response);
        }
    }
}
