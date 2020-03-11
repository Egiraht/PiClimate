namespace PiClimate.Monitor
{
  export class MeasurementFilter
  {
    public timePeriod: number = 24 * 60 * 60;

    public fromTime: Date = new Date(Date.now() - 24 * 60 * 60 * 1000);

    public toTime: Date = new Date();

    public resolution: number = 1440;
  }
}
