using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ControlCenterMockApi.Models
{
    public class FileStatus
    {
        [JsonPropertyName("fileId")]
        public string FileId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("fileName")]
        public string FileName { get; set; }

        [JsonPropertyName("fullPath")]
        public string FullPath { get; set; }

        [JsonPropertyName("fileTransferProgress")]
        public int FileTransferProgress { get; set; }
    }
    public class BatchStatus
    {
        [JsonPropertyName("entityId")]
        public string EntityId { get; set; }

        [JsonPropertyName("meridianBatchId")]
        public string BatchId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("filesTotal")]
        public int FilesTotal { get; set; }

        [JsonPropertyName("filesTransferred")]
        public int FilesTransferred { get; set; }

        [JsonPropertyName("files")]
        public List<FileStatus> Files { get; set; }
    }
}
