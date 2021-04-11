// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PiClimate.Logger.Models;
using PiClimate.Logger.Settings;
using UnitsNet;
using UnitsNet.Units;

namespace PiClimate.Logger.Providers
{
  /// <summary>
  ///   A measurement provider that uses the BMEReader hardware adapter to read measurements data from the the BME280
  ///   sensor connected to it.
  /// </summary>
  public class BmeReaderProvider : IMeasurementProvider
  {
    /// <summary>
    ///   Defines the BMEReader identification command.
    /// </summary>
    protected const string IdCommand = "Id";

    /// <summary>
    ///   Defines the BMEReader measuring command.
    /// </summary>
    protected const string MeasureCommand = "Measure All";

    /// <summary>
    ///   Defines the regular expression pattern for valid BMEReader identification strings.
    /// </summary>
    protected const string IdPattern = @"^\s*OK\s*;\s*BMEReader\s*;\s*Version:\s*1\.\d+\s*;\s*SN:\s*[\dA-F]{24}\s*$";

    /// <summary>
    ///   Defines the regular expression pattern for measurement response strings.
    /// </summary>
    protected const string MeasurementPattern =
      @"^\s*OK\s*;\s*P\s*=\s*([\d\.]+)\s*mmHg\s*;\s*T\s*=\s*([\d\.]+)\s*degC\s*;\s*H\s*=\s*([\d\.]+)\s*%\s*$";

    /// <summary>
    ///   The object's disposal flag.
    /// </summary>
    private bool _disposed;

    /// <summary>
    ///   Gets or sets the serial port object used for communication.
    /// </summary>
    protected SerialPort? Port { get; set; }

    /// <inheritdoc />
    public bool IsConfigured { get; protected set; }

    /// <summary>
    ///   Tries to connect to the BMEReader adapter using the provided serial port name.
    /// </summary>
    /// <param name="serialPort">
    ///   The serial port instance used for connection testing.
    /// </param>
    /// <returns>
    ///   <c>true</c> on successful BMEReader adapter connection and identification, otherwise <c>false</c>.
    /// </returns>
    protected static bool TryConnect(SerialPort serialPort)
    {
      try
      {
        serialPort.Open();
        serialPort.DiscardInBuffer();
        serialPort.DiscardOutBuffer();
        serialPort.WriteLine(IdCommand);
        return Regex.IsMatch(serialPort.ReadLine(), IdPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
      }
      catch
      {
        return false;
      }
      finally
      {
        serialPort.Close();
      }
    }

    /// <inheritdoc />
    public virtual void Configure(GlobalSettings settings)
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(BmeReaderProvider));

      Port = new SerialPort
      {
        PortName = settings.BmeReaderOptions.SerialPortName,
        ReadTimeout = settings.BmeReaderOptions.SerialPortTimeout,
        WriteTimeout = settings.BmeReaderOptions.SerialPortTimeout,
        Encoding = Encoding.ASCII,
        NewLine = "\x0A"
      };

      if (!TryConnect(Port))
        throw new IOException($"Failed to connect to the BMEReader adapter device " +
          $"using the serial port name \"{settings.BmeReaderOptions.SerialPortName}\".");

      IsConfigured = true;
    }

    /// <inheritdoc />
    public virtual Task ConfigureAsync(GlobalSettings settings) => Task.Run(() => Configure(settings));

    /// <inheritdoc />
    public virtual Measurement Measure()
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(BmeReaderProvider));

      if (!IsConfigured || Port == null)
        throw new InvalidOperationException($"{nameof(BmeReaderProvider)} is not configured.");

      try
      {
        Port.Open();
        Port.DiscardInBuffer();
        Port.DiscardOutBuffer();
        Port.WriteLine(MeasureCommand);
        var response = Port.ReadLine();
        var match = Regex.Match(response, MeasurementPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        if (!match.Success)
          throw new FormatException($"Invalid response received: {response}");

        return new Measurement
        {
          Timestamp = DateTime.Now,
          Pressure =
            new Pressure(double.Parse(match.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture),
              PressureUnit.MillimeterOfMercury),
          Temperature =
            new Temperature(double.Parse(match.Groups[2].Value, NumberStyles.Any, CultureInfo.InvariantCulture),
              TemperatureUnit.DegreeCelsius),
          Humidity =
            new RelativeHumidity(double.Parse(match.Groups[3].Value, NumberStyles.Any, CultureInfo.InvariantCulture),
              RelativeHumidityUnit.Percent)
        };
      }
      catch (Exception e)
      {
        throw new IOException($"Failed to read the measurement data from the BMEReader adapter: {e.Message}", e);
      }
      finally
      {
        Port.Close();
      }
    }

    /// <inheritdoc />
    public virtual Task<Measurement> MeasureAsync() => Task.Run(Measure);

    /// <inheritdoc />
    public virtual void Dispose()
    {
      if (_disposed)
        return;

      Port?.Dispose();

      GC.SuppressFinalize(this);
      _disposed = true;
    }

    /// <inheritdoc />
    ~BmeReaderProvider() => Dispose();
  }
}
