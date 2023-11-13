using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Http;
using Meridian.IngestionManagement.Interface.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Microsoft.Extensions.DependencyInjection;
using Meridian.IngestionManagement.Core.Interfaces.BackgroundTasks;
using System.Collections.Generic;
using System.IO;
using Meridian.IngestionManagement.Core.Interfaces.Services;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Meridian.IngestionManagement.Core.Models;
using Meridian.IngestionManagement.Infrastructure.Data;

namespace Meridian.IngestionManagement.Controllers
{
    public class DataIngestionController : BaseApiController
    {
        #region "Global Variables"
        public IBackgroundTaskQueue _queue { get; }
        private readonly ILogger<DataIngestionController> _logger;
        private readonly IBucketService _bucketService;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        #endregion

        #region "Contructor"
        public DataIngestionController(ILogger<DataIngestionController> logger, IBucketService bucketService, IConfiguration configuration,
            IBackgroundTaskQueue queue, IServiceScopeFactory serviceScopeFactory)
        {

            _logger = logger;
            _bucketService = bucketService;
            _configuration = configuration;
            _queue = queue;
            _serviceScopeFactory = serviceScopeFactory;
        }
        #endregion

        #region "Action Methods"
        /// <summary>
        /// This will receive a key name of s3 object and move to different bucket and return meta data
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        [HttpPost("ingestfromS3")]
        public async Task<IActionResult> ProcessData(string keyName, string bucketName)
        {
            _logger.LogInformation("New request for proccessing list of files");

            try
            {
                MetadataCollection metaData = await _bucketService.ReadFileMetaData(keyName, bucketName);
                Dictionary<string, string> metaCollection = new Dictionary<string, string>();
                if (keyName.Contains("/manifest/", StringComparison.OrdinalIgnoreCase) && keyName.Contains(".json", StringComparison.OrdinalIgnoreCase))
                {
                    await Task.Delay(30000);
                }
                if (metaData != null && metaData.Count > 0)
                {

                    foreach (string key in metaData.Keys)
                    {
                        metaCollection.Add(key, metaData[key]);
                    }
                }
                _queue.QueueBackgroundWorkItem(async token =>
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
           
                        IManifestService _manifestService = scope.ServiceProvider.GetService<IManifestService>();
                        ICatalogService _catalogService = scope.ServiceProvider.GetService<ICatalogService>();
                        IMetaDataService _metadataService = scope.ServiceProvider.GetService<IMetaDataService>();
                        var processedKey = keyName;

                        if (keyName.Contains("/manifest/", StringComparison.OrdinalIgnoreCase) && keyName.Contains(".json", StringComparison.OrdinalIgnoreCase))
                        {
                            string[] keyParts = keyName.Split("/");
                            string batchId = keyParts[keyParts.Length - 1];
                            batchId = batchId.Replace(".json", string.Empty, StringComparison.OrdinalIgnoreCase);
                            Manifest manifest = null;

                            manifest = JsonConvert.DeserializeObject<Manifest>(await _bucketService.ReadFileAsync(processedKey, bucketName));

                            if (manifest != null)
                            {
                                _manifestService.validateManifest(manifest);
                                await _catalogService.CreateCatalog(new Guid(batchId), $"{_configuration.GetSection("API")["CatalogAPI"]}{_configuration.GetSection("API")["CreateCatalogEndPoint"]}");
                            }
                        }
                        else
                        {
                            // call that function - service function
                            _metadataService.CreateBatchMeta(metaCollection, keyName, bucketName);
                            //Add logic to validate checksum and bytesize
                            await _bucketService.CopyObjectAsync(bucketName, keyName, _configuration.GetSection("S3")["FinalBucket"], processedKey);


                        }
                    }
                });

                return new OkObjectResult("Process Initiated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This will accept a file and upload to s3 bucket, This is just of internal testing to validate
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile image, string scanid, string type, string parentid,string entityid)
        {
            _logger.LogInformation("New request for uploading a file");

            try
            {
                if (type.Equals("data", StringComparison.OrdinalIgnoreCase))
                {
                    string filename = image.FileName;
                    string name = filename.Substring(0, filename.LastIndexOf('.'));
                    string extension = filename.Substring(filename.LastIndexOf('.'), filename.Length - name.Length);
                    string path = "2101G1122";
                    Dictionary<string, string> metaData = new Dictionary<string, string>();
                    metaData.Add("schematype", "ImageFile");
                    metaData.Add("parentid", parentid);
                    metaData.Add("batchid", scanid);
                    metaData.Add("entityid", entityid);
                    metaData.Add("schemaversion", "1.0.0");
                    metaData.Add("fullpath", $"C:/DtaSetup/data/{filename}");                   
                    metaData.Add("md5checksum", "06cb38d17f68a7dc5db6b2d8c4122b03");
                    metaData.Add("fileid", entityid);
                    metaData.Add("filename", filename);
                    await _bucketService.UploadFileAsync(_configuration.GetSection("S3")["IngestionBucket"], image, $"{path}/data/{scanid}/{name}{extension}", metaData);
                }
                else
                {
                    string path = "2101G1122";
                    await _bucketService.UploadFileAsync(_configuration.GetSection("S3")["IngestionBucket"], image, $"{path}/manifest/{scanid}.json");
                }
                return new OkObjectResult("Done");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        #endregion

        
    }
}
