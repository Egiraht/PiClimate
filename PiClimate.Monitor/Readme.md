# PiClimate.Monitor

**PiClimate.Monitor** is a web server used for visualization of the climatic data stored by the
*PiClimate.Logger* service.

## Features

The server uses **Chart.js**-generated charts to visualize the climatic data.
Using the HTTP query requests the stored climatic data can be filtered for the selected time period.
For data acquisition the server uses the data sources. At the moment these data sources are provided:

* **MySqlSource** - acquires the climatic data from the specified MySQL database.

## Configuration

The program can be configured using the *Configuration.json* file available in the program's root directory.
See the description comments in the file for more information about the settings it provides.
Also the individual configuration settings can be overriden using the command line arguments.

## References

The program is based on the open-source *ASP.NET Core* framework. Also for functioning it uses these
open-source libraries:

* **Dapper** - the SQL query object mapper.
* **MySqlConnector** - the MySQL database driver.
* **jQuery** - the JavaScript library used for web page DOM manipulation.
* **Chart.js** - the JavaScript chart-based data visualization library.
* **Moment.js** - the JavaScript date-time manipulation library, used as a part of *Chart.js*.
* **Bootstrap** - the CSS framework for the frontend.

## Links

* **PiClimate** <https://github.com/Egiraht/PiClimate>
* **ASP.NET Core** <https://dotnet.microsoft.com/apps/aspnet>
* **Dapper** <https://stackexchange.github.io/Dapper/>
* **MySqlConnector** <https://mysqlconnector.net/>
* **jQuery** <https://jquery.com/>
* **Chart.js** <https://www.chartjs.org/>
* **Moment.js** <https://momentjs.com/>
* **Bootstrap** <https://getbootstrap.com/>
