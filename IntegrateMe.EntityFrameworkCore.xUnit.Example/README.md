# IntegrateMe.EntityFrameworkCore Example Project for xUnit

This project serves as an example implementation of the IntegrateMe framework, specifically showcasing its capabilities
for integrating with Entity Framework Core in an xUnit testing environment.
The purpose of this project is to provide clear and concise examples of how to write integration tests for Entity
Framework Core applications using the IntegrateMe framework, enabling developers to validate their database interactions
intuitively and efficiently.

## Project Structure

The example project is organized into several key components:

* EntityFrameworkCoreExamples: Contains the example tests that demonstrate how to write integration tests for Entity
  Framework Core using the IntegrateMe framework.
    * CustomEntityFrameworkCoreExample.cs: Demonstrates how to write custom actions that can be executed on the
      DbContext during testing.
    * GetConnectionStringEntityFrameworkCoreExample.cs: Showcases the usage of the GetConnectionStringStep to retrieve
      the connection string from the DbContext.

## Key Features

* Simple Integration Testing: The project demonstrates how to write integration tests for Entity Framework Core using
  the IntegrateMe framework, allowing developers to validate their data interactions easily.
* Asynchronous and Synchronous Testing: Tests can be written using both async and sync styles, thanks to the intuitive
  syntax of the framework.
* Custom Actions: Shows how to define custom actions that can be executed on the DbContext, providing developers with
  flexibility in their testing scenarios.

## Getting Started

To run the example tests, clone this repository and ensure you have the necessary dependencies installed.
The tests are structured to work seamlessly with xUnit, providing a clear starting point for writing your own
integration tests using the IntegrateMe framework with Entity Framework Core.

Explore the example tests to learn how to utilize IntegrateMe for your own projects and improve your integration testing
capabilities!