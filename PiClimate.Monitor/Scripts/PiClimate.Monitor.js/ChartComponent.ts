/// <reference path="ChartParameters.ts" />
/// <reference path="MeasurementsCollection.ts" />

namespace PiClimate.Monitor
{
  /**
   * A class representing the measurement data chart component.
   */
  export class ChartComponent
  {
    private _isUpdating: boolean = false;
    private _isEmpty: boolean = true;
    private _isUpdatingFailed: boolean = false;

    public static readonly emptyClassName: string = "empty";
    public static readonly updatingClassName: string = "updating";
    public static readonly updatingFailedClassName: string = "failed";

    public static readonly dateAxisId: string = "date";
    public static readonly pressureAxisId: string = "pressure";
    public static readonly temperatureAxisId: string = "temperature";
    public static readonly humidityAxisId: string = "humidity";

    /**
     * The chart parameters object used for the chart configuring.
     */
    public readonly chartParameters: ChartParameters;

    /**
     * The *Chart.js* chart object assigned to the instance.
     */
      // @ts-ignore
    public chart: Chart;

    /**
     * Gets or sets the flag indicating the emptiness of the measurement data.
     */
    get isEmpty(): boolean
    {
      return this._isEmpty;
    }

    set isEmpty(value: boolean)
    {
      this._isEmpty = value;

      // @ts-ignore
      let chartWrapperElement = $(`#${this.chartParameters.chartId}-wrapper`);
      if (this._isEmpty)
        chartWrapperElement.addClass(ChartComponent.emptyClassName);
      else
        chartWrapperElement.removeClass(ChartComponent.emptyClassName);
    }

    /**
     * Gets or sets the flag indicating the measurement data updating state.
     */
    get isUpdating(): boolean
    {
      return this._isUpdating;
    }

    set isUpdating(value: boolean)
    {
      this._isUpdating = value;

      // @ts-ignore
      let chartWrapperElement = $(`#${this.chartParameters.chartId}-wrapper`);
      if (this._isUpdating)
        chartWrapperElement.addClass(ChartComponent.updatingClassName);
      else
        chartWrapperElement.removeClass(ChartComponent.updatingClassName);
    }

    /**
     * Gets or sets the flag indicating the chart updating failure.
     */
    get isUpdatingFailed(): boolean
    {
      return this._isUpdatingFailed;
    }

    set isUpdatingFailed(value: boolean)
    {
      this._isUpdatingFailed = value;

      // @ts-ignore
      let chartWrapperElement = $(`#${this.chartParameters.chartId}-wrapper`);
      if (this._isUpdatingFailed)
        chartWrapperElement.addClass(ChartComponent.updatingFailedClassName);
      else
        chartWrapperElement.removeClass(ChartComponent.updatingFailedClassName);
    }

