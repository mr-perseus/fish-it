using System.Net;

namespace Fishit.Dal.Entities
{
    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Content { get; set; }

        public override string ToString()
        {
            return base.ToString() + "; " +
                   nameof(StatusCode) + "; " + StatusCode + "; " +
                   nameof(Message) + "; " + Message + "; " +
                   nameof(Content) + "; " + Content + "; ";
        }
    }
}