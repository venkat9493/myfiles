
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Meridian.CatalogManagement.Interface.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meridian.CatalogManagement.Infrastructure.Services
{
    public class S3BucketService : IBucketService
    {
        private readonly IAmazonS3 _s3Client;

        public S3BucketService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }
        /// <summary>
        /// Read Metadata of a S3 object
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task<MetadataCollection> ReadFileMetaData(string keyName, string bucketName)
        {
            GetObjectMetadataRequest request = new GetObjectMetadataRequest
            {
                BucketName = bucketName,
                Key = keyName
            };
            GetObjectMetadataResponse response = await _s3Client.GetObjectMetadataAsync(request);
            return response.Metadata;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public async Task<string> ReadFileAsync(string keyName, string bucketName)
        {
            string responseBody = "";
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };
                using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                    string contentType = response.Headers["Content-Type"];
                    Console.WriteLine("Object metadata, Title: {0}", title);
                    Console.WriteLine("Content type: {0}", contentType);

                    responseBody = reader.ReadToEnd();
                    return responseBody;
                }
            }
            catch (AmazonS3Exception e)
            {
                // If bucket or object does not exist
                return string.Empty;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Copy object for source bucket to destination bucket
        /// </summary>
        /// <param name="sourceBucket"></param>
        /// <param name="sourceObjectKey"></param>
        /// <param name="targetBucket"></param>
        /// <param name="targetObjectKey"></param>
        /// <returns></returns>
        public async Task CopyObjectAsync(string sourceBucket, string sourceObjectKey, string targetBucket, string targetObjectKey)
        {
            // Create a list to store the upload part responses.
            List<UploadPartResponse> uploadResponses = new List<UploadPartResponse>();
            List<CopyPartResponse> copyResponses = new List<CopyPartResponse>();

            // Setup information required to initiate the multipart upload.
            InitiateMultipartUploadRequest initiateRequest =
                new InitiateMultipartUploadRequest
                {
                    BucketName = targetBucket,
                    Key = targetObjectKey
                };

            // Initiate the upload.
            InitiateMultipartUploadResponse initResponse =
                await _s3Client.InitiateMultipartUploadAsync(initiateRequest);

            // Save the upload ID.
            String uploadId = initResponse.UploadId;

            // Get the size of the object.
            GetObjectMetadataRequest metadataRequest = new GetObjectMetadataRequest
            {
                BucketName = sourceBucket,
                Key = sourceObjectKey
            };

            GetObjectMetadataResponse metadataResponse =
                await _s3Client.GetObjectMetadataAsync(metadataRequest);
            long objectSize = metadataResponse.ContentLength; // Length in bytes.

            // Copy the parts.
            long partSize = 5 * (long)Math.Pow(2, 20); // Part size is 5 MB.

            long bytePosition = 0;
            for (int i = 1; bytePosition < objectSize; i++)
            {
                CopyPartRequest copyRequest = new CopyPartRequest
                {
                    DestinationBucket = targetBucket,
                    DestinationKey = targetObjectKey,
                    SourceBucket = sourceBucket,
                    SourceKey = sourceObjectKey,
                    UploadId = uploadId,
                    FirstByte = bytePosition,
                    LastByte = bytePosition + partSize - 1 >= objectSize ? objectSize - 1 : bytePosition + partSize - 1,
                    PartNumber = i
                };

                copyResponses.Add(await _s3Client.CopyPartAsync(copyRequest));

                bytePosition += partSize;
            }

            // Set up to complete the copy.
            CompleteMultipartUploadRequest completeRequest =
            new CompleteMultipartUploadRequest
            {
                BucketName = targetBucket,
                Key = targetObjectKey,
                UploadId = initResponse.UploadId
            };
            completeRequest.AddPartETags(copyResponses);

            // Complete the copy.
            CompleteMultipartUploadResponse completeUploadResponse =
                await _s3Client.CompleteMultipartUploadAsync(completeRequest);


        }
        /// <summary>
        /// Multi part upload to S3 bucket
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="filePath"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public async Task UploadFileAsync(string bucketName, IFormFile file, string keyName)
        {

            var fileTransferUtility =
                new TransferUtility(_s3Client);

            using (var newMemoryStream = new MemoryStream())
            {
                file.CopyTo(newMemoryStream);
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    InputStream = newMemoryStream,
                    Key = keyName
                };
                fileTransferUtilityRequest.Metadata.Add("keyName", keyName);
                fileTransferUtilityRequest.Metadata.Add("dummymeta", "dummy");

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
            }

        }

        /// <summary>
        /// Delete file from s3 bucket
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFileAsync(string keyName, string bucketName)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };
                await _s3Client.DeleteObjectAsync(deleteObjectRequest);
                return true;
            }
            catch (AmazonS3Exception e)
            {
                //TODO: Need to implement logging 
                return false;
            }
            catch (Exception e)
            {
                //TODO: Need to implement logging 
                return false;
            }
        }

        /// <summary>
        /// store string as json on S3
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="content"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task<bool> PutContentAsync(string keyName, string content, string bucketName)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                var encodedBytes = Encoding.UTF8.GetBytes(content);
                memoryStream.Write(encodedBytes, 0, encodedBytes.Length);
                memoryStream.Position = 0;
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    InputStream = memoryStream,
                    ContentType = "application/json"
                };
               
                await _s3Client.PutObjectAsync(putRequest);
            }
            catch (Exception ex)
            {                
                return false;
            }
            return true;

        }
    }
}
