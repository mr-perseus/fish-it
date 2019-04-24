using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Fishit.Dal.Entities
{
    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public String Message { get; set; }
        public T Content { get; set; }
    }
}
