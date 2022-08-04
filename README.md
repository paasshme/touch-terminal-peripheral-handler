# Touch terminal peripheral handler


## Presentation

This project is a microservice made to handle any peripheral embeded in a touch terminal.  
This provide an uniformized way to develop interface for this peripherals and ease the development and integration.  

Peripheral embeded can be barcode reader, heater, any kind of printer...

This application is made in ASP.NET Core and can run in docker containers (Windows based and Linux based).

## Repository organisation:

.  
├── **InteractiveTerminalCrossPlatformMicroservice/**: App code, including HTTP server, swagger doc and peripheral handling  
├── **PeripheralTools/**: Fake peripheral, for demo and test purpose  
├── **Scripts/**: Setup and run for Windows/linux  
├── **TestDevices/**: Example peripheral code for barcode reader  
└── **TestInteractiveTerminalCrossPlatformMicroservice**: Unit tests  


 
## Usage

Scripts folders contain launch scripts and install scripts to have the service ready and started at the boot of the touch terminal.  

### Linux
```
cd Scripts/Linux/
chmod +x launchd.sh
./launchd.sh
```

### Windows

```
cd Scripts\Windows
.\winLaunch.cmd
```



## Implemntation details

### Peripheral implementation

All peripheral are created by implementing a common interface **IDevice**. This interface defines two methods, start and stop and peripheral can have other methods (such as read for a barcode...).

### Peripheral load

The most important part of the application is the peripheral creation. It is done through a config file and via an AbstractFactory.
The factory load peripheral through their binary files (dll). Since all of them implement IDevice, they are created as such. The Factory also handle the others methods via System reflection.
In this manner, when the client request a method to be invoked on a peripheral, the application will try to invoke this method if it exists.

### Controller
The entrypoint of the project is located under InteractiveTerminalCrossPlatformMicroservice/Controllers where there is a ASP.NET controller to handle HTTP Requests.  
The main idea is to use request in the following way:  **api/{ObjectName}/{Method}**, for instance api/barcode/read.  
