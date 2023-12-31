# NewShoreAIR.Api.Client <!-- omit in toc -->

- [Overview](#overview)
- [C# Client](#c-client)
  - [Getting Started](#getting-started)
  - [Client registration](#client-registration)
  - [Publish Generted Client Package](#publish-generted-client-package)
- [Typescript Client](#typescript-client)

## Overview

This project is responsible to generate the following clients using [Nswag](https://github.com/RicoSuter/NSwag) :

* C# Client
* Typescript Client

## C# Client

### Getting Started


```bash
Install-Package NewShore.Api.Client
```

### Client registration

Create a section in your `appsettings.json`


Configure services:

```c#
//Register NewShoreConfiguration class that is used by generated clients
services.Configure<NewShoreConfiguration>(configuration.GetSection("NewShoreService"));

// Register YourGeneratedClient as Typed Client
services.AddHttpClient<IYourGeneratedClient, YourGeneratedClient>();
```

### Publish Generted Client Package

Manual publication:

1. Build `NewShore.Api.Client.csproj` in order to generate `Client.g.cs` and `Contracts.g.cs` files. This files under Autogenerated folder must be commit into the repository.
2. Build again the project with Release configuration in order to generate the corresponding nuget package `NewShore.Api.Client.2.12.3.nupkg` in \bin\Release folder.
3. Execute the following command in \bin\Release folder:

   `nuget push NewShore.Api.Client.2.12.3.nupkg -Source Artifactory`

## Typescript Client

> TBD
