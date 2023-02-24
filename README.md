# MPLogFileParser
This repo contains a web server log parser

## Repo Information
MPLogFileParser was created on February 21, 2023. It contains a take-home assignment for a web server log parser. The application summarizes number of accesses to the webserver by host, and number of successful resource accesses by URI, over a specified time range. 

## Build Information
MPLogFileParser was developed in Visual Studio Community 2022, as a C# WPF Application. It is built as a Windows Application with a target framework of .NET 6.0. Requires Windows 7.0 or later. 

## Running the Program
The compiled executable resides in \MPLogFileParser\MPLogFileParser\bin\Release\net6.0-windows. To run, double click 'MPLogFileParser.exe'. 

## Using the Program
When the program is launched, the user interface shown in the image below will appear. 
1. Select an input web server Log File. The file must follow the format standards detailed in the subsection below. 
2. Optionally, filter the Log File by DateTime. If this filter is used, DateTime format must follow [DD:HH:MM:SS]. 
3. Specify an Output file to save results to. The output file is detailed in the subsection below. 
4. Click Parse File to run. The interface will disappear, and the output file will be generated. 
<img src="/Resources/UserInterface.png" alt="MPLogFileParser User Interface" width="1000"/>

### Input Log File Format

### Output Results File Details
