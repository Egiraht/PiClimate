// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

/// <reference path="MeasurementFilter.ts" />

namespace PiClimate.Monitor
{
  /**
   * The measurement data chart parameters class.
   */
  export class ChartParameters
  {
    /**
     * Gets the ID string identifying the chart in the HTML markup.
     */
    public readonly chartId: string;

    /**
     * Gets or sets the chart's label string for pressure.
     */
    public pressureChartLabel: string = "Pressure";

    /**
     * Gets or sets the chart's label string for temperature.
     */
    public temperatureChartLabel: string = "Temperature";

    /**
     * Gets or sets the chart's label string for humidity.
     */
    public humidityChartLabel: string = "Humidity";

    /**
     * Gets or sets the string of units the pressure is expressed in.
     */
    public pressureUnits: string = "mmHg";

    /**
     * Gets or sets the string of units the temperature is expressed in.
     */
    public temperatureUnits: string = "°C";

    /**
     * Gets or sets the string of units the humidity is expressed in.
     */
    public humidityUnits: string = "%";

    /**
     * Gets or sets the chart's line color value for pressure.
     */
    public pressureLineColor: string = "blue";

    /**
     * Gets or sets the chart's line color value for temperature.
     */
    public temperatureLineColor: string = "green";

    /**
     * Gets or sets the chart's line color value for humidity.
     */
    public humidityLineColor: string = "red";

    /**
     * Gets or sets the URI string where the HTTP request should be made.
     */
    public requestUri: string = "";

    /**
     * Gets or sets the HTTP method for the request.
     */
    public requestMethod: string = "POST";

    /**
     * Gets or sets the measurement filter assigned for the chart.
     */
    public filter: MeasurementFilter = new MeasurementFilter();

    /**
     * Creates a new instance of chart parameters.
     * @param chartId The ID string identifying the chart in the HTML markup.
     */
    public constructor(chartId: string)
    {
      this.chartId = chartId;
    }
  }
}
