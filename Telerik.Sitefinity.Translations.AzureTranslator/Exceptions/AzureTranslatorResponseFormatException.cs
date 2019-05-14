using System;
using System.Runtime.Serialization;

namespace Telerik.Sitefinity.Translations.AzureTranslator.Exceptions
{
    [Serializable]
    internal class AzureTranslatorResponseFormatException : AzureTranslatorException
    {
        public AzureTranslatorResponseFormatException()
        {
        }

        public AzureTranslatorResponseFormatException(string message) : base(message)
        {
        }

        public AzureTranslatorResponseFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AzureTranslatorResponseFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}