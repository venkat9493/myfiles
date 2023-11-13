using Meridian.IngestionManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Meridian.IngestionManagement.Controllers
{
    public class MessageController : BaseApiController
    {
        /// <summary>
        /// Get data from queueue on the basis of tenant id
        /// </summary>
        /// <param name="tenantid">tenent id for which want to get message</param>
        /// <param name="queueid">queue id from where need to read data</param>
        /// <param name="batchId">This is optional field and added for now so that DTA can get message for a specific batch id</param>
        /// <returns></returns>
        [HttpGet("fetchreturndata")]
        public ActionResult<Message> FetchReturnData(string tenantid, string queueid, string batchId = null)
        {
            Message message = new() { TenantId = tenantid, MessageType = MessageType.CLEAN_UP, BatchId = string.IsNullOrEmpty(batchId) ?  null  : new System.Guid(batchId) };
            return Ok(message);
        }

        /// <summary>
        /// It will return true/false according to the message avaialalbe in queue for a tenant id
        /// </summary>
        /// <param name="tenantid">tenent id for which sending heart beat message</param>
        /// <param name="queueid">queue id from where need to read data</param>
        /// <returns></returns>
        [HttpGet("heartbeat")]
        public ActionResult<string> HeartBeat(string tenantid, string queueid)
        {
            return Ok(true);
        }

       
        [HttpGet("fetcherrorreturndata")]
        public ActionResult<Message> FetchErrorReturnData(string tenantid, string deviceSerialNumber, string batchId = null)
        {
            Message message = new() { TenantId = tenantid, MessageType = MessageType.FILE_RETRY, BatchId = string.IsNullOrEmpty(batchId) ? null : new System.Guid(batchId), DeviceSerialNumber= deviceSerialNumber, BatchMetadataErroredFiles =new System.Collections.Generic.List<FileMessage>{ new FileMessage { 
            FileId = new System.Guid("b7f5f6f7-3f85-4434-8ba9-c5e364201353"),
            FileName = "mockImage.png",
            FilePath = "C:/DtaSetup/CosMx/mockImage.png",
            FileSizeBytes = 10000,
            MD5CheckSum = "f5787412575c703c960d98ed34d1410e",
            EntityId = new System.Guid("3104dd62-88a2-4bba-98f0-8bd41fd90193"),
            ParentId = new System.Guid("d4fdc40e-1125-45a6-b1cf-2dd73815bca6"),
            SchemaType ="ImageFile",
            SchemaVersion ="1.0.0",
            FailureReason = "Checksum mismatch"

            } } };
            return Ok(message);
        }
    }
}
