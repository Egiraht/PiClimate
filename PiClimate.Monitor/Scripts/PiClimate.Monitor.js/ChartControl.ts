/// <reference path="ChartParameters.ts" />
/// <reference path="MeasurementsResult.ts" />

namespace PiClimate.Monitor
{
  export class ChartControl
  {
    public readonly chartParameters: ChartParameters;

    // @ts-ignore
    private chart: Chart | null = null;

    public constructor(chartParameters: ChartParameters)
    {
      this.chartParameters = chartParameters;
    }

    private async fetchFromJson(): Promise<MeasurementsResult | null>
    {
      try
      {
        let response = await fetch(this.chartParameters.requestUri, {
          method: this.chartParameters.requestMethod,
          mode: "cors",
          headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
          },
          body: JSON.stringify(this.chartParameters.filter)
        });

        return await response.json() as MeasurementsResult;
      }
      catch
      {
        return null;
      }
    }

    public async updateChart(): Promise<boolean>
    {
      let response = await this.fetchFromJson();
      if (!response || !response.measurements)
        return false;

      // @ts-ignore
      this.chart = new Chart(this.chartParameters.chartId, {
        type: "scatter",
        data: {
          datasets: [
            {
              label: this.chartParameters.chartLabel,
              data: response.measurements.map(measurement =>
              {
                return {
                  x: measurement.d,
                  // @ts-ignore
                  y: measurement[this.chartParameters.measurementParameter]
                }
              }),
              radius: 0,
              showLine: true,
              backgroundColor: this.chartParameters.lineColor,
              borderColor: this.chartParameters.lineColor,
              borderWidth: 2,
              lineTension: 0,
              fill: false
            }
          ],
        },
        options: {
          scales: {
            xAxes: [
              {
                type: "time",
                time: {
                  isoWeekday: true,
                  minUnit: "second",
                  displayFormats: {
                    second: "HH:mm:ss",
                    minute: "HH:mm",
                    hour: "HH:00"
                  }
                }
              }
            ]
          },
          tooltips: {
            mode: "index",
            intersect: false
          }
        }
      });

      return true;
    }
  }
}
