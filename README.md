# axinom-test

# Running on Windows
Build by running the build.ps1 script.

The system will save json files to the hard drive. It defaults to `C:\Files` and the system should create the directory automaticallly. If you want to override this, you need to set the `FileSystem>Root` setting in appsettings.Development.json in the Axinom.DataManagement project.

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

The system will save json files to the hard drive. It defaults to `C:\Files` and the system should create the directory automaticallly. If you want to override this, you need to set the `FileSystem>Root` setting in appsettings.Development.json in the Axinom.DataManagement project.

## Control Panel App
In terminal, navigate to the Axinom.ControlPanel directory and run 

`ASPNETCORE_ENVIRONMENT=Development dotnet run`

This should start the server on localhost, port 5001. 

## Data Management App
In terminal, navigate to the Axinom.DataManagement directory and run 

`ASPNETCORE_ENVIRONMENT=Development dotnet run`

This should start the server on localhost, port 5002. 
