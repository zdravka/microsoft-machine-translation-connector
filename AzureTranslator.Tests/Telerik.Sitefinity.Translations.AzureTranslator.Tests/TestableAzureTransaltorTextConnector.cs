using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Telerik.Sitefinity.Translations.AzureTranslator.Tests
{
    internal class TestableAzureTranslatorTextConnector : AzureTranslatorTextConnector
    {
        public Func<HttpRequestMessage, HttpResponseMessage> mockedHttpClientSendAsyncDelegate { get; set; }
        public bool MockedIsSendingHtmlEnabled { get; set; }

        public void InitializeCallMock(NameValueCollection config)
        {
            base.InitializeConnector(config);
        }

        public List<string> TranslateCallMock(List<string> input, ITranslationOptions translationOptions)
        {
            return base.Translate(input, translationOptions);
        }

        protected override HttpClient GetClient()
        {
            return new HttpClient(new MockedMessageHandler(this.mockedHttpClientSendAsyncDelegate));
        }

        protected override bool IsSendingHtmlEnabled()
        {
            return this.MockedIsSendingHtmlEnabled;
        }

        private class MockedMessageHandler : HttpMessageHandler
        {
            public MockedMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> sendAsyncDelegate)
            {
                this.sendAsyncDelegate = sendAsyncDelegate;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (this.sendAsyncDelegate != null)
                {
                    return Task.FromResult(this.sendAsyncDelegate(request));
                }
                else
                {
                    return Task.FromResult(new HttpResponseMessage()
                    {
                        StatusCode = System.Net.HttpStatusCode.NotImplemented
                    });
                }
            }

            private readonly Func<HttpRequestMessage, HttpResponseMessage> sendAsyncDelegate;
        }
    }
}