/**
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *
 * Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>
 */

/**
 * @summary Initializes a new <i>Chart.js</i> chart object.
 * @param chartId {string} The HTML <i>canvas</i> element ID to be used for chart rendering.
 * @param config {object} The chart configuration object. Gets serialized from the <i>ChartSettings.ChartJsConfig</i>
 *   class.
 */
function initializeChart(chartId, config)
{
  "use strict";

  if (!chartId)
  {
    console.error("No chart ID is provided.");
    return;
  }
  if (!config)
  {
    console.error("The provided chart configuration object is empty.");
    return;
  }
  if (!moment)
  {
    console.error("Moment.js library is missing.");
    return;
  }
  if (!Chart)
  {
    console.error("Chart.js library is missing.");
    return;
  }

  // Setting the callbacks for chart tooltip format customization.
  moment.locale(config.format.locale);
  config.options.tooltips.callbacks = {
    title: tooltipItems => moment(tooltipItems[0].xLabel).format("L LTS"),
    label: tooltipItem =>
    {
      let units = [
        config.format.units.pressureUnits,
        config.format.units.temperatureUnits,
        config.format.units.humidityUnits
      ];
      return `${tooltipItem.yLabel} ${units[tooltipItem.datasetIndex]}`
    }
  }

  // Removing the previous chart objects.
  if (window[`chart:${chartId}`] !== undefined)
  {
    window[`chart:${chartId}`].destroy();
    delete window[`chart:${chartId}`];
  }
  if (window[`chartSettings:${chartId}`] !== undefined)
    delete window[`chartSettings:${chartId}`];

  // Creating and registering a new chart object.
  window[`chartSettings:${chartId}`] = config;
  window[`chart:${chartId}`] = new Chart(chartId, config);
}
