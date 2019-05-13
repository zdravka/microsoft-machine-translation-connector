using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Telerik.Sitefinity.Translations.AzureTranslator.Tests
{
    internal class TestableAzureTranslatorTextConnector : AzureTranslatorTextConnector
    {
        public HttpResponseMessage HttpResponseMessage { get; set; }

        protected override HttpClient GetClient()
        {
            return new HttpClient(new MockedMessageHandler(this.HttpResponseMessage));
        }

        public void InitializeCallMock(NameValueCollection config)
        {
            base.InitializeConnector(config);
        }

        public List<string> TranslateCallMock(List<string> input, ITranslationOptions translationOptions)
        {
            return base.Translate(input, translationOptions);
        }
    }

    internal class MockedMessageHandler : HttpMessageHandler
    {
        public MockedMessageHandler(HttpResponseMessage message)
        {
            this.Message = message;
        }

        public HttpResponseMessage Message { get; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Message);
        }
    }
}