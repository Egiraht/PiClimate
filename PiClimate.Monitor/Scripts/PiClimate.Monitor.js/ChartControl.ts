/// <reference path="ChartParameters.ts" />
/// <reference path="MeasurementsCollection.ts" />

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

    private async fetchFromJson(): Promise<MeasurementsCollection | null>
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

        return await response.json() as MeasurementsCollection;
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
      let defaults = Chart.defaults.global.elements;
      defaults.point.radius = 0.5;
      defaults.point.hitRadius = 0;
      defaults.point.hoverRadius = 0;
      defaults.line.borderWidth = 2;
      defaults.line.tension = 0;
      defaults.line.fill = false;

      // @ts-ignore
      this.chart = new Chart(this.chartParameters.chartId, {
        type: "scatter",
        data: {
          datasets: [
            {
              label: this.chartParameters.pressureChartLabel,
              yAxisID: this.chartParameters.pressureChartLabel,
              backgroundColor: this.chartParameters.pressureLineColor,
              borderColor: this.chartParameters.pressureLineColor,
              showLine: true,
              data: response.measurements.map(measurement =>
              {
                return {
                  x: measurement.d,
                  y: measurement.p
                }
              })
            },
            {
              label: this.chartParameters.temperatureChartLabel,
              yAxisID: this.chartParameters.temperatureChartLabel,
              backgroundColor: this.chartParameters.temperatureLineColor,
              borderColor: this.chartParameters.temperatureLineColor,
              showLine: true,
              data: response.measurements.map(measurement =>
              {
                return {
                  x: measurement.d,
                  y: measurement.t
                }
              })
            },
            {
              label: this.chartParameters.humidityChartLabel,
              yAxisID: this.chartParameters.humidityChartLabel,
              backgroundColor: this.chartParameters.humidityLineColor,
              borderColor: this.chartParameters.humidityLineColor,
              showLine: true,
              data: response.measurements.map(measurement =>
              {
                return {
                  x: measurement.d,
                  y: measurement.h
                }
              })
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
                },
                ticks: {
                  min:
                    response.minTime.valueOf() < this.chartParameters.filter.fromTime.valueOf()
                      ? response.minTime
                      : this.chartParameters.filter.fromTime,
                  max:
                    response.maxTime.valueOf() > this.chartParameters.filter.toTime.valueOf()
                      ? response.maxTime
                      : this.chartParameters.filter.toTime
                }
              }
            ],
            yAxes: [
              {
                id: this.chartParameters.pressureChartLabel,
                type: "linear",
                position: "left",
                scaleLabel: {
                  display: true,
                  labelString: `${this.chartParameters.pressureChartLabel}, ${this.chartParameters.pressureUnits}`,
                  fontColor: this.chartParameters.pressureLineColor
                },
                gridLines: {
                  color: this.chartParameters.pressureLineColor,
                  lineWidth: 0.5
                },
                ticks: {
                  fontColor: this.chartParameters.pressureLineColor
                }
              },
              {
                id: this.chartParameters.temperatureChartLabel,
                type: "linear",
                position: "right",
                scaleLabel: {
                  display: true,
                  labelString: `${this.chartParameters.temperatureChartLabel}, ${this.chartParameters.temperatureUnits}`,
                  fontColor: this.chartParameters.temperatureLineColor
                },
                gridLines: {
                  color: this.chartParameters.temperatureLineColor,
                  lineWidth: 0.5
                },
                ticks: {
                  fontColor: this.chartParameters.temperatureLineColor
                }
              },
              {
                id: this.chartParameters.humidityChartLabel,
                type: "linear",
                position: "right",
                scaleLabel: {
                  display: true,
                  labelString: `${this.chartParameters.humidityChartLabel}, ${this.chartParameters.humidityUnits}`,
                  fontColor: this.chartParameters.humidityLineColor
                },
                gridLines: {
                  color: this.chartParameters.humidityLineColor,
                  lineWidth: 0.5
                },
                ticks: {
                  fontColor: this.chartParameters.humidityLineColor
                }
              }
            ]
          },
          tooltips: {
            mode: "index",
            intersect: false,
            position: "nearest"
          },
          animation: {
            duration: 500
          }
        }
      });

      return true;
    }
  }
}
