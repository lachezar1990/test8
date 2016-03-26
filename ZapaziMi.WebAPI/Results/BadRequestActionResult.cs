using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApplicationppp.Results
{
    public class BadRequestActionResult<T> : IHttpActionResult
    {
        public BadRequestActionResult(T response, HttpRequestMessage request)
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
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, Content);

            return response;
        }
    }
}
