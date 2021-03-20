# PiClimate.Logger

**PiClimate.Logger** represents a service program responsible for climatic data acquisition and logging.

## Features

The **PiClimate.Logger** uses an abstraction of measurement providers for data measuring. At the moment the following
climatic data providers are supported:

* **BmeReaderProvider** - reads the climatic data from the *BME280* sensor using the *BMEReader* *USB* to *I2C* bus
  adapter,
* **Bme280Provider** - reads the climatic data from the *BME280* sensor connected directly via the *I2C* bus (requires a
  general purpose *I2C* bus to be available in the device),
* **RandomDataProvider** - generates the random climatic data for testing purposes.

The original idea for the project was to run a logging service on a *Raspberry Pi 3* device with a connected
*BME280* sensor. So the actual program testing was taking place using this hardware but potentially the service can
successfully run on other similar hardware too.

For logging the data measured by the provider the program uses a number of loggers:

* **ConsoleLogger** - logs the data to the program's standard output stream,
* **MySqlLogger** - logs the data to the MySQL database.

Multiple loggers can be selected for logging simultaneously.

Also the program features the data limiters useful for limiting the number of stored data entries. Currently these
limiters are provided:

* **MySqlCountLimiter** - limits the total number of data entries stored in the MySQL database,
* **MySqlPeriodLimiter** - limits the number of data entries stored in the MySQL database to those fitting the selected
  time period.

Multiple limiters can be applied simultaneously.

## Configuration

The program can be configured using the *Configuration.json* file available in the program's root directory. See the
description comments in the file for more information about the settings it provides. Also the individual configuration
settings can be overriden using the command line arguments with syntax of `--parameter=value` or
`--section:parameter=value`.

## Requirements

The program requires the *.NET Runtime 5.0* to be installed in the system.

## References

The program is based on the open-source *.NET* framework. Also for functioning it uses these third-party open-source
libraries:

* **Iot.Device.Bindings** - provides an abstraction layer for accessing the *BME280* sensor using the *I2C* bus,
* **Dapper** - the SQL query object mapper,
* **MySqlConnector** - the MySQL database driver.

## Links

* **PiClimate** <https://github.com/Egiraht/PiClimate>
* **.NET** <https://dotnet.microsoft.com/>
* **Iot.Device.Bindings** <https://github.com/dotnet/iot>
* **Dapper** <https://stackexchange.github.io/Dapper/>
* **MySqlConnector** <https://mysqlconnector.net/>
