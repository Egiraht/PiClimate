/// <reference path="Measurement.ts" />

namespace PiClimate.Monitor
{
  export class MeasurementsResult
  {
    public fromTime : Date | null = null;

    public toTime : Date | null = null;

    public resolution : number | null = null;

    public measurements : Measurement[] | null = null;

    public static async fetchFromJson(url: string, method: string, filter: object): Promise<MeasurementsResult | null>
    {
      try
      {
        let response = await fetch(url, {
          method: method,
          mode: "cors",
          headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
          },
          body: JSON.stringify(filter)
        });

        return await response.json() as MeasurementsResult;
      }
      catch
      {
        return null;
      }
    }
  }
}
