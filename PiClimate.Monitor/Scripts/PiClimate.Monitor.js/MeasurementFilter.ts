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
    private _resolution = MeasurementFilter.defaultResolution;

    /**
     * Defines the minimal data resolution within the selected timespan.
     */
    public static readonly minimalResolution: number = 1;

    /**
     * Defines the default data resolution within the selected timespan.
     */
    public static readonly defaultResolution: number = 1440;

    /**
     * Defines the default time period in milliseconds to be used for the timespan.
     */
    public static readonly defaultTimePeriod: number = 24 * 60 * 60 * 1000; // 1 day period.

    /**
     * Gets or sets the time period in seconds defining the beginning of the selected timespan relatively to
     * the `toTime` property value.
     */
    public get timePeriod(): number
    {
      return Math.round(Math.abs(new Date(this.toTime).valueOf() - new Date(this.fromTime).valueOf()) / 1000);
    }
    public set timePeriod(value: number)
    {
      this.fromTime = new Date(new Date(this.toTime).valueOf() - value * 1000).toISOString();
    }

    /**
     * The beginning of the selected timespan.
     */
    public fromTime: string = new Date(Date.now() - MeasurementFilter.defaultTimePeriod).toISOString();

    /**
     * The ending of the selected timespan.
     */
    public toTime: string = new Date().toISOString();

    /**
     * Gets or sets the data resolution to be used within the selected timespan.
     * The actual number of filtered data entries may not be equal to the provided value.
     */
    public get resolution(): number
    {
      return this._resolution
    }
    public set resolution(value: number)
    {
      this._resolution = Math.max(value, MeasurementFilter.minimalResolution)
    }
  }
}
