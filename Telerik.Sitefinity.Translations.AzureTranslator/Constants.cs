namespace Telerik.Sitefinity.Translations.AzureTranslator
{
    internal class Constants
    {
        public const string Name = "AzureTranslatorTextConnector";
        public const string Title = "Azure Translator Text Connector";
        public const int ValidApiKeyLength = 32;

        internal class ExceptionMessages
        {
            public const string InvalidApiKeyExceptionMessage = "Invalid API subscription key.";
            public const string NoApiKeyExceptionMessage = "No API key configured for azure translations connector.";
            public const string InvalidParameterForAzureTransaltionRequestExceptionMessagePrefix = "Invalid parameter for azure translation request.";
            public const string NullOrEmptyParameterExceptionMessageTemplate = "Parameter with name {0} cannot be null or empty.";
            public static readonly string InvalidParameterForAzureTransaltionRequestExceptionMessageTemplate = InvalidParameterForAzureTransaltionRequestExceptionMessagePrefix + " " + NullOrEmptyParameterExceptionMessageTemplate;

            public static readonly string UnexpectedErrorResponseFormat = $"{AzureServerErrorMessage} {UnexpectedResponseFormat}";
            public static readonly string ErrorSerializingErrorResponseFromServer = $"{AzureServerErrorMessage} {ErrorSerializingResponseFromServer}";
            public static readonly string ErrorSerializingResponseFromServer = "Could not serialize response from Azure.";
            public const string UnexpectedResponseFormat = "The response received was not in the expected format.";

            public const string AzureServerErrorMessage = "An error ocurred on the Azure server.";
        }

        internal struct ConfigParameters
        {
            public const string ApiKey = "apiKey";
        }

        internal struct AzureTransalteEndpointConstants
        {
            public const string EndpointUrl = "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0";
            public const string TextTypeQueryParam = "textType";
            public const string TargetCultureQueryParam = "to";
            public const string SourceCultureQueryParam = "from";
        }
    }
}