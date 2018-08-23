# Elaborate Hybrid Connection Demo/Example

This repo holds three different applications that work together along with configuration in Azure to demonstrate how you can have a container application in Kubernetes call to an API hosted in App Services that then brokers some API operation/calls to an on-premise endpoint system (in this case Azure Functions) using the App Services Hybrid Connections feature (based on Azure Relay).

Components are:
- A NET Core 2.0 Razor Web App that runs in a linux container on Azure Kubernetes Services
- A Net Core 2.0 MVC Web API app that runs in Azure App Services and acts as a resource called by the containerized app above
- An on premise Azure Functions app (running preview 2.0 version) that runs on a local machine on a local port 7071 (by default).  The Web API app above makes calls through Hybrid Connections of App Services to this on premise app.
- An Azure SQL Database that has a table with the following schema (Note this could be removed as a dependency and replaced with just mocked local JSON API responses in the local Functions app.)
- ClientLog Table has 4 Columns:   SerialNumber, ActivationDate, Comments, SentTime to script the generation of the table & columns you can use this: 

```
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
    CREATE TABLE [dbo].[ClientLog](
      [SerialNumber] [nvarchar](50) NULL,
      [ActivationDate] [nvarchar](50) NULL,
      [Comments] [nvarchar](max) NULL,
      [SentTime] [nvarchar](50) NULL
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO
```

A Diagram of the solution is in the Word document at the root of this repo and at the bottom of this readme.

Notes & Commments:
- You don't have to use all tiers and you can pick and choose which solutions / projects you intend to leverage
- All these projects were based on generic examples from Microsoft and were authed in VS 2017 though you could leverage VS Code or another IDE 

High Level Steps to setup:
  1. If using Azure SQL create a small Azure SQL single database instance, get the Azure DB service URL/FQDN, a username & pwd, use the script above to create a ClientLog table in your database
  2. Start with the local functions app and get it running locally in VS or VS Code or etc and validate that you are able to call the 2 operations in that application (read operation PullRecords and write operation PersistRecord).  You will need to set the connection string in the local functions code to your Azure SQL DB generated above.  (Alternatively you can replace the code in each operation to return hard coded stub values vs calling out to Azure SQL DB)
  3. Once you have the local functions app working on the local port then add the App Services Web API App by publishing it to App Services and then testing the following generic API operations:
     - {APP Service FQDN} /api/values = should return a simple list of 2 values that are hard coded
     - /api/todo = should return a list of ToDo tasks (json) that are hard coded (3)
     - /api/values/DeviceRecords = this should invoke an attempt to call the Hybrid Connection to invoke the PullRecords API operation on the locally runnin Azure Functions project (note this will return an error if the connection is not up OR the functions project/app is not running)
     - /api/values/logsn/{serialnumber}  (i.e. /api/values/logsn/4978FFG00 =  this should again invoke the Hybrid Connection but this time calling the PersistRecord API write operation published on the local Azure Function
  4. Setup a Hybrid Connection on your App Service App that is running the Web API.  To do this click on Networking properties and setup the Hybrid Connection (ref article: https://docs.microsoft.com/en-us/azure/app-service/app-service-hybrid-connections)


[![Hybrid Connection Diagram](/HybridConnDiagram.png)](https://github.com/thekwilson/HybridToDo/blob/master/Hybrid%20Connection%20Diagrams.docx)
