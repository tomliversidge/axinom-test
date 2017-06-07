# axinom-test

# Running on Windows
Build by running the build.ps1 script.

run the ControlPanel app through the cmd line:

`set ASPNETCORE_ENVIRONMENT=Development`

in the Axinom.ControlPanel directory run

`dotnet run`
## Control Panel App
In cmd, navigate to the Axinom.ControlPanel directory and run 
`set ASPNETCORE_ENVIRONMENT=Development`
followed by 
`dotnet run`

This should start the server on localhost, port 5001. 

## Data Management App
In cmd, navigate to the Axinom.DataManagement directory and run 

`set ASPNETCORE_ENVIRONMENT=Development`
followed by 
`dotnet run`

This should start the server on localhost, port 5002. 

# Running on Mac

Build by running the build.sh script.

## Control Panel App
In terminal, navigate to the Axinom.ControlPanel directory and run 

`ASPNETCORE_ENVIRONMENT=Development dotnet run`

This should start the server on localhost, port 5001. 

## Data Management App
In terminal, navigate to the Axinom.DataManagement directory and run 

`ASPNETCORE_ENVIRONMENT=Development dotnet run`

This should start the server on localhost, port 5002. 
