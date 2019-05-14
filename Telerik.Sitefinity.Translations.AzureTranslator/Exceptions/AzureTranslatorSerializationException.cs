using System;
using System.Runtime.Serialization;

namespace Telerik.Sitefinity.Translations.AzureTranslator.Exceptions
{
    [Serializable]
    internal class AzureTranslatorSerializationException : AzureTranslatorException
    {
        public AzureTranslatorSerializationException()
        {
        }

        public AzureTranslatorSerializationException(string message) : base(message)
        {
        }

        public AzureTranslatorSerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AzureTranslatorSerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}