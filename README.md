## PokemonInfoAPI
The code to access the Pokemon information using the pokemon Api's. This is written in the .Net Core 3.1 using C#.

## Prerequisites
Below are the pre-requisites to run the code locally:
* .Net core 5 version;
* **[optional]** git (to clone the project or you can simply download from the github website).

## Clone Instructions
Clone the code into your local using the below command:
Clone the project:
```sh
$ git clone https://github.com/srinumutyala/PokemonInfoAPI.git
```

There are two ways to run this project:
1. You could open the solution directly in Visual Studio and run the PokemonInfoAPI to run the API. You can run the Tests from Test-->Run All Test Menu.


2. You can build the API from the command line
Build and run (from the project directory):
```sh
$ dotnet build
$ dotnet run --project .\PokemonAPI\PokemonInfoAPI.csproj
```

To run tests:
```sh
$ dotnet test
```

### Enhancements for productionization
Below are few things that can be enhanced for production grade code
* More appropriate exception with tracing ability to trouble shoot issues in the production
* Retry & Circuit breaker for better resiliency - can introduce the retryable staus code and use Polly policies
* E2E tests
