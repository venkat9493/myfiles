using Meridian.IngestionManagement.Core.Interfaces.Services;
using Meridian.IngestionManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;


namespace Meridian.IngestionManagement.Controllers
{
    public class BatchController : BaseApiController
    {
        #region "Global Variables"
        public readonly ILogger<BatchController> _logger; 
        public readonly IMetaDataService _metaDataService;
        #endregion

        #region "Constructor"
        public BatchController(ILogger<BatchController> logger, IMetaDataService metaDataService)
        {
            _logger = logger;
            _metaDataService = metaDataService;
        }
        #endregion
        #region "Public Methods"
        [HttpGet("{batchId}/status")]
        public ActionResult<string> BatchStatus(Guid batchId)
        {
            try
            {
                return Ok(_metaDataService.GetBatchStatus(batchId));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

    }
}
