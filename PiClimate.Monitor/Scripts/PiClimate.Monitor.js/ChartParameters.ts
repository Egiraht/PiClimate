/// <reference path="MeasurementFilter.ts" />

namespace PiClimate.Monitor
{
  export class ChartParameters
  {
    public chartId: string = "measurements-chart";

    public pressureChartLabel: string = "Pressure";

    public temperatureChartLabel: string = "Temperature";

    public humidityChartLabel: string = "Humidity";

    public pressureUnits: string = "mmHg";

    public temperatureUnits: string = "°C";

    public humidityUnits: string = "%";

    public pressureLineColor: string = "blue";

    public temperatureLineColor: string = "green";

    public humidityLineColor: string = "red";

    public requestUri: string = "";

    public requestMethod: string = "POST";

    public filter: MeasurementFilter = new MeasurementFilter();
  }
}
