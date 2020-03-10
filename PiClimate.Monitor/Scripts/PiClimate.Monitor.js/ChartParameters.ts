/// <reference path="MeasurementFilter.ts" />

namespace PiClimate.Monitor
{
  export class ChartParameters
  {
    public chartId: string = "";

    public chartLabel: string = "";

    public measurementParameter: string = "";

    public lineColor: string = "";

    public requestUri: string = "";

    public requestMethod: string = "POST";

    public filter: MeasurementFilter = new MeasurementFilter();
  }
}
