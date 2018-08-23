# Elaborate Hybrid Connection Demo/Example

This repo holds three different applications that work together along with configuration in Azure to demonstrate how you can have a container application in Kubernetes call to an API hosted in App Services that then brokers some API operation/calls to an on-premise endpoint system (in this case Azure Functions) using the App Services Hybrid Connections feature (based on Azure Relay).

Components are:
- A NET Core 2.0 Razor Web App that runs in a linux container on Azure Kubernetes Services
- A Net Core 2.0 MVC Web API app that runs in Azure App Services and acts as a resource called by the containerized app above
- An on premise Azure Functions app (running preview 2.0 version) that runs on a local machine on a local port 7071 (by default).  The Web API app above makes calls through Hybrid Connections of App Services to this on premise app.

A Diagram of the solution is in the Word document at the root of this repo.   

![Hybrid Connection Diagram](/HybridConnDiagram.png)
