using System;
using System.Runtime.Serialization;

namespace Progress.Sitefinity.Translations.MicrosoftMachineTranslatorConnector.Exceptions
{
    /// <summary>
    /// Exception thrown when MicrosoftTransaltorClient requests to Microsoft service receive error from server.
    /// </summary>
    [Serializable]
    public class MicrosoftTranslatorConnectorException : Exception
    {
        public MicrosoftTranslatorConnectorException()
        {
        }

        public MicrosoftTranslatorConnectorException(string message) : base(message)
        {
        }

        public MicrosoftTranslatorConnectorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MicrosoftTranslatorConnectorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}