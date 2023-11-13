using Meridian.IngestionManagement.Core.Interfaces.Services;
using Meridian.IngestionManagement.Core.Models.APISchemaModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Meridian.IngestionManagement.Controllers
{
    public class MetadataController : BaseApiController
    {
        private readonly IMetaDataService _metadataService;
        private readonly ILogger<MetadataController> _logger;

        public MetadataController(IMetaDataService metadataService, ILogger<MetadataController> logger)
        {
            _metadataService = metadataService;
            _logger = logger;
        }

        [HttpGet("/metadata/{batchId}")]
        public IActionResult GetMetadataDetail(Guid batchId)
        {
            _logger.LogInformation("New request for GetTenantDetail");

            try
            {
                var response = _metadataService.GetMetadataDetail(batchId);
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("/metadata")]
        public IActionResult CreateMetadata([FromBody] BatchMetadata batchMetadata)
        {
            _logger.LogInformation("New request for CreateMetadata");

            try
            {
                using (var scope = new TransactionScope())
                {
                    var response = _metadataService.CreateMetadata(batchMetadata);
                    scope.Complete();
                    return new OkObjectResult(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("/metadata")]
        public IActionResult GetMetadata()
        {
            _logger.LogInformation("New request for GetMetadata");

            try
            {
                var response = _metadataService.GetMetadata();
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPut("/metadata")]
        public IActionResult UpdateMetadata([FromBody] BatchMetadata batchMetadata)
        {
            _logger.LogInformation("New request for UpdateMetadata");

            try
            {
                using (var scope = new TransactionScope())
                {
                    var response = _metadataService.UpdateMetadata(batchMetadata);
                    scope.Complete();
                    return new OkObjectResult(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpDelete("/metadata/{batchId}")]
        public IActionResult DeleteMetadata(Guid batchId)
        {
            _logger.LogInformation("New request for DeleteMetadata");

            try
            {
                var response = _metadataService.DeleteMetadata(batchId);
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
