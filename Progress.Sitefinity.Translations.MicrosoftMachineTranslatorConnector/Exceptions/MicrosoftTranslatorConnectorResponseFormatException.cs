using System;
using System.Runtime.Serialization;

namespace Progress.Sitefinity.Translations.MicrosoftMachineTranslatorConnector.Exceptions
{
    [Serializable]
    internal class MicrosoftTranslatorConnectorResponseFormatException : MicrosoftTranslatorConnectorException
    {
        public MicrosoftTranslatorConnectorResponseFormatException()
        {
        }

        public MicrosoftTranslatorConnectorResponseFormatException(string message) : base(message)
        {
        }

        public MicrosoftTranslatorConnectorResponseFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MicrosoftTranslatorConnectorResponseFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}