using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApplicationppp.Results
{
    public class NotFoundActionResult<T> : IHttpActionResult
    {
        public NotFoundActionResult(T response, HttpRequestMessage request)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            Content = response;
            Request = request;
        }

        public T Content { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound, Content);

            return response;
        }
    }
}
