// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Monitor
{
  /**
   * A model class representing a single climatic data measurement.
   */
  export class Measurement
  {
    /**
     * The measurement timestamp.
     */
    public d: Date | null = null;

    /**
     * The measured pressure value expressed in mmHg.
     */
    public p: number | null = null;

    /**
     * The measured temperature value expressed in degrees Celsius.
     */
    public t: number | null = null;

    /**
     * The measured humidity value expressed in percents.
     */
    public h: number | null = null;
  }
}
