# PiClimate

PiClimate is a software for logging and displaying climatic data.

It consists of two parts:

* **PiClimate.Logger** service is responsible for data measuring and logging.
* **PiClimate.Monitor** represents a web server used for measured data visualization.

Using **PiClimate.Logger**, climatic data are collected using various data providers such as hardware sensors, and then
are logged into data storages like SQL databases. Using **PiClimate.Monitor**, the logged data can be visualized as
parametric charts for selected time periods.

For more detailed information see the *Readme* files in the corresponding project directories.
