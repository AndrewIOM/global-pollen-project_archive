**Note. This repository contains the as-published / peer-reviewed version of the Global Pollen Project from 2017. It has been superseded by [AndrewIOM/global-pollen-project](https://github.com/AndrewIOM/global-pollen-project/).**

# [GlobalPollenProject](https://globalpollenproject.org) v1
*Key Words: Pollen; Palynology; Reference-Collection; Digitisation; Dissemination; Crowdsourcing*

[![DOI](https://zenodo.org/badge/80122758.svg)](https://zenodo.org/badge/latestdoi/80122758)

The Global Pollen Project is a web-based tool to enable:

1. **crowdsourcing** of pollen identification using images; and

2. **digitisation** of existing pollen reference material.


The ideas behind the tool, and its development, are discussed in detail in the following paper:

Martin A.C., and Harvey, W. J. (2017). The Global Pollen Project: A New Tool for Pollen Identification and the Dissemination of Physical Reference Collections. *Methods in Ecology and Evolution*. **Accepted**

## Technologies Used
Version 1 of the Global Pollen project (GPP) is a .Net Core web app, using a MSSQL database. 
The live version is hosted on Azure at present.

## Contributing to Development
We enthusiastically welcome pull requests and issues.

**Please note that version one of the GPP is to be superseded by a new, rewritten version. 
This is currently hosted in the [AndrewIOM/gpp-cqrs](https://github.com/AndrewIOM/gpp-cqrs) repository.**

### Running GPP locally
As the GPP is a .Net Core web app, it can be run on any Linux, Windows, or macOS computer that has .Net Core 1.1 installed. 
To run, you must set up an appsettings.json in the root of GlobalPollenProject.WebUI, containing the following parameters:

```json
{
  "Account": {
    "Admin": {
      "DefaultAdminUserName": "",
      "DefaultAdminPassword": ""
    }
  },
  "Authentication": {
    "Facebook": {
      "AppId": "",
      "AppSecret": ""
    },
    "MicrosoftAccount": {
      "ClientId": "",
      "ClientSecret": ""
    },
    "Twitter": {
      "ConsumerKey": "",
      "ConsumerSecret": ""
    }
  },
  "SendGridKey": "",
  "SendGridUser": "",
  "AzureConnectionString": "", 
  "AzureImageContainer": "" 
}
```

Admin Account: You must use an email address (e.g. x@y.com) for the Admin user name.

Authentication: These settings are optional, but required for 3rd party logins.

SendGrid: This service is currently used to send emails. Without configuring these settings, emails will not work.

AzureConnectionString + AzureImageContainer: Azure blob storage is used in this implementation for image storage. You need to configure an Azure storage account using these settings.

### Local Taxonomic Backbone
The GPP currently implements a local taxonomic backbone. 
We have used The Plant List for this purpose. 
You will need to populate this locally to be able to import taxa into GPP.
To do this, obtain the text-based, tab-delimited archive of The Plant List (2013). This can be imported using the Admin interface of the web app.

Visit the [Global Pollen Project](https://globalpollenproject.org).
