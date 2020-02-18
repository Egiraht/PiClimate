using System;

namespace PiClimate.Logger.Components
{
  public class MeasurementLoopOptions
  {
    private int _measurementLoopDelay = 60;

    public int MeasurementLoopDelay
    {
      get => _measurementLoopDelay;
      set => _measurementLoopDelay = Math.Max(value, 1);
    }
  }
}