    /**
     * Creates a new chart control instance.
     * @param chartParameters The chart parameters object used for the chart configuring.
     */
    public constructor(chartParameters: ChartParameters)
    {
      this.chartParameters = chartParameters;

      // Setting chart default options.
      // @ts-ignore
      let defaults = Chart.defaults.global.elements;
      defaults.point.radius = 0.5;
      defaults.point.hitRadius = 5;
      defaults.point.hoverRadius = 5;
      defaults.line.borderWidth = 2;
      defaults.line.tension = 0;
      defaults.line.fill = false;

      // Initializing the new chart instance.
      // @ts-ignore
      this.chart = new Chart(this.chartParameters.chartId, {
        type: "scatter",
        data: {
          datasets: [
            {
              xAxisID: ChartComponent.dateAxisId,
              yAxisID: ChartComponent.pressureAxisId,
              label: this.chartParameters.pressureChartLabel,
              backgroundColor: this.chartParameters.pressureLineColor,
              borderColor: this.chartParameters.pressureLineColor,
              showLine: true,
              data: []
            },
            {
              xAxisID: ChartComponent.dateAxisId,
              yAxisID: ChartComponent.temperatureAxisId,
              label: this.chartParameters.temperatureChartLabel,
              backgroundColor: this.chartParameters.temperatureLineColor,
              borderColor: this.chartParameters.temperatureLineColor,
              showLine: true,
              data: []
            },
            {
              xAxisID: ChartComponent.dateAxisId,
              yAxisID: ChartComponent.humidityAxisId,
              label: this.chartParameters.humidityChartLabel,
              backgroundColor: this.chartParameters.humidityLineColor,
              borderColor: this.chartParameters.humidityLineColor,
              showLine: true,
              data: []
            }
          ]
        },
        options: {
          scales: {
            xAxes: [
              {
                id: ChartComponent.dateAxisId,
                type: "time",
                display: true,
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
                id: ChartComponent.pressureAxisId,
                type: "linear",
                display: "auto",
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
                id: ChartComponent.temperatureAxisId,
                type: "linear",
                display: "auto",
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
                id: ChartComponent.humidityAxisId,
                type: "linear",
                display: "auto",
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
            intersect: true,
            position: "nearest",
            callbacks: {
              title: (tooltipItems: any[]) => new Date(tooltipItems[0].xLabel).toLocaleString(),
              label: (tooltipItem: any) =>
              {
                let units = [
                  this.chartParameters.pressureUnits,
                  this.chartParameters.temperatureUnits,
                  this.chartParameters.humidityUnits
                ];
                return `${tooltipItem.yLabel} ${units[tooltipItem.datasetIndex]}`
              }
            }
          },
          animation: {
            duration: 500
          }
        }
      });
    }

    /**
     * Asynchronously fetches the filtered measurement data from the JSON-encoded HTTP request to the backend.
     * @returns A collection of measurements acquired from the backend or `null` on failure.
     */
    private async fetchFromJson(): Promise<MeasurementsCollection | null>
    {
      try
      {
        this.isEmpty = false;
        this.isUpdatingFailed = false;
        this.isUpdating = true;

        let response = await fetch(this.chartParameters.requestUri, {
          method: this.chartParameters.requestMethod,
          mode: "cors",
          headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
          },

          // HACK: Timezone suffix replacement ("Z" -> "+00:00") in the JS-generated ISO timestamps is needed for
          // correct timezone parsing in the backend.
          body: JSON.stringify({
            resolution: this.chartParameters.filter.resolution,
            fromTime:
              new Date(this.chartParameters.filter.fromTime)
                .toISOString()
                .replace(/Z$/ig, "+00:00"),
            toTime:
              new Date(this.chartParameters.filter.toTime)
                .toISOString()
                .replace(/Z$/ig, "+00:00")
          })
        });

        if (!response.ok)
        {
          // noinspection ExceptionCaughtLocallyJS
          throw response;
        }

        let data = await response.json();
        this.isEmpty = !data.measurements?.length;
        return Object.assign(new MeasurementsCollection(), data);
      }
      catch
      {
        this.isUpdatingFailed = true;
        return null;
      }
      finally
      {
        this.isUpdating = false;
      }
    }

    /**
     * Asynchronously updates the chart by fetching the filtered measurement data from the backend.
     * @returns `true` if chart updating succeeded, otherwise `false`.
     */
    public async updateChart(): Promise<boolean>
    {
      // Fetching the data.
      let response = await this.fetchFromJson();
      if (!response || !response.measurements)
        return false;

      // Mapping the pressure data to the chart.
      this.chart.data.datasets[0].data = response.measurements.map(measurement =>
      {
        return {
          x: measurement.d,
          y: measurement.p
        }
      });

      // Mapping the temperature data to the chart.
      this.chart.data.datasets[1].data = response.measurements.map(measurement =>
      {
        return {
          x: measurement.d,
          y: measurement.t
        }
      });

      // Mapping the humidity data to the chart.
      this.chart.data.datasets[2].data = response.measurements.map(measurement =>
      {
        return {
          x: measurement.d,
          y: measurement.h
        }
      });

      // Scaling the chart's Y axis.
      this.chart.options.scales.xAxes[0].ticks = {
        min:
          new Date(response.minTimestamp).valueOf() < new Date(this.chartParameters.filter.fromTime).valueOf()
            ? new Date(response.minTimestamp)
            : new Date(this.chartParameters.filter.fromTime),
        max:
          new Date(response.maxTimestamp).valueOf() > new Date(this.chartParameters.filter.toTime).valueOf()
            ? new Date(response.maxTimestamp)
            : new Date(this.chartParameters.filter.toTime)
      };

      // Rendering the chart and updating the summary table.
      this.chart.update();
      this.updateChartSummary(response);

      return true;
    }

    /**
     * Updates the chart's summary table.
     * @param response The collection of measurements used for summary updating.
     */
    private updateChartSummary(response: MeasurementsCollection)
    {
      // @ts-ignore
      let $ = jQuery;
      let periodStartElement = $(`#${this.chartParameters.chartId}-summary .period-start`);
      let periodEndElement = $(`#${this.chartParameters.chartId}-summary .period-end`);
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

      periodStartElement.text(this.chart.options.scales.xAxes[0].ticks.min.toLocaleString());
      periodEndElement.text(this.chart.options.scales.xAxes[0].ticks.max.toLocaleString());
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
