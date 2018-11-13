using System;
using System.Net;

namespace Microsoft.MobCAT.Services
{
    public class HttpServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public new Exception InnerException { get; private set; }

        public HttpServiceException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpServiceException(HttpStatusCode statusCode, Exception innerException) : this(statusCode)
        {
            InnerException = innerException;
        }
    }
}