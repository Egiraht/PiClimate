// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

/// <reference path="Measurement.ts`

namespace PiClimate.Monitor
{
  /**
   * A model class representing a collection of climatic data measurements.
   */
  export class MeasurementsCollection
  {
    /**
     * The minimal timestamp available in the collection.
     */
    public minTimestamp: string = new Date().toISOString();

    /**
     * The maximal timestamp available in the collection.
     */
    public maxTimestamp: string = new Date().toISOString();

    /**
     * The minimal pressure value available in the collection.
     */
    public minPressure: number = 0;

    /**
     * The maximal pressure value available in the collection.
     */
    public maxPressure: number = 0;

    /**
     * The timestamp corresponding to the `minPressure` value.
     */
    public minPressureTimestamp: string = new Date().toISOString();

    /**
     * The timestamp corresponding to the `maxPressure` value.
     */
    public maxPressureTimestamp: string = new Date().toISOString();

    /**
     * The minimal temperature value available in the collection.
     */
    public minTemperature: number = 0;

    /**
     * The maximal temperature value available in the collection.
     */
    public maxTemperature: number = 0;

    /**
     * The timestamp corresponding to the `minTemperature` value.
     */
    public minTemperatureTimestamp: string = new Date().toISOString();

    /**
     * The timestamp corresponding to the `maxTemperature` value.
     */
    public maxTemperatureTimestamp: string = new Date().toISOString();

    /**
     * The minimal humidity value available in the collection.
     */
    public minHumidity: number = 0;

    /**
     * The maximal humidity value available in the collection.
     */
    public maxHumidity: number = 0;

    /**
     * The timestamp corresponding to the `minHumidity` value.
     */
    public minHumidityTimestamp: string = new Date().toISOString();

    /**
     * The timestamp corresponding to the `maxHumidity` value.
     */
    public maxHumidityTimestamp: string = new Date().toISOString();

    /**
     * The count of data entries in the collection.
     */
    public count: number = 0;

    /**
     * The measurement data items stored in the collection.
     */
    public measurements: Measurement[] = [];
  }
}
