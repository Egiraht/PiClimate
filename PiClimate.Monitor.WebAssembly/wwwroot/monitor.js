/**
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *
 * Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>
 */

/**
 * Initializes a new <i>Chart.js</i> chart object.
 * @param chartId The HTML <i>canvas</i> element ID to be used for chart rendering.
 * @param settings The chart settings object.
 */
function initializeChart(chartId, settings)
{
  // Checking the libraries.
  if (!Chart)
    console.error("Chart.js library is missing!");
  if (!moment)
    console.error("Moment.js library is missing!");

  // Setting the date-time locale.
  moment.locale(settings.format.locale);

  // Setting the callbacks for chart tooltip format customization.
  settings.options.tooltips.callbacks = {
    title: tooltipItems => moment(tooltipItems[0].xLabel).format(settings.format.dateTimeFormat),
    label: tooltipItem =>
    {
      let units = [
        settings.format.units.pressureUnits,
        settings.format.units.temperatureUnits,
        settings.format.units.humidityUnits
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
  window[`chartSettings:${chartId}`] = settings;
  window[`chart:${chartId}`] = new Chart(chartId, settings);
}
