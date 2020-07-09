# Tod eines Handlungsreisenden

This is a C# implementation of the Held-Karp algorithm for solving the travelling salesman problem as part of the [get int {IT} coding challenge](https://www.get-in-it.de/coding-challenge).

## Installation

### Windows

- Download and install the [.NET Core 3.1 Runtime](https://download.visualstudio.microsoft.com/download/pr/d97cfaf4-b17f-46c7-9a11-7f0d25dfd8b0/f76d4fce8e38b289efb9403aab0a0c9f/dotnet-runtime-3.1.5-win-x64.exe)
- Download the [Windows version of the application]() from the releases and extract the content

### Linux

- Download and install the [.NET Core 3.1 Runtime](https://docs.microsoft.com/de-de/dotnet/core/install/linux)
- Download the [Linux version of the application]() from the releases and extract the content

## Usage

- Open a console or PowerShell and navigate to the folder with the extracted application
- Run the application with the following optional arguments:

```
tod-eines-handlungsreisenden:
  A program to solve the travelling salesman problem

Usage:
  tod-eines-handlungsreisenden [options]

Options:
  --filepath <filepath>    Path to the CSV file containing the sites [default: msg_standorte_deutschland.csv]
  --brute-force            Use a brute force approach instead of the Held-Karp algorithm [default: False]
  --version                Show version information
  -?, -h, --help           Show help and usage information
```

## Credits

This application uses the following libraries:

- [CsvHelper](https://joshclose.github.io/CsvHelper/) for reading and mapping the content of a CSV file
- [BAMCIS GeoCoordinate](https://github.com/bamcis-io/GeoCoordinate) for calculating the distance between two locations
- [System.CommandLine.DragonFruit](https://github.com/dotnet/command-line-api) for parsing the command line arguments