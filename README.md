# Progress.Sitefinity.Translations.AzureTranslator

### Latest supported version: Sitefinity CMS 12.0.7000.0

When working with the Sitefinity CMS *Translation* module, you can benefit from a number of translation connectors that you use out-of-the-box with minimum setup. You can, however, implement your own translation connector with custom logic to serve your requirements. 

This tutorial provides you with a sample that you use to implement a custom translation connector to work with the **Azure Translator Text** service. You first create and setup the connector and then use the *Translation* API to implement the overall translation process.   
## Requirements

You must have a Sitefinity CMS license

For more information, see the [System requirements](https://docs.sitefinity.com/system-requirements) for the  respective Sitefinity CMS version.

## Prerequisites

Your Sitefinity CMS web site must be in multilingual mode meaning that you have added atleast one additinal language to the current website you are browsing. Otherwise you will not see the translations screen in the administrations tab of your application.

You should either use country specific languages like 'en-US' and not just 'en' or specify a mapping between the country invariant and country specific language in the translations advanced settings screen: <i>Administration >> Settings >> Advanced >> Culture mappings </i> text box.

TODO


## Create and configure the connector

TODO

##API Overview: AzureTranslatorConnector
The <strong>AzureTranslatorConnector</strong> class has properties that hold information about the connector. The following table summarizes these API properties.

**NOTE:** In this tutorial, you do not use all of the methods listed below. You can use the table below as reference, as well.
#### AzureTranslatorConnector specific properties

TODO