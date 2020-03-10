/// <reference path="MeasurementFilter.ts" />

namespace PiClimate.Monitor
{
  export class ChartParameters
  {
    public chartId: string = "";

    public pressureChartLabel: string = "";

    public temperatureChartLabel: string = "";

    public humidityChartLabel: string = "";

    public pressureLineColor: string = "";

    public temperatureLineColor: string = "";

    public humidityLineColor: string = "";

    public trimSpaces: boolean = false;

    public requestUri: string = "";

    public requestMethod: string = "POST";

    public filter: MeasurementFilter = new MeasurementFilter();
  }
}
