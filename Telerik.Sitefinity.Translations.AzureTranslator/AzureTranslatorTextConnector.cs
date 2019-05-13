using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Telerik.Sitefinity.Translations.AzureTranslator
{
    /// <summary>
    /// Client for Azure Transaltor Text Service API
    /// </summary>
    public class AzureTranslatorTextConnector : MachineTranslationConnector
    {
        protected virtual HttpClient GetClient()
        {
            return new HttpClient();
        }

        /// <summary>
        /// Configures the connector instance
        /// </summary>
        /// <param name="config">apiKey key should contain the Azure Transaltor Text Api Service key.</param>
        protected override void InitializeConnector(NameValueCollection config)
        {
            var key = config.Get(Parameters.ApiKey);
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(Constants.NoApiKeyExceptionMessage);
            }

            if (key.Length != Constants.ValidApiKeyLength)
            {
                throw new ArgumentException(Constants.InvalidApiKeyExceptionMessage);
            }

            this.key = key;
        }

        protected override List<string> Translate(List<string> input, ITranslationOptions translationOptions)
        {
            var fromLanguageCode = translationOptions.SourceLanguage;
            var toLanguageCode = translationOptions.TargetLanguage;
            if (string.IsNullOrWhiteSpace(fromLanguageCode))
            {
                throw new ArgumentException(GetTranslаteArgumentExceptionMessage($"{nameof(translationOptions)}.{nameof(translationOptions.SourceLanguage)}", translationOptions.SourceLanguage.GetType()));
            }

            if (string.IsNullOrWhiteSpace(toLanguageCode))
            {
                throw new ArgumentException(GetTranslаteArgumentExceptionMessage($"{nameof(translationOptions)}.{nameof(translationOptions.TargetLanguage)}", translationOptions.SourceLanguage.GetType()));
            }

            if (input == null || input.Count == 0)
            {
                throw new ArgumentException(GetTranslаteArgumentExceptionMessage(nameof(input), input.GetType()));
            }

            if (fromLanguageCode == toLanguageCode)
            {
                return input;
            }

            string uri = string.Format(Constants.TEXT_TRANSLATION_API_ENDPOINT + "&from={0}&to={1}", fromLanguageCode, toLanguageCode);

            var body = new List<object>();
            foreach (var text in input)
            {
                body.Add(new { Text = text });
            }

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string requestBody = serializer.Serialize(body);

            using (var client = this.GetClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", this.key);
                request.Headers.Add("X-ClientTraceId", Guid.NewGuid().ToString());

                var response = client.SendAsync(request).Result;

                var responseBody = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    this.HandleApiError(responseBody);
                }

                dynamic result = serializer.DeserializeObject(responseBody);
                var translations = new List<string>();
                for (int i = 0; i < input.Count(); i++)
                {
                    // currently Sitefinity does not support sending multiple languages at once, only multiple strings
                    var translation = result[i]["translations"][0]["text"];
                    translations.Add(translation);
                }

                return translations;
            }
        }

        private static string GetTranslаteArgumentExceptionMessage(string paramName, Type paramType)
        {
            return string.Format(Constants.InvalidParameterForAzureTransaltionRequestExceptionMessageTemplate, paramName, paramType);
        }

        private void HandleApiError(string responseBody)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            dynamic jsonResponse = serializer.DeserializeObject(responseBody);
            if (jsonResponse["error"] != null)
            {
                throw new AzureTranslatorException(jsonResponse["error"]["message"]);
            }
        }

        private string key;

        internal class Constants
        {
            internal const string Name = "AzureTranslatorTextConnector";
            internal const string InvalidApiKeyExceptionMessage = "Invalid API subscription keys.";
            internal const string NoApiKeyExceptionMessage = "No API key configured for azure translations connector.";
            internal const string InvalidParameterForAzureTransaltionRequestExceptionMessagePrefix = "Invalid parameter for azure translation request.";
            internal const string InvalidParameterExceptionMessageTemplate = "Parameter with name {0} of type {1} cannot be null or empty.";
            internal const string Title = "Azure Translator Text Connector";
            internal const string TEXT_TRANSLATION_API_ENDPOINT = "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0";
            internal const int ValidApiKeyLength = 32;
            internal static readonly string InvalidParameterForAzureTransaltionRequestExceptionMessageTemplate = InvalidParameterForAzureTransaltionRequestExceptionMessagePrefix + " " + InvalidParameterExceptionMessageTemplate;
        }

        internal struct Parameters
        {
            internal const string ApiKey = "apiKey";
        }
    }
}