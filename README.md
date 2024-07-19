# Hive SDK
The Hive SDK provides Node and Server Side development platform for Nerd United. 

# Purpose
The Purpose of Hive is to provide a composable architecture that Brands can use to build utility for their customers.  Hive coupled with Nerd Dev X Portal allows Brands to publish code to run on the Node Network and also to the Hive server through the use of Plugins. 

# Plugins
Plugins are both client and server side biniaries that can be pushed in real-time to the node network. Plugins are versioned so older versions will be overwritten and replaced with newer versions as the Brand continues to add functionality and features. 

## Node SDK
The Node SDK provides tools that the Brands can use to build their node utility. 

## HiveServer SDK
The HiveSever SDK provides tools that tne Brands can use to publish server side code. 

# Running
You should be able to run and debug this application localy. It was developed using Visual Studio Community 2022 (https://visualstudio.microsoft.com/vs/community/)

## Building
>TODO// Build this into the CI/CD pipeline when we are ready to release. 


## Node.Shell
Node.Shell is the basic Node. It is a holder for plugins and has some default functionality. Use the documentation below to learn how to build your node to run on any platform. 

All of these types of builds should include `self-contained`. This provides the option to run the node on any platforms natively, without any outside dependencies. 

### Windows
1. Open a terminal or command prompt.
2. Navigate to your project's root directory (where the .csproj file is located).
3. Run the following command:

`dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true`

4. The published output will be in:
`bin/Release/net8.0/win-x64/publish`

Copy all of the contents of this folder to the Windows computer. 

5. Using Terminal, cmd, or PowerShell navigate to the Node.Shell executable and run:

`./Node.Shell`

>This will NOT run as service, once the Terminal window is closed the Node will stop running. To run as a service on Windows use SC or NSSM.

### Mac
1. Open a terminal or command prompt.
2. Navigate to your project's root directory (where the .csproj file is located).
3. Run the following command:

`dotnet publish -c Release -r osx-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true`

4. The published output will be in:
`bin/Release/net8.0/osx-x64/publish`

Copy all of the contents of this folder to the Mac. 

5. On a Mac, give the executable permission to run:
`chmod +x ./Node.Shell` 

6. Run the application:
`./Node.Shell`

 >This will NOT run as service, once the Terminal window is closed the Node will stop running. To run as a service on Mac more setup is required.

## Linux
1. Open a terminal or command prompt.
2. Navigate to your project's root directory (where the .csproj file is located).
3. Run the following command:

`dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true`

4. The published output will be in:
`bin/Release/net8.0/linux-x64/publish`

Copy all of the contents of this folder to Linux. 

5. On a LInux, give the executable permission to run:
`chmod +x ./Node.Shell` 

6. Run the application:
`./Node.Shell`

>This will NOT run as service, once the Terminal window is closed the Node will stop running. To run as a service on Mac more setup is required.

## Hive Server
Hive Server can also be published to run on Mac, Linux or Windows. Follow the instructions below to publish Hive Server. 

