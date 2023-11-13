using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ControlCenterMockApi.Models
{
    public class ApiResponseResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public dynamic Response { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class Detail
    {
        public string code { get; set; }
        public string message { get; set; }
        public string target { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public List<Detail> details { get; set; }
        public string message { get; set; }
        public int httpStatusCode { get; set; }
        public string target { get; set; }
    }

    public class ApiErrorResponse
    {
        public Error error { get; set; }
    }
}
