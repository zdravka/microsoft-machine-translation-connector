using System;
using System.Runtime.Serialization;

namespace Telerik.Sitefinity.Translations.AzureTranslator
{
    /// <summary>
    /// Exception thrown when AzureTransaltorClient requests to Azure service receive error from server.
    /// </summary>
    [Serializable]
    public class AzureTranslatorException : Exception
    {
        public AzureTranslatorException()
        {
        }

        public AzureTranslatorException(string message) : base(message)
        {
        }

        public AzureTranslatorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AzureTranslatorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}