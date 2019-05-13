using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Translations.Configuration;

namespace Telerik.Sitefinity.Translations.AzureTranslator
{
    public static class Startup
    {
        /// <summary>
        /// Called before the Asp.Net application is started. Subscribes for the application start events to add configuration.
        /// </summary>
        public static void OnPreApplicationStart()
        {
            Bootstrapper.Initialized -= Startup.Bootstrapper_Initialized;
            Bootstrapper.Initialized += Startup.Bootstrapper_Initialized;            
        }

        private static void Bootstrapper_Initialized(object sender, ExecutedEventArgs e)
        {
            if (SystemManager.GetModule("Translations") != null)
            {
                var configManager = ConfigManager.GetManager();
                var translationsConfig = configManager.GetSection<TranslationsConfig>();

                if (!translationsConfig.Connectors.ContainsKey(AzureTranslatorTextConnector.Constants.Name))
                {
                    translationsConfig.Connectors.Add(new ConnectorSettings(translationsConfig.Connectors)
                    {
                        ConnectorType = typeof(AzureTranslatorTextConnector).FullName,
                        Enabled = true,
                        Name = AzureTranslatorTextConnector.Constants.Name,
                        Title = AzureTranslatorTextConnector.Constants.Title,
                        Parameters = { { AzureTranslatorTextConnector.Parameters.ApiKey, string.Empty } }
                    });

                    configManager.SaveSection(translationsConfig, true);
                }
            }
        }
    }
}
