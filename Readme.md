### Instructions

You can run the project using docker without installing anything on the machine. 
(See docker/docker-compose.yml for more information)

```
docker-compose -f docker/docker-compose.yml up
```

Alternatively you need Dotnet Core SDK 3.1 
https://dotnet.microsoft.com/download/dotnet-core/3.1

To run tests:

```
dotnet test
```

To compile:
```
dotnet restore
dotnet build
```

To run:
```
dotnet run -p src/MyCustomers.Console/MyCustomers.Console.csproj -- -i data/customers.txt -o data/output.txt
```


