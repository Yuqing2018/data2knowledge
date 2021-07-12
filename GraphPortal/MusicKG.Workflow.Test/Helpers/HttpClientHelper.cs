using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Test.Helpers
{
    public class HttpClientHelper
    {
        public static HttpClient GetMockClient(bool isSucceed = true)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns((HttpRequestMessage request, CancellationToken cancellationToken) => GetMockResponse(request, cancellationToken, isSucceed));
            return new HttpClient(mockHttpMessageHandler.Object);
        }

        private static Task<HttpResponseMessage> GetMockResponse(HttpRequestMessage request, CancellationToken cancellationToken, bool isSucceed)
        {
            var response = new HttpResponseMessage(isSucceed ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.BadRequest);
            response.Content = new StringContent(GetAuthJson(), Encoding.UTF8, "application/json");
            return Task.FromResult(response);
        }

        private static string GetAuthJson()
        {
            return "{ \"isAuthenticated\": true }";
        }
    }
}
