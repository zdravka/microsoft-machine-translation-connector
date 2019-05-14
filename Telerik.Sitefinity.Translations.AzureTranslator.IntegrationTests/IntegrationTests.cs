using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace Telerik.Sitefinity.Translations.AzureTranslator.IntegrationTests
{
    [TestClass]
    [TestCategory("Integration Tests")]
    public class IntegrationTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            var client = new RestClient(this.BaseUrl + "/Sitefinity/Authenticate/OpenID/connect/token");
            var request = new RestRequest(Method.POST);

            // Make sure you have add this client to the authentication config.
            request.AddParameter("auth", "username=admin%40test.test&password=admin%402&grant_type=password&scope=openid&client_id=integration-tests&client_secret=secret", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200)
            {
                var results = JsonConvert.DeserializeObject<dynamic>(response.Content);
                accessToken = results.access_token;
            }
        }

        [TestMethod]
        public void SendNewsForTranslation()
        {
            var newsId = this.CreateNewsItem(Guid.NewGuid().ToString(), accessToken);
            this.PublishNews(newsId, accessToken);
            this.SendForTranslation(newsId);

            int sentItems = this.GetSentForTranslationItemsCount();
            Assert.AreEqual(1, sentItems);
        }

        private Guid CreateNewsItem(string title, string accessToken)
        {
            var client = new RestClient(this.BaseUrl + "/sf/system/newsitems");
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "Bearer " + accessToken);
            request.AddHeader("x-sf-service-request", "true");
            request.AddHeader("content-type", "application/json");

            var body = new JObject
            {
                { "Title", Guid.NewGuid() }
            };

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            Assert.AreEqual(201, (int)response.StatusCode);

            var results = JsonConvert.DeserializeObject<dynamic>(response.Content);
            var newsId = results.Id;

            return newsId;
        }

        private void PublishNews(Guid newsId, string accessToken)
        {
            var client = new RestClient(this.BaseUrl + "/sf/system/newsitems" + "(" + newsId + ")" + "/operation");
            var publishRequest = new RestRequest(Method.POST);
            publishRequest.AddHeader("authorization", "Bearer " + accessToken);
            publishRequest.AddHeader("x-sf-service-request", "true");
            publishRequest.AddHeader("content-type", "application/json");

            publishRequest.AddParameter("application/json", "{\n    action: \"Publish\",\n    actionParameters: {\n    }\n}", ParameterType.RequestBody);
            IRestResponse publishResponse = client.Execute(publishRequest);

            Assert.AreEqual(200, (int)publishResponse.StatusCode);
        }

        private void SendForTranslation(Guid newsId)
        {
            // You need to enable basic authentication in authentication config to use this service.
            var client = new RestClient(this.BaseUrl + "/restapi/translations/translate/markandsend")
            {
                Authenticator = new HttpBasicAuthenticator(this.Username, this.Password)
            };

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            // TODO: This should be modified to accept different providers, content types, languagues etc. It is hardcoded for now.
            request.AddParameter("undefined", "{\"Ids\":[\"" + newsId + "\"],\"ProviderName\":\"newsProvider2\",\"ItemType\":\"Telerik.Sitefinity.News.Model.NewsItem\",\"SourceLanguage\":\"en\",\"TargetLanguages\":[\"de\"],\"ActualSourceLanguage\":\"en\",\"ProjectData\":{\"connector\":\"" + ConnectorName + "\",\"actualSourceLanguage\":\"en\"}}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            Assert.AreEqual(200, (int)response.StatusCode);
        }

        private int GetSentForTranslationItemsCount()
        {
            int count = 0;

            var client = new RestClient(this.BaseUrl + "/restapi/translations/translations/count?translationStatus=Sending,+Sent")
            {
                Authenticator = new HttpBasicAuthenticator(this.Username, this.Password)
            };

            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200)
            {
                var results = JsonConvert.DeserializeObject<dynamic>(response.Content);
                count = results.TotalCount;
            }

            return count;
        }

        private static string accessToken;
        private const string ConnectorName = "AzureTranslatorTextConnector";

        private string BaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["baseUrl"];
            }
        }

        private string Username
        {
            get
            {
                return ConfigurationManager.AppSettings["username"];
            }
        }

        private string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["password"];
            }
        }
    }
}
