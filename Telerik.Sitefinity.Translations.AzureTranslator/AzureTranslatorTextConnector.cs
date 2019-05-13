using System.Collections.Generic;
using System.Collections.Specialized;

namespace Telerik.Sitefinity.Translations.AzureTranslator
{
    public class AzureTranslatorTextConnector : MachineTranslationConnector
    {
        protected override void InitializeConnector(NameValueCollection config)
        {
            throw new System.NotImplementedException();
        }

        protected override List<string> Translate(List<string> input, ITranslationOptions translationOptions)
        {
            throw new System.NotImplementedException();
        }

        internal class Constants
        {
            internal const string Name = "AzureTranslatorTextConnector";
            internal const string Title = "Azure Translator Text Connector";           
        }

        internal struct Parameters
        {
            internal const string ApiKey = "apiKey";
        }
    }
}
