Cryptography
===========

Short description
-----------------
This project contains cryptographic utilities and test cases for .NET 10. It includes implementation code and unit tests to validate cryptographic functionality.

Prerequisites
-------------
- .NET 10 SDK (install from https://dotnet.microsoft.com)
- Visual Studio 2026 or a compatible IDE

Repository layout
-----------------
- /Cryptography  - main project (implementation)
- /Cryptographytest - unit tests (xUnit / NUnit / MSTest depending on project)

Build and run
-------------
From the repository root, using the dotnet CLI:

1. Restore dependencies and build:
   dotnet restore
   dotnet build

2. Run the test project:
   dotnet test ./Cryptographytest

Using Visual Studio
-------------------
- Open the solution in Visual Studio 2026 and build or run tests using Test Explorer.

Contributing
------------
- Open an issue or submit a pull request with a clear description of the change.
- Keep changes focused and include unit tests for behavior changes.

License
-------
Specify a license file (LICENSE) in the repo or add license information here.

Contact
-------
For questions, open an issue in this repository.
