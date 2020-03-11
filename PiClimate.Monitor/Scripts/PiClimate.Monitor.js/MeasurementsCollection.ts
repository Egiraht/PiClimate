/// <reference path="Measurement.ts" />

namespace PiClimate.Monitor
{
  export class MeasurementsCollection
  {
    public minTimestamp: Date = new Date();

    public maxTimestamp: Date = new Date();

    public minPressure: number = 0;

    public maxPressure: number = 0;

    public minPressureTimestamp: Date = new Date();

    public maxPressureTimestamp: Date = new Date();

    public minTemperature: number = 0;

    public maxTemperature: number = 0;

    public minTemperatureTimestamp: Date = new Date();

    public maxTemperatureTimestamp: Date = new Date();

    public minHumidity: number = 0;

    public maxHumidity: number = 0;

    public minHumidityTimestamp: Date = new Date();

    public maxHumidityTimestamp: Date = new Date();

    public count: number = 0;

    public measurements: Measurement[] = [];
  }
}
