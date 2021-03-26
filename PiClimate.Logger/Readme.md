# PiClimate.Logger

**PiClimate.Logger** represents a service program responsible for climatic data acquisition and logging.

## Features

The **PiClimate.Logger** uses an abstraction of measurement providers for data measuring. At the moment the following
climatic data providers are supported:

* **BmeReaderProvider** - reads climatic data from the *BME280* sensor using the connected *BMEReader* adapter,
* **Bme280Provider** - reads climatic data from the *BME280* sensor connected directly via the exposed *I2C* bus
  (accessible in devices like Raspberry Pi),
* **RandomDataProvider** - generates random climatic data for testing purposes (selected by default).

For logging the data measured by the provider the program uses a number of loggers:

* **ConsoleLogger** - logs the data to the program's standard output stream,
* **MySqlLogger** - logs the data to a MySQL database.

Multiple loggers can be selected for logging simultaneously.

Also the program features the data limiters concept useful for limiting the number of stored data entries. Currently
these limiters are provided:

* **MySqlCountLimiter** - limits the total number of data entries stored in a MySQL database,
* **MySqlPeriodLimiter** - limits the number of data entries stored in a MySQL database to the specified time period.

Multiple limiters can be applied simultaneously.

## Settings

The program can be configured using the *Settings.json* file added the program's root directory. Also individual
settings can be overriden using the command line arguments with syntax of `--parameter=value` or
`--section:parameter=value`.

## Requirements

The program requires the *.NET Runtime 5.0* package to be installed in the system.

## References

The program is based on the open-source *.NET* framework. Also for functioning it uses these third-party open-source
libraries:

* **Iot.Device.Bindings** - provides an abstraction layer for accessing the *BME280* sensor using *I2C* bus,
* **Dapper** - the SQL query object mapper,
* **MySqlConnector** - the MySQL database driver.

## Links

* **PiClimate** <https://github.com/Egiraht/PiClimate>
* **.NET** <https://dotnet.microsoft.com/>
* **Iot.Device.Bindings** <https://github.com/dotnet/iot>
* **Dapper** <https://stackexchange.github.io/Dapper/>
* **MySqlConnector** <https://mysqlconnector.net/>
