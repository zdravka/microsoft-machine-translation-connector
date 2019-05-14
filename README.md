# Progress.Sitefinity.Translations.AzureTranslator

TODO Update Version
### Latest supported version: Sitefinity CMS 12.0.7000.0

When working with the Sitefinity CMS *Translation* module, you can benefit from a number of translation connectors that you use out-of-the-box with minimum setup.

This repo contains a translation connector to work with the **[Azure Translator Text](https://azure.microsoft.com/en-us/services/cognitive-services/translator-text-api/)** V3 API service. 


## Requirements

### Sitefinity

For more information, see the [System requirements](https://docs.sitefinity.com/system-requirements) and [Transaltion procedure](https://www.progress.com/documentation/sitefinity-cms/translation-procedure) for the  respective Sitefinity CMS version.

### Azure

You must **[Sign up for Azure Translator Text API V3](https://docs.microsoft.com/en-us/azure/cognitive-services/translator/translator-text-how-to-signup)** service in Azure. Then you can use one of the generated API Keys there to configure the connector in Sitefinity. You can use the free tier as well. It supports up to 2 million characters per month for free with no automatic billing afterwards.

## Configure the connector

Using one of the generated API keys after **[sign up for Azure Translator Text API](https://docs.microsoft.com/en-us/azure/cognitive-services/translator/translator-text-how-to-signup)** service in Azure configure the following setting:
*Administration >> Settings >> Advanced >> Translations >> Connectors >> Azure Translator Text Connector >> parameters >> apiKey*

**Warning** For proper transaltion of whole paragraphs of text, **do NOT** set the *Strip HTML tags* parameter to *True* (under *Administration >> Settings >> Advanced >> Translations >> Connectors >> Azure Translator Text Connector*).

## Supported languages

You can check out the official [Azure language support documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/translator/language-support#translation). The connector uses the v3 API.
