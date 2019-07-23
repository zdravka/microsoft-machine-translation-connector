using System;
using System.Runtime.Serialization;

namespace Progress.Sitefinity.Translations.MicrosoftMachineTranslatorConnector.Exceptions
{
    [Serializable]
    internal class MicrosoftTranslatorConnectorSerializationException : MicrosoftTranslatorConnectorException
    {
        public MicrosoftTranslatorConnectorSerializationException()
        {
        }

        public MicrosoftTranslatorConnectorSerializationException(string message) : base(message)
        {
        }

        public MicrosoftTranslatorConnectorSerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MicrosoftTranslatorConnectorSerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}