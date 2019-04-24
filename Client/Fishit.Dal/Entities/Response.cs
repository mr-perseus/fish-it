using System.Net;

namespace Fishit.Dal.Entities
{
    public class Response<T>
    {
        public T Content;
        public string Message;
        public HttpStatusCode StatusCode;
    }
}