# IntegrateMe

IntegrateMe is a powerful framework designed to simplify the process of writing asynchronous integration tests in a
highly intuitive, synchronous-style syntax.
Built for .NET 8, it provides developers with an easy and clean way to test integrations with various Azure services and
databases, without getting bogged down by complex async code.
This framework allows you to focus on what matters: validating your integrations.

# Table of Contents
- [Get Started](#get-started)
  - [Installation](#installation)
- [Writing Tests](#writing-tests)
- [Capabilities](#capabilities)
- [Limitations](#limitations)
- [Examples](#examples)
  -[Azure Blob Storage](#azure-blob-storage)
  - [Azure Container Apps](#azure-container-apps)
  - [Azure Container Registry](#azure-container-registry)
  - [Azure Logic Apps](#azure-logic-apps)
- [Alternative Applications of IntegrateMe](#alternative-applications-of-integrateme)

## Get Started

### Installation

To start using IntegrateMe in your project, you’ll need to install the core NuGet package and any additional connectors
for the Azure services you intend to test.

1. Install the core package:

`dotnet add package IntegrateMe`

2. Add the specific connectors for Azure services or Entity Framework Core as needed:

For Azure Blob Storage:
`dotnet add package IntegrateMe.Azure.BlobStorage`

For Azure Container Apps:
`dotnet add package IntegrateMe.Azure.ContainerApp`

For Azure Container Registry:
`dotnet add package IntegrateMe.Azure.ContainerRegistry`

For Azure Logic Apps:
`dotnet add package IntegrateMe.Azure.LogicApp`

For Entity Framework Core:
`dotnet add package IntegrateMe.EntityFrameworkCore`

3. Once installed, reference the framework in your test project and start writing your integration tests.

## Writing Tests

## Capabilities

## Limitations

## Examples

## Alternative Applications of IntegrateMe

