using Telerik.Sitefinity.Translations;

namespace Progress.Sitefinity.Translations.MicrosoftMachineTranslatorConnector.Tests
{
    internal class MockedTranslationOptions : ITranslationOptions
    {
        public string SourceLanguage
        {
            get; set;
        }

        public string TargetLanguage
        {
            get; set;
        }
    }
}
