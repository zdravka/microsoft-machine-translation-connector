using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Sitefinity.Translations.AzureTranslator.Exceptions;
using static Telerik.Sitefinity.Translations.AzureTranslator.AzureTranslatorTextConnector;

namespace Telerik.Sitefinity.Translations.AzureTranslator.Tests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class AzureTranslatorTextConnectorTests
    {
        private TestableAzureTranslatorTextConnector sut;
        private MockedTranslationOptions options;
        private const string SuccessfulTranslationResponseTemplate = "[{{\"translations\" : [{{\"text\":\"{0}\"}}]}}]";
        private readonly string GenericTranslatedText = "translated_text";
        private string GenericSuccessfulTranslationResponse => string.Format(SuccessfulTranslationResponseTemplate, GenericTranslatedText);

        [TestInitialize]
        public void TestInit()
        {
            this.sut = new TestableAzureTranslatorTextConnector();
            this.options = new MockedTranslationOptions() { SourceLanguage = "en", TargetLanguage = "bg" };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Expected config with invalid length apiKey parameter to throw.")]
        public void InitializeConnector_InvalidKeyLength_Throws()
        {
            var testConfig = new NameValueCollection();
            testConfig.Add(Constants.ConfigParameters.ApiKey, new string('*', Constants.ValidApiKeyLength + 1));
            this.sut.InitializeCallMock(testConfig);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Expected config with no apiKey parameter to throw.")]
        public void InitializeConnector_KeyIsNull_Throws()
        {
            var testConfig = new NameValueCollection();
            this.sut.InitializeCallMock(testConfig);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Expected null input parameter to throw.")]
        public void Translate_NullInputCollection_Throws()
        {
            this.sut.TranslateCallMock(null, this.options);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Expected empty input parameter to throw.")]
        public void Translate_EmptyInputCollection_Throws()
        {
            this.sut.TranslateCallMock(new List<string>(), this.options);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Expected null options parameter to throw.")]
        public void Translate_NullOptions_Throws()
        {
            this.sut.TranslateCallMock(new List<string>() { "test" }, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Expected null source language parameter to throw.")]
        public void Translate_NullSourceLanguage_Throws()
        {
            this.options.SourceLanguage = null;
            this.sut.TranslateCallMock(new List<string>() { "test" }, this.options);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Expected empty source language parameter to throw.")]
        public void Translate_EmptySourceLanguage_Throws()
        {
            this.options.SourceLanguage = string.Empty;
            this.sut.TranslateCallMock(new List<string>() { "test" }, this.options);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Expected null target language parameter to throw.")]
        public void Translate_NullTargetLanguage_Throws()
        {
            this.options.TargetLanguage = null;
            this.sut.TranslateCallMock(new List<string>() { "test" }, this.options);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Expected empty target language parameter to throw.")]
        public void Translate_EmptyTargetLanguage_Throws()
        {
            this.options.TargetLanguage = string.Empty;
            this.sut.TranslateCallMock(new List<string>() { "test" }, this.options);
        }

        [TestMethod]
        public void Translate_NullTextInput_SendsEmptyTextForTransaltion()
        {
            // arrange
            string requestedTextForTranslation = null;
            this.sut.mockedHttpClientSendAsyncDelegate = x =>
            {
                var requestText = x.Content.ReadAsStringAsync().Result;
                var serializer = new JavaScriptSerializer();
                dynamic result = serializer.DeserializeObject(requestText);

                requestedTextForTranslation = result[0]["Text"];

                return new HttpResponseMessage() { Content = new StringContent(GenericSuccessfulTranslationResponse) };
            };

            this.sut.TranslateCallMock(new List<string>() { null }, this.options);
            Assert.AreEqual(string.Empty, requestedTextForTranslation);
        }

        [TestMethod]
        public void Translate_SuccessfulTransaltionsRequest_ReturnsCollectionWithTranslation()
        {
            // arrange
            this.sut.mockedHttpClientSendAsyncDelegate = x => new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(GenericSuccessfulTranslationResponse)
            };

            // act
            var result = this.sut.TranslateCallMock(new List<string>() { "stub_text" }, this.options);

            // assert
            Assert.AreEqual(GenericTranslatedText, result[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(AzureTranslatorException), "Expected unsuccessful translation request to throw.")]
        public void Translate_UnsuccessfulTransaltions_Throws()
        {
            this.sut.mockedHttpClientSendAsyncDelegate = x => new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new StringContent(@"
{""error"" : {""message"":""test_error_message""}}
")
            };

            this.sut.TranslateCallMock(new List<string>() { "test" }, this.options);
        }

        [TestMethod]
        [ExpectedException(typeof(AzureTranslatorResponseFormatException), "Expected unexpected successful response format to throw.")]
        public void Translate_UnexpectedResponseFormat_Throws()
        {
            // arrange
            this.sut.mockedHttpClientSendAsyncDelegate = x => new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(@"
[{""strang_translations"" : [{""strange_text"":""result_text""}]}]
")
            };

            // act
            var result = this.sut.TranslateCallMock(new List<string>() { "stub_text" }, this.options);

            // assert
            Assert.AreEqual("result_text", result[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(AzureTranslatorResponseFormatException), "Expected unexpected error response format to throw.")]
        public void Translate_UnexpectedErrorFormat_Throws()
        {
            this.sut.mockedHttpClientSendAsyncDelegate = x => new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new StringContent(@"
{""strange_error"" : {""strange_message"":""test_error_message""}}
")
            };

            this.sut.TranslateCallMock(new List<string>() { "test" }, this.options);
        }

        [TestMethod]
        [ExpectedException(typeof(AzureTranslatorSerializationException), "Expected error response wrong json format to throw.")]
        public void Translate_ErrorResponseBrokenJson_Throws()
        {
            this.sut.mockedHttpClientSendAsyncDelegate = x => new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new StringContent(@"
{""strange_error"" : ""strange_message"":""test_error_message""}}
")
            };

            this.sut.TranslateCallMock(new List<string>() { "test" }, this.options);
        }

        [TestMethod]
        [ExpectedException(typeof(AzureTranslatorSerializationException), "Expected successful response wrong json format to throw.")]
        public void Translate_ResponseWrongJsonFormat_Throws()
        {
            // arrange
            this.sut.mockedHttpClientSendAsyncDelegate = x => new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(@"
[{""strang_translations"" : ""strange_text"":""result_text""}]}]
")
            };

            // act
            var result = this.sut.TranslateCallMock(new List<string>() { "stub_text" }, this.options);

            // assert
            Assert.AreEqual("result_text", result[0]);
        }

        [TestMethod]
        public void Translate_WhenRemoveHtmlTagsDisabled_AddsCorrectQueryStringParamToRequest()
        {
            // arrange
            string reqeustQueryString = null;
            this.sut.MockedIsRemoveHtmlTagsEnabled = false;
            this.sut.mockedHttpClientSendAsyncDelegate = x =>
            {
                reqeustQueryString = x.RequestUri.Query;

                return new HttpResponseMessage() { Content = new StringContent(GenericSuccessfulTranslationResponse) };
            };

            this.sut.TranslateCallMock(new List<string>() { null }, this.options);
            var expected = $"{Constants.AzureTransalteEndpointConstants.TextTypeQueryParam}=html";
            Assert.IsTrue(reqeustQueryString.Contains(expected), $"Expected {reqeustQueryString} to contain {expected}");
        }

        [TestMethod]
        public void Translate_WhenRemoveHtmlTagsEnabled_DoesNotAddQueryStringParamToRequest()
        {
            // arrange
            string reqeustQueryString = null;
            this.sut.MockedIsRemoveHtmlTagsEnabled = true;
            this.sut.mockedHttpClientSendAsyncDelegate = x =>
            {
                reqeustQueryString = x.RequestUri.Query;

                return new HttpResponseMessage() { Content = new StringContent(GenericSuccessfulTranslationResponse) };
            };

            this.sut.TranslateCallMock(new List<string>() { null }, this.options);
            var expected = $"{Constants.AzureTransalteEndpointConstants.TextTypeQueryParam}=html";
            Assert.IsFalse(reqeustQueryString.Contains(expected), $"Expected {reqeustQueryString} to not contain {expected}");
        }

        [TestMethod]
        public void Translate_WhenSourceCultureIsCZAndTargetIsBG_SendsQueryStringParamWithSourceValueCZAndTargetValueBG()
        {
            // arrange
            this.options.SourceLanguage = "CZ";
            this.options.TargetLanguage = "BG";
            string reqeustQueryString = null;
            this.sut.mockedHttpClientSendAsyncDelegate = x =>
            {
                reqeustQueryString = x.RequestUri.Query;

                return new HttpResponseMessage() { Content = new StringContent(GenericSuccessfulTranslationResponse) };
            };

            this.sut.TranslateCallMock(new List<string>() { null }, this.options);
            var bgExpectValue = $"{Constants.AzureTransalteEndpointConstants.TargetCultureQueryParam}=BG";
            var czExpectValue = $"{Constants.AzureTransalteEndpointConstants.SourceCultureQueryParam}=CZ";
            Assert.IsTrue(reqeustQueryString.Contains(bgExpectValue), $"Expected {reqeustQueryString} to contain {bgExpectValue}");
            Assert.IsTrue(reqeustQueryString.Contains(czExpectValue), $"Expected {reqeustQueryString} to contain {czExpectValue}");
        }

    }
}
