namespace Telerik.Sitefinity.Translations.AzureTranslator
{
    internal class Constants
    {
        internal const string Name = "AzureTranslatorTextConnector";
        internal const string Title = "Azure Translator Text Connector";
        internal const string AzureTranslateApiEndpointUrl = "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0";
        internal const int ValidApiKeyLength = 32;

        internal class ExceptionMessages
        {
            internal const string InvalidApiKeyExceptionMessage = "Invalid API subscription key.";
            internal const string NoApiKeyExceptionMessage = "No API key configured for azure translations connector.";
            internal const string InvalidParameterForAzureTransaltionRequestExceptionMessagePrefix = "Invalid parameter for azure translation request.";
            internal const string NullOrEmptyParameterExceptionMessageTemplate = "Parameter with name {0} cannot be null or empty.";
            internal static readonly string InvalidParameterForAzureTransaltionRequestExceptionMessageTemplate = InvalidParameterForAzureTransaltionRequestExceptionMessagePrefix + " " + NullOrEmptyParameterExceptionMessageTemplate;
            internal static readonly string UnexpectedErrorResponseFormat = $"An error ocurred on the Azure server. {Constants.ExceptionMessages.UnexpectedResponseFormat}";
            internal const string UnexpectedResponseFormat = "The response received was not in the expected format.";
        }

        internal struct ConfigParameters
        {
            internal const string ApiKey = "apiKey";
        }
    }
}