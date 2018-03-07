# Customer Finder
Customer Finder by [PTI Costa Rica](https://www.pticostarica.com) is a Cloud-Based system which has the objetive to help Entrepreneurs and SMBs to identify Potential Customers, by leveraging the power of their Social Media Profiles.

The first phase of the project uses Twitter only.

Customer Finder allows users to gather information of their First and Second Level followers.
With Customer Finder, users can search for contacts on Education, Engineering, Medicine, Videogames, and more.

Customer Finder also allows users to get insights of what their network is talking about,
which words are being used the most.

Customer Finder allows users to get insights of what a specific individual in their network is talking about, and also allows users to identify the possible general mood of a contact.

You can see more more about Customer Finder on our [Facebook Page](https://www.facebook.com/Customer-Finder-by-PTI-Costa-Rica-299490810415132/), feel free to like it and share it.

## Getting Started
For development it is required to have the following
* [Microsoft Visual Studio 2017](https://www.visualstudio.com/downloads/)
* [Microsoft Azure SDK 2.9](http://www.microsoft.com/downloads/details.aspx?FamilyID=ebf6e0a3-3494-4514-bcb8-b68b44e4a692)


## Configuring the System - Cloud Service

Download the latest version from the master branch.

* Create an SQL Database, you can use either an Azure SQL database or a SQL on premise.
* Create a Twitter application. [You can do it here](https://apps.twitter.com/)
* Set CustomerFinderCloudService.ccproj as a Start Project.
* In CustomerFinderCloudService.ccproj open the file ServiceConfiguration.Cloud.cscfg to edit it.
* Fill the following configuration settings with your respective values
  * Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString
  * APPINSIGHTS_INSTRUMENTATIONKEY
  * TwitterApp_ConsumerKey
  * TwitterApp_ConsumerSecret
  * TwitterApp_AccessToken
  * TwitterApp_AccessTokenSecret
  * CustomerFinderContext
  * Twitter_Username
  * TextAnaliticsKey
  * Notifications_ToEmailAddress
*  Open the app.config file.
*  Set the configuration for mailSettings
*  Open the properties for CustomerFinderCloudService.ccproj
*  Under Developement, set Service Configuration to Cloud
*  Deploy the Database Schema using the Database Project CustomerFinderDB.sqlproj
*  Run the application

## Configuring the System - Power BI
* Get Power BI. [You can do it here](https://portal.office.com/partner/partnersignup.aspx?type=Purchase&id=823604c2-4847-4219-9afd-177fb32dc8ab&msppid=4227824)