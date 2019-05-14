﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

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
            var key = config.Get(Constants.ConfigParameters.ApiKey);
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(Constants.ExceptionMessages.NoApiKeyExceptionMessage);
            }

            if (key.Length != Constants.ValidApiKeyLength)
            {
                throw new ArgumentException(Constants.ExceptionMessages.InvalidApiKeyExceptionMessage);
            }

            this.key = key;
        }

        protected override List<string> Translate(List<string> input, ITranslationOptions translationOptions)
        {
            if (translationOptions == null)
            {
                throw new ArgumentException(GetTranslаteArgumentExceptionMessage(nameof(translationOptions)));
            }

            var fromLanguageCode = translationOptions.SourceLanguage;
            var toLanguageCode = translationOptions.TargetLanguage;

            if (string.IsNullOrWhiteSpace(fromLanguageCode))
            {
                throw new ArgumentException(GetTranslаteArgumentExceptionMessage($"{nameof(translationOptions)}.{nameof(translationOptions.SourceLanguage)}"));
            }

            if (string.IsNullOrWhiteSpace(toLanguageCode))
            {
                throw new ArgumentException(GetTranslаteArgumentExceptionMessage($"{nameof(translationOptions)}.{nameof(translationOptions.TargetLanguage)}"));
            }

            if (input == null || input.Count == 0)
            {
                throw new ArgumentException(GetTranslаteArgumentExceptionMessage(nameof(input)));
            }

            if (fromLanguageCode == toLanguageCode)
            {
                return input;
            }

            string uri = string.Format(Constants.AzureTranslateApiEndpointUrl + "&from={0}&to={1}", fromLanguageCode, toLanguageCode);

            var body = new List<object>();
            foreach (var text in input)
            {
                body.Add(new { Text = text ?? string.Empty });
            }

            var serializer = new JavaScriptSerializer();
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
                    this.HandleApiError(responseBody, response);
                }

                dynamic result = serializer.DeserializeObject(responseBody);
                var translations = new List<string>();
                try
                {
                    for (int i = 0; i < input.Count(); i++)
                    {
                        // currently Sitefinity does not support sending multiple languages at once, only multiple strings
                        var translation = result[i]["translations"][0]["text"];
                        translations.Add(translation);
                    }
                }
                catch (Exception)
                {
                    throw new AzureTranslatorException($"{Constants.ExceptionMessages.UnexpectedResponseFormat} Server response: {response.StatusCode} {response.ReasonPhrase} {responseBody}");
                }

                return translations;
            }
        }

        private static string GetTranslаteArgumentExceptionMessage(string paramName)
        {
            return string.Format(Constants.ExceptionMessages.InvalidParameterForAzureTransaltionRequestExceptionMessageTemplate, paramName);
        }

        private void HandleApiError(string responseBody, HttpResponseMessage response)
        {
            var serializer = new JavaScriptSerializer();
            try
            {
                dynamic jsonResponse = serializer.DeserializeObject(responseBody);
                throw new AzureTranslatorException(jsonResponse["error"]["message"]);
            }
            catch (Exception)
            {
                throw new AzureTranslatorException($"{Constants.ExceptionMessages.UnexpectedErrorResponseFormat} Server response: {response.StatusCode} {response.ReasonPhrase} {responseBody}");
            }
        }

        private string key;

    }
}