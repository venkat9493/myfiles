using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Meridian.CatalogManagement.Interface.Services;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using Meridian.CatalogManagement.Core.Interfaces.BackgroundTasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Meridian.CatalogManagement.Core.Models;

namespace Meridian.CatalogManagement.Controllers
{
    public class CatalogController : BaseApiController
    {
        #region "Global Variables"
        public IBackgroundTaskQueue _queue { get; }
        private readonly ILogger<CatalogController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        #endregion

        #region "Contructor"
        public CatalogController(ILogger<CatalogController> logger, IConfiguration configuration,IBackgroundTaskQueue queue, IServiceScopeFactory serviceScopeFactory)
        {
          
            _logger = logger;        
            _configuration = configuration;
            _queue = queue;
            _serviceScopeFactory = serviceScopeFactory;
        }
        #endregion

        #region "Action Methods"

        /// <summary>
        /// Create catalog entry
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        [HttpPost("createCatalogentry")]
        public IActionResult CreateCatalogEntry(string filepath, [FromBody] Dictionary<string, string> metaData)
        {
            _logger.LogInformation("New request for creating catalog");

            try
            {                
                _queue.QueueBackgroundWorkItem(async token =>
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        string[] fileNamePart = filepath.Split("/");                        
                        string fileName = $"{fileNamePart[fileNamePart.Length - 1].Substring(0, fileNamePart[fileNamePart.Length - 1].LastIndexOf('.'))}_{fileNamePart[fileNamePart.Length - 2]}";
                        string sourcePath = $"s3://{ _configuration.GetSection("S3")["FinalBucket"]}/{filepath}";
                        string destinationPath = $"s3://{ _configuration.GetSection("S3")["FinalBucket"]}/{filepath.Substring(0, filepath.LastIndexOf('.'))}";
                        //run python script
                        ExecutePython(sourcePath, destinationPath, metaData, fileName);
                    }
                });
                return new OkObjectResult("Catalog Created");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("readObject")]
        public IActionResult ReadObject([BindRequired] Guid fileid)
        {
            _logger.LogInformation("New request for reading object");

            try
            {
                string s3Path = string.Empty;
                TileDB.Cloud.Client.Login(token: _configuration.GetSection("TileDB")["Token"], host: $"{_configuration.GetSection("TileDB")["APIURL"]}/v1");
                TileDB.Cloud.Rest.Model.ArrayBrowserData result = TileDB.Cloud.RestUtil.ListArrays(username: _configuration.GetSection("TileDB")["Namespace"], search: fileid.ToString());
                if (result.Arrays.Count > 0)
                {
                    s3Path = result.Arrays[0].Uri;
                }
                return Ok(s3Path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("filesearch")]
        public IActionResult FileSearch([FromBody] SearchParameter searchParameter)
        {
            try
            {
                TileDB.Cloud.Client.Login(token: _configuration.GetSection("TileDB")["Token"], host: $"{_configuration.GetSection("TileDB")["APIURL"]}/v1");
                TileDB.Cloud.Rest.Model.ArrayBrowserData result = TileDB.Cloud.RestUtil.ListArrays(username: _configuration.GetSection("TileDB")["Namespace"], search: searchParameter.SearchText);
                return Ok(result.Arrays);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                throw;
            }
        }

        [HttpGet("getmetadata")]
        public IActionResult GetMetaData(string tileDBURL)
        {
            try
            {
                TileDB.Config cfg = new TileDB.Config();
                //In python we were not setting AWS credential but in C# it is not working without this. I will connect with them to fix it.
                cfg.set("vfs.s3.aws_access_key_id", "AKIA2S2JIPDP22JYISNV");
                cfg.set("vfs.s3.aws_secret_access_key", "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");
                cfg.set("vfs.s3.region", "us-west-2");
                TileDB.Context ctx = new TileDB.Context(cfg);
                var array_read = new TileDB.Array(ctx, tileDBURL, TileDB.QueryType.TILEDB_READ);               
                return Ok(array_read);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        #endregion

        #region "Private Methods"
        private void ExecutePython(string sourcePath, string destinationPath, Dictionary<string, string> metaCollection,string filename)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo();
                _logger.LogInformation("Inside python");
                //argument with file name and input parameters to run
                string metaDataString = string.Empty;
                foreach (string key in metaCollection.Keys)
                {
                    metaDataString += string.Format("{0}={1} ", key, metaCollection[key]);
                }
                string arguments = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", "python3 tiledb-register-array.py", sourcePath, destinationPath, _configuration.GetSection("TileDB")["APIURL"], _configuration.GetSection("TileDB")["Token"], _configuration.GetSection("TileDB")["Namespace"],filename, metaDataString);
                _logger.LogInformation(arguments);
                arguments = arguments.Replace("\"", "\\\"");
                arguments = arguments.Replace(@"\", @"/");
                _logger.LogInformation(arguments);
                start.FileName = "/bin/bash";
                start.Arguments = $"-c \"{arguments}\"";

                start.UseShellExecute = false;// Do not use OS shell
                start.CreateNoWindow = true; // We don't need new window
                start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
                start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
                                                    //start.LoadUserProfile = true;
                using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                        string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")                   
                        _logger.LogInformation(stderr);
                        _logger.LogInformation(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }
        #endregion
    }
}
