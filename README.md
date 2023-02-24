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
2. Optionally, filter the Log File by DateTime. If this filter is used, DateTime format must follow "DD:HH:MM:SS". 
3. Specify an Output file to save results to. The output file is detailed in the subsection below. 
4. Click Parse File to run. The interface will disappear, and the output file will be generated. 

<img src="/MPLogFileParser/Resources/UserInterface.png" alt="MPLogFileParser User Interface" width="1000"/>

### Input File Specifications
The input log must be space delimited. 

Column definitions:
1. Host making the request: A hostname when possible, otherwise the Internet address if the name could not be looked up.
2. Date and time: Format "[DD:HH:MM:SS]", where DD is the day of the month from [1-31] and HH:MM:SS is the time of day using the 24-hour clock. 
3. Request: Request URI given in quotes. 
4. Return code: HTTP reply code.
5. Return size: Number of bytes in the reply. 

Examples:
*	141.243.1.172 [29:23:53:25] "GET /Software.html HTTP/1.0" 200 1497
*	query2.lycos.cs.cmu.edu [29:23:53:36] "GET /Consumer.html HTTP/1.0" 200 1325

Notes:
*	Multiple consecutive delimiters are not permitted.
*	The file must not contain any header line(s).
*	Any invalid lines will be skipped.
*	Filtering is not recommended if data spans multiple months.

An example input dataset is provided in 'ExampleData.zip'. 

### Output File Specifications
The output file generates two lists, as follows:
1. Number of accesses to webserver per host, sorted in descending order.
2. Number of successful resource accesses by URI (only 'GET' accesses with a reply code of 200).

The file is space delimited, and the two lists are separated by a blank line.

An example output dataset is provided in 'ExampleData.zip'.