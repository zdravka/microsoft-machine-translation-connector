namespace Telerik.Sitefinity.Translations.AzureTranslator.Tests
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
