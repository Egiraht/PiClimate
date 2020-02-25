using System;

namespace PiClimate.Logger.Components
{
  /// <summary>
  ///   The measurement loop options.
  /// </summary>
  public class MeasurementLoopOptions
  {
    private int _measurementLoopDelay = 60;

    /// <summary>
    ///   Gets or sets the measurement loop delay value in seconds.
    /// </summary>
    public int MeasurementLoopDelay
    {
      get => _measurementLoopDelay;
      set => _measurementLoopDelay = Math.Max(value, 1);
    }
  }
}
