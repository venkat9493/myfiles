using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Meridian.CatalogManagement.Interface.Services
{
    public interface IBucketService
    {
        Task<MetadataCollection> ReadFileMetaData(string keyName, string bucketName);
        Task<string> ReadFileAsync(string keyName, string bucketName);

        Task CopyObjectAsync(string sourceBucket, string sourceObjectKey, string targetBucket, string targetObjectKey);
        Task UploadFileAsync(string bucketName, IFormFile file, string keyName);
        Task<bool> DeleteFileAsync(string keyName, string bucketName);
        Task<bool> PutContentAsync(string keyName, string content, string bucketName);
    }
}
