// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

/// <reference path="ChartParameters.ts" />
/// <reference path="MeasurementsCollection.ts" />

namespace PiClimate.Monitor
{
  /**
   * Importing jQuery library.
   */
  // @ts-ignore
  import $ = window.jQuery;
  if (!$)
    console.error("jQuery library is missing!");

  /**
   * Importing Chart.js library.
   */
  // @ts-ignore
  import Chart = window.Chart;
  if (!Chart)
    console.error("Chart.js library is missing!");

  /**
   * Importing Moment.js library.
   */
  // @ts-ignore
  import moment = window.moment;
  if (!moment)
    console.error("Moment.js library is missing!");

  /**
   * A class representing the measurement data chart component.
   */
  export class ChartComponent
  {
    private _isUpdating: boolean = false;
    private _isEmpty: boolean = true;
    private _isUpdatingFailed: boolean = false;

    public static readonly dateTimeFormat: string = "L LTS";
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

      // Setting the date-time locale.
      let locale = navigator.languages ? navigator.languages[0] : navigator.language;
      moment.locale(locale);

      // Setting chart default options.
      let defaults = Chart.defaults.global.elements;
      defaults.point.radius = 0.5;
      defaults.point.hitRadius = 5;
      defaults.point.hoverRadius = 5;
      defaults.line.borderWidth = 2;
      defaults.line.tension = 0;
      defaults.line.fill = false;

      // Initializing the new chart instance.
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
                    second: "LTS",
                    minute: "LT",
                    hour: "LT",
                    day: "L",
                    month: "L"
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
              title: (tooltipItems: any[]) => moment(tooltipItems[0].xLabel).format(ChartComponent.dateTimeFormat),
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
            duration: 0
          },
          hover: {
            animationDuration: 100
          },
          responsiveAnimationDuration: 0
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
          body: JSON.stringify(this.chartParameters.filter)
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
          new Date(response.minTimestamp).valueOf() < this.chartParameters.filter.getFromTimeDate().valueOf()
            ? new Date(response.minTimestamp)
            : this.chartParameters.filter.getFromTimeDate(),
        max:
          new Date(response.maxTimestamp).valueOf() > this.chartParameters.filter.getToTimeDate().valueOf()
            ? new Date(response.maxTimestamp)
            : this.chartParameters.filter.getToTimeDate()
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
      let entriesCountElement = $(`#${this.chartParameters.chartId}-summary .entries-count`);

      periodStartElement.text(moment(this.chart.options.scales.xAxes[0].ticks.min).format(ChartComponent.dateTimeFormat));
      periodEndElement.text(moment(this.chart.options.scales.xAxes[0].ticks.max).format(ChartComponent.dateTimeFormat));
      minPressureElement.text(response.minPressure);
      maxPressureElement.text(response.maxPressure);
      minPressureTimestampElement.text(moment(response.minPressureTimestamp).format(ChartComponent.dateTimeFormat));
      maxPressureTimestampElement.text(moment(response.maxPressureTimestamp).format(ChartComponent.dateTimeFormat));
      minTemperatureElement.text(response.minTemperature);
      maxTemperatureElement.text(response.maxTemperature);
      minTemperatureTimestampElement.text(moment(response.minTemperatureTimestamp).format(ChartComponent.dateTimeFormat));
      maxTemperatureTimestampElement.text(moment(response.maxTemperatureTimestamp).format(ChartComponent.dateTimeFormat));
      minHumidityElement.text(response.minHumidity);
      maxHumidityElement.text(response.maxHumidity);
      minHumidityTimestampElement.text(moment(response.minHumidityTimestamp).format(ChartComponent.dateTimeFormat));
      maxHumidityTimestampElement.text(moment(response.maxHumidityTimestamp).format(ChartComponent.dateTimeFormat));
      entriesCountElement.text(response.measurements.length);
    }
  }
}
