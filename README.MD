# GlobalPollenProject v1

The Global Pollen Project is a Web App that enables
1. The crowdsourcing of pollen identification using images; and
2. The digitisation of existing pollen reference material.

The project has been described in detail in:
Martin and Harvey (2017). The Global Pollen Project: A New Tool for Pollen Identification and the Dissemination of Physical Reference Collections. Methods in Ecology and Evolution.

## Technical Basis
The current version (v1) of the GPP is a .Net Core Web App, hosted on Azure. 

## Running GPP locally
As the GPP is a .Net Core web app, it can be run on any Linux, Windows, or macOS computer that has .Net Core 1.1 installed. 
To run, you must set up an appsettings.json in the root of GlobalPollenProject.WebUI, containing the following parameters:

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
  "AzureConnectionString": ""
}

### Local Taxonomic Backbone
The GPP currently implements a local taxonomic backbone. 
We have used The Plant List for this purpose. 
You will need to populate this locally to be able to import taxa into GPP.

## Contributing to Development
Please note that version one of the GPP is to be superseded by a new, rewritten version. 
This is currently hosted in the AndrewIOM/gpp-cqrs repository.