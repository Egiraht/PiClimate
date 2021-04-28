/**
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *
 * Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>
 */

/**
 * @summary Initializes a new <i>Chart.js</i> mini-chart object.
 * @param canvasId {string} The HTML <i>canvas</i> element ID to be used for mini-chart rendering.
 * @param config {object} The mini-chart configuration object. Gets serialized from the
 *   <i>MiniChartSettings.ChartJsConfig</i> class.
 */
function initializeMiniChart(canvasId, config)
{
  "use strict";

  if (!canvasId)
  {
    console.error("No mini-chart canvas element ID is provided.");
    return;
  }
  if (!config)
  {
    console.error("The provided mini-chart configuration object is empty.");
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

  // Removing the previous chart objects.
  if (window[`miniChart:${canvasId}`] !== undefined)
  {
    window[`miniChart:${canvasId}`].destroy();
    delete window[`miniChart:${canvasId}`];
  }
  if (window[`miniChartSettings:${canvasId}`] !== undefined)
    delete window[`miniChartSettings:${canvasId}`];

  // Creating and registering a new chart object.
  window[`miniChartSettings:${canvasId}`] = config;
  window[`miniChart:${canvasId}`] = new Chart(canvasId, config);
}

/**
 * @summary Initializes a new <i>Chart.js</i> chart object.
 * @param canvasId {string} The HTML <i>canvas</i> element ID to be used for chart rendering.
 * @param config {object} The chart configuration object. Gets serialized from the <i>ChartSettings.ChartJsConfig</i>
 *   class.
 */
function initializeClimaticChart(canvasId, config)
{
  "use strict";

  if (!canvasId)
  {
    console.error("No chart canvas element ID is provided.");
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
  config.options.plugins.tooltip.callbacks = {
    title: tooltipItems => moment(tooltipItems[0].raw.x).format("L LTS"),
    label: tooltipItem =>
    {
      let units = [
        config.format.units.pressureUnits,
        config.format.units.temperatureUnits,
        config.format.units.humidityUnits
      ];
      return `${tooltipItem.formattedValue} ${units[tooltipItem.datasetIndex]}`
    }
  }

  // Removing the previous chart objects.
  if (window[`climaticChart:${canvasId}`] !== undefined)
  {
    window[`climaticChart:${canvasId}`].destroy();
    delete window[`climaticChart:${canvasId}`];
  }
  if (window[`climaticChartSettings:${canvasId}`] !== undefined)
    delete window[`climaticChartSettings:${canvasId}`];

  // Creating and registering a new chart object.
  window[`climaticChartSettings:${canvasId}`] = config;
  window[`climaticChart:${canvasId}`] = new Chart(canvasId, config);
}
