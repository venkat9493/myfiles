using Meridian.IngestionManagement.Core.Models.APISchemaModels;
using System;
using System.Collections.Generic;

namespace Meridian.IngestionManagement.Core.Helper
{
    public static class FileMetadataBind
    {
        public static FileMetadata GetFileInfoData(Dictionary<string, string> metaCollection, FileInformation fileInformation, FileMetadata fileMetadata, Guid batchId)
        {
            fileInformation.FileName = metaCollection.ContainsKey("x-amz-meta-filename") ? metaCollection["x-amz-meta-filename"]: string.Empty;
            fileInformation.SchemaType = metaCollection.ContainsKey("x-amz-meta-schematype") ? metaCollection["x-amz-meta-schematype"] : string.Empty;
            fileInformation.SchemaVersion = metaCollection.ContainsKey("x-amz-meta-schemaversion") ? metaCollection["x-amz-meta-schemaversion"] : string.Empty;
            fileInformation.FullPath = metaCollection.ContainsKey("x-amz-meta-fullpath") ? metaCollection["x-amz-meta-fullpath"] : string.Empty;
            fileInformation.EntityId = metaCollection["x-amz-meta-entityid"];

            fileMetadata.FileInfo = fileInformation;
            fileMetadata.BatchMetadataId = batchId;
            fileMetadata.MD5CheckSum = metaCollection["x-amz-meta-md5checksum"];

            return fileMetadata;
        }
        public static List<FileMetadata> GetFileInfoDataToBind(Dictionary<string, string> metaCollection, FileInformation fileInformation, FileMetadata fileMetadata, Guid batchId)
        {
            List<FileMetadata> listdata = new();

            fileInformation.FileName = metaCollection.ContainsKey("x-amz-meta-filename") ? metaCollection["x-amz-meta-filename"] : string.Empty;
            fileInformation.SchemaType = metaCollection.ContainsKey("x-amz-meta-schematype") ? metaCollection["x-amz-meta-schematype"] : string.Empty;
            fileInformation.SchemaVersion = metaCollection.ContainsKey("x-amz-meta-schemaversion") ? metaCollection["x-amz-meta-schemaversion"] : string.Empty;
            fileInformation.FullPath = metaCollection.ContainsKey("x-amz-meta-fullpath") ? metaCollection["x-amz-meta-fullpath"] : string.Empty;
            fileInformation.EntityId = metaCollection["x-amz-meta-entityid"];
            fileInformation.Status = 1;// Need to add validation of file
            fileMetadata.FileInfo = fileInformation;

            fileMetadata.BatchMetadataId = batchId;
            fileMetadata.MD5CheckSum = metaCollection["x-amz-meta-md5checksum"];

            listdata.Add(fileMetadata);

            return listdata;
        }
        
        

    }
}
