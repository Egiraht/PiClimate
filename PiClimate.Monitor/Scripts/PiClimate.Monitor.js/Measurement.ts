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
