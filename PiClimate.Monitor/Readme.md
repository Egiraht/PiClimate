# PiClimate.Monitor

**PiClimate.Monitor** is a web server used for climatic data visualization.

## Features

Climatic data are acquired by the server using the measurement data source concept.
At the moment these data sources are provided:

* **MySqlSource** - acquires the climatic data from the specified MySQL database,
* **RandomDataSource** - generates random climatic data for testing purposes (selected by default).

By accessing the web server page, collected data can further be visualized using charts for selected time periods.

## Settings

The program can be configured using the *Settings.json* file added the program's root directory. Also individual
settings can be overriden using the command line arguments with syntax of `--parameter=value` or
`--section:parameter=value`.

## Requirements

The program requires the *.NET Runtime 5.0* and *ASP.NET Core Runtime 5.0* packages to be installed in the system.

## References

The program is based on the open-source *.NET* framework. Also for functioning it uses these third-party open-source
libraries:

* **Dapper** - the SQL query object mapper.
* **MySqlConnector** - the MySQL database driver.
* **jQuery** - the JavaScript library used for web page DOM manipulation.
* **Chart.js** - the JavaScript chart-based data visualization library.
* **Moment.js** - the JavaScript date-time manipulation library.
* **Bootstrap** - the CSS framework for the frontend.

## Links

* **PiClimate** <https://github.com/Egiraht/PiClimate>
* **.NET** <https://dotnet.microsoft.com/>
* **Dapper** <https://stackexchange.github.io/Dapper/>
* **MySqlConnector** <https://mysqlconnector.net/>
* **jQuery** <https://jquery.com/>
* **Chart.js** <https://www.chartjs.org/>
* **Moment.js** <https://momentjs.com/>
* **Bootstrap** <https://getbootstrap.com/>
