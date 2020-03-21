# PiClimate.Logger

**PiClimate.Logger** represents a service program responsible for climatic data measuring and logging.

## Features

The **PiClimate.Logger** uses an abstraction of measurement providers for data measuring.
At the moment the following climatic data providers are supported:

* **Bme280Provider** - acquires the climatic data from the *BME280* hardware sensor connected to the device
via the *I2C* bus.
* **RandomDataProvider** - generates the random climatic data for testing purposes.

The original idea for the project was to run a logging service on a *Raspberry Pi 3* device with a connected
*BME280* sensor. So the actual program testing was taking place using this hardware but potentially the
service can successfully run on other similar hardware too.

For logging the data measured by the provider the program uses a number of loggers:

* **ConsoleLogger** - logs the data to the program's standard output stream.
* **MySqlLogger** - logs the data to the MySQL database.

Multiple loggers can be selected for logging simultaneously.

Also the program features the data limiters useful for limiting the number of stored data entries.
Currently these limiters are provided:

* **MySqlCountLimiter** - limits the total number of data entries stored in the MySQL database.
* **MySqlPeriodLimiter** - limits the number of data entries stored in the MySQL database to those fitting
the selected time period.

Multiple limiters can be applied simultaneously.

## Configuration

The program can be configured using the *Configuration.json* file available in the program's root directory.
See the description comments in the file for more information about the settings it provides.
Also the individual configuration settings can be overriden using the command line arguments.

## References

The program is based on the open-source *.NET Core* framework. Also for functioning it uses these
open-source libraries:

* **Microsoft.Extensions.Configuration.*** - part of the *ASP.NET Core* framework used for collecting the configuration settings.
* **System.Device.Gpio** - provides a general access to the hardware IO.
* **Iot.Device.Bindings** - provides an abstraction layer for accessing the *BME280* sensor. 
* **Dapper** - the SQL query object mapper.
* **MySqlConnector** - the MySQL database driver.

## Links

* **PiClimate** <https://github.com/Egiraht/PiClimate>
* **.NET Core** <https://dotnet.microsoft.com/>
* **ASP.NET Core** <https://dotnet.microsoft.com/apps/aspnet>
* **Iot.Device.Bindings** <https://github.com/dotnet/iot>
* **Dapper** <https://stackexchange.github.io/Dapper/>
* **MySqlConnector** <https://mysqlconnector.net/>
