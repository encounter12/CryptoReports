using System.Net;

namespace Reports.Crypto.WebService.Infrastructure.Helpers.Http
{
    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public T Data { get; set; }
    }
}