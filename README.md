# SHIP MANAGEMENT API VERSION 1.0

RESTFull API Service to perform CRUD (Create, Read, Update & Delete) operations on a ship.

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Packages](#packages)
  - [Installation](#installation)
  - [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Folder Structure](#folder-structure)

## Getting Started

A ASP.NET Core Web API RESTful service. It includes basic CRUD operations on a (Ship Management System).

Each ship must have a name (string), length (in meters), width (in meters), and code.

### Prerequisites

    1. Downlaod for Windows

    ```shell
    - [.NET Core SDK](https://dotnet.microsoft.com/download) installed
    ```

    2. CLI

    onMac

    ```shell
        brew install --cask dotnet-sdk
    ```

### Packages

#### Project - version details

    1. net8.0
    2. Swashbuckle.AspNetCore 6.4.0
    3. AspNetCore.OpenApi 8.0.0
    4. AspNetCore.Mvc.Versioning 5.1.0

#### Unit Test - version details

    1. NET.Test.Sdk 17.6.0
    2. xunit 2.4.2
    3. xunit.runner.visualstudio 2.4.5
    4. AutoFixture 4.18.1
    5. FluentAssertions 6.12.0
    6. Moq 4.20.70

### Installation

1. Clone the repository:

Open your terminal or command prompt, go to the desired directory, and use the following command to clone the .NET Core Web API project:

    ```shell
    git clone https://github.com/yourusername/your-webapi-repo.git
    cd your-webapi-repo
    ```

2. Build the project:

   ```shell
   dotnet build
   ```

### Running the Application

Run your .NET Core Web API using the following command:

    ```shell
    cd .\src\ShipService.Api\

    dotnet run or dotnet watch run
    ```

## API Documentation

### Crete API

#### Request

    ```yaml
    POST  {{host}}/ship/create
    Content-Type: application/json
    ```

    ```http
        {
        "name":"LongShip",
        "length": 101,
        "width": 202,
        "code": "AAAA-1234-E5"
        }
    ```

#### Response

### List API

#### Request

    ```yaml
    GET  {{host}}/ship/list
    Content-Type: application/json
    ```

#### Response

    ```http
        {
            "id":""
            "name":"LongShip",
            "length": 101,
            "width": 202,
            "code": "AAAA-1234-E5"
        }
    ```

### Update API

#### Request

    ```yaml
    PUT  {{host}}/ship/update/{Id}
    Content-Type: application/json
    ```

    ```http
        {
            "name": "WhiteShip",
            "length": 120,
            "width": 122,
            "code": "AAAA-1111-A1"
        }
    ```

#### Response

### Delete API

#### Request

    ```yaml
    DELETE  {{host}}/ship/delete/{id}
    Content-Type: application/json
    ```

## Folder Structure

### --src

![Folder structure](assets/CleanArchitectureFolderStucture.jpg)

### --tests

![Folder structure](assets/TestFolderStructure.png)
