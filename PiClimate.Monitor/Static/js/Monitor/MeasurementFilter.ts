// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Monitor
{
  /**
   * A model class for measurement data filtering.
   */
  export class MeasurementFilter
  {
    /**
     * Defines the default data resolution within the selected timespan.
     */
    public static readonly defaultResolution: number = 1500;

    /**
     * Defines the default time period in milliseconds to be used for the timespan.
     */
    public static readonly defaultTimePeriod: number = 24 * 60 * 60 * 1000; // 1 day period.

    /**
     * Gets or sets the time period in seconds defining the beginning of the selected timespan relatively to
     * the `toTime` property value.
     * This value is used only if the `fromTime` value is `null`.
     * The minimal value must be 1 second.
     */
    public timePeriod: number = MeasurementFilter.defaultTimePeriod;

    /**
     * The beginning of the selected timespan.
     * If the value is `null` the actual timespan beginning will be calculated using the value of the `timePeriod`
     * property.
     */
    public fromTime: string | null = null;

    /**
     * The ending of the selected timespan.
     */
    public toTime: string = new Date().toISOString();

    /**
     * Gets or sets the data resolution to be used within the selected timespan.
     * The actual number of filtered data entries may not be equal to the provided value.
     */
    public resolution: number = MeasurementFilter.defaultResolution;

    /**
     * Gets the `Date` object for the current timespan beginning value depending on the values of the `fromTime` and
     * `timePeriod` properties.
     */
    public getFromTimeDate(): Date
    {
      if (this.fromTime != null)
        return new Date(this.fromTime);

      return new Date(new Date(this.toTime).valueOf() - this.timePeriod * 1000)
    }

    /**
     * Gets the `Date` object for the current timespan ending value.
     */
    public getToTimeDate(): Date
    {
      return new Date(this.toTime);
    }
  }
}
