using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Telerik.Sitefinity.Translations.AzureTranslator.AzureTranslatorTextConnector;

namespace Telerik.Sitefinity.Translations.AzureTranslator.Tests
{
    [TestClass]
    public class AzureTranslatorTextConnectorTests
    {
        private TestableAzureTranslatorTextConnector sut;
        private MockedTranslationOptions options;

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
            testConfig.Add(Parameters.ApiKey, new string('*', Constants.ValidApiKeyLength + 1));
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
        public void Translate_EmptyInput_Throws()
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
        public void Translate_NullOrEmptyText_ReturnsEmptyText()
        {

        }

        [TestMethod]
        public void Translate_SuccessfulTransaltionsRequest_ReturnsCollectionWithTranslation()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(AzureTranslatorException), "Expected unsuccessful translation request to throw.")]
        public void Translate_UnsuccessfulTransaltions_Throws()
        {
            var errorMessage = new object[]
            {

            };

            this.sut.HttpResponseMessage = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                //Content = 
            };

            this.sut.TranslateCallMock(new List<string>() { "test" }, this.options);
        }
    }
}
