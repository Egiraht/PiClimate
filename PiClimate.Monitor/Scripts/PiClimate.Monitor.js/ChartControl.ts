/// <reference path="ChartParameters.ts" />
/// <reference path="MeasurementsCollection.ts" />

namespace PiClimate.Monitor
{
  export class ChartControl
  {
    public readonly chartParameters: ChartParameters;

    // @ts-ignore
    private chart: Chart;

    public constructor(chartParameters: ChartParameters)
    {
      this.chartParameters = chartParameters;

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
              data: []
            },
            {
              label: this.chartParameters.temperatureChartLabel,
              yAxisID: this.chartParameters.temperatureChartLabel,
              backgroundColor: this.chartParameters.temperatureLineColor,
              borderColor: this.chartParameters.temperatureLineColor,
              showLine: true,
              data: []
            },
            {
              label: this.chartParameters.humidityChartLabel,
              yAxisID: this.chartParameters.humidityChartLabel,
              backgroundColor: this.chartParameters.humidityLineColor,
              borderColor: this.chartParameters.humidityLineColor,
              showLine: true,
              data: []
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
                ticks: {}
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

      this.chart.data.datasets[0].data = response.measurements.map(measurement =>
      {
        return {
          x: measurement.d,
          y: measurement.p
        }
      });

      this.chart.data.datasets[1].data = response.measurements.map(measurement =>
      {
        return {
          x: measurement.d,
          y: measurement.t
        }
      });

      this.chart.data.datasets[2].data = response.measurements.map(measurement =>
      {
        return {
          x: measurement.d,
          y: measurement.h
        }
      });

      this.chart.options.scales.xAxes[0].ticks = {
        min:
          response.minTimestamp.valueOf() < this.chartParameters.filter.fromTime.valueOf()
            ? response.minTimestamp
            : this.chartParameters.filter.fromTime,
        max:
          response.maxTimestamp.valueOf() > this.chartParameters.filter.toTime.valueOf()
            ? response.maxTimestamp
            : this.chartParameters.filter.toTime
      };

      this.chart.update();
      this.updateChartSummary(response);

      return true;
    }

    private updateChartSummary(response: MeasurementsCollection)
    {
      // @ts-ignore
      let $ = jQuery;
      let firstTimestampElement = $(`#${this.chartParameters.chartId}-summary .first-timestamp`);
      let lastTimestampElement = $(`#${this.chartParameters.chartId}-summary .last-timestamp`);
      let minPressureElement = $(`#${this.chartParameters.chartId}-summary .min-pressure`);
      let maxPressureElement = $(`#${this.chartParameters.chartId}-summary .max-pressure`);
      let minPressureTimestampElement = $(`#${this.chartParameters.chartId}-summary .min-pressure-timestamp`);
      let maxPressureTimestampElement = $(`#${this.chartParameters.chartId}-summary .max-pressure-timestamp`);
      let minTemperatureElement = $(`#${this.chartParameters.chartId}-summary .min-temperature`);
      let maxTemperatureElement = $(`#${this.chartParameters.chartId}-summary .max-temperature`);
      let minTemperatureTimestampElement = $(`#${this.chartParameters.chartId}-summary .min-temperature-timestamp`);
      let maxTemperatureTimestampElement = $(`#${this.chartParameters.chartId}-summary .max-temperature-timestamp`);
      let minHumidityElement = $(`#${this.chartParameters.chartId}-summary .min-humidity`);
      let maxHumidityElement = $(`#${this.chartParameters.chartId}-summary .max-humidity`);
      let minHumidityTimestampElement = $(`#${this.chartParameters.chartId}-summary .min-humidity-timestamp`);
      let maxHumidityTimestampElement = $(`#${this.chartParameters.chartId}-summary .max-humidity-timestamp`);

      firstTimestampElement.text(new Date(response.minTimestamp).toLocaleString());
      lastTimestampElement.text(new Date(response.maxTimestamp).toLocaleString());
      minPressureElement.text(response.minPressure);
      maxPressureElement.text(response.maxPressure);
      minPressureTimestampElement.text(new Date(response.minPressureTimestamp).toLocaleString());
      maxPressureTimestampElement.text(new Date(response.maxPressureTimestamp).toLocaleString());
      minTemperatureElement.text(response.minTemperature);
      maxTemperatureElement.text(response.maxTemperature);
      minTemperatureTimestampElement.text(new Date(response.minTemperatureTimestamp).toLocaleString());
      maxTemperatureTimestampElement.text(new Date(response.maxTemperatureTimestamp).toLocaleString());
      minHumidityElement.text(response.minHumidity);
      maxHumidityElement.text(response.maxHumidity);
      minHumidityTimestampElement.text(new Date(response.minHumidityTimestamp).toLocaleString());
      maxHumidityTimestampElement.text(new Date(response.maxHumidityTimestamp).toLocaleString());
    }
  }
}
