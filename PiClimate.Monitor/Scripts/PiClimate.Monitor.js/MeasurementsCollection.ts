/// <reference path="Measurement.ts" />

namespace PiClimate.Monitor
{
  export class MeasurementsCollection
  {
    public minTime: Date = new Date();

    public maxTime: Date = new Date();

    public minPressure: number = 0;

    public maxPressure: number = 0;

    public minPressureTime: Date = new Date();

    public maxPressureTime: Date = new Date();

    public minTemperature: number = 0;

    public maxTemperature: number = 0;

    public minTemperatureTime: Date = new Date();

    public maxTemperatureTime: Date = new Date();

    public minHumidity: number = 0;

    public maxHumidity: number = 0;

    public minHumidityTime: Date = new Date();

    public maxHumidityTime: Date = new Date();

    public count: number = 0;

    public measurements: Measurement[] = [];
  }
}
