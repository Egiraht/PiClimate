// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Globalization;

namespace PiClimate.Logger.Components
{
  /// <summary>
  ///   The console writer class used for console output message formatting.
  /// </summary>
  public class ConsoleWriter
  {
    /// <summary>
    ///   Gets or sets the text color for notice messages.
    /// </summary>
    public ConsoleColor NoticeTextColor { get; set; } = ConsoleColor.White;

    /// <summary>
    ///   Gets or sets the background color for notice messages.
    /// </summary>
    public ConsoleColor NoticeBackgroundColor { get; set; } = ConsoleColor.Black;

    /// <summary>
    ///   Gets or sets the text color for data messages.
    /// </summary>
    public ConsoleColor DataTextColor { get; set; } = ConsoleColor.DarkCyan;

    /// <summary>
    ///   Gets or sets the background color for data messages.
    /// </summary>
    public ConsoleColor DataBackgroundColor { get; set; } = ConsoleColor.Black;

    /// <summary>
    ///   Gets or sets the text color for warning messages.
    /// </summary>
    public ConsoleColor WarningTextColor { get; set; } = ConsoleColor.DarkYellow;

    /// <summary>
    ///   Gets or sets the background color for warning messages.
    /// </summary>
    public ConsoleColor WarningBackgroundColor { get; set; } = ConsoleColor.Black;

    /// <summary>
    ///   Gets or sets the text color for error messages.
    /// </summary>
    public ConsoleColor ErrorTextColor { get; set; } = ConsoleColor.DarkRed;

    /// <summary>
    ///   Gets or sets the background color for error messages.
    /// </summary>
    public ConsoleColor ErrorBackgroundColor { get; set; } = ConsoleColor.Black;

    /// <summary>
    ///   Writes the formatted notice message to the console standard output stream.
    /// </summary>
    /// <param name="message">
    ///   The message to be written
    /// </param>
    /// <param name="printTimestamp">
    ///   Selects if a timestamp should be printed before the message.
    /// </param>
    public void WriteNotice(string message, bool printTimestamp = true)
    {
      Console.BackgroundColor = NoticeBackgroundColor;
      Console.ForegroundColor = NoticeTextColor;
      if (printTimestamp)
        Console.Out.Write($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] ");
      Console.Out.WriteLine(message);
      Console.ResetColor();
    }

    /// <summary>
    ///   Writes the formatted data message to the console standard output stream.
    /// </summary>
    /// <param name="message">
    ///   The message to be written
    /// </param>
    /// <param name="printTimestamp">
    ///   Selects if a timestamp should be printed before the message.
    /// </param>
    public void WriteData(string message, bool printTimestamp = true)
    {
      Console.BackgroundColor = DataBackgroundColor;
      Console.ForegroundColor = DataTextColor;
      if (printTimestamp)
        Console.Out.Write($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] ");
      Console.Out.WriteLine(message);
      Console.ResetColor();
    }

    /// <summary>
    ///   Writes the formatted warning message to the console standard output stream.
    /// </summary>
    /// <param name="message">
    ///   The message to be written
    /// </param>
    /// <param name="printTimestamp">
    ///   Selects if a timestamp should be printed before the message.
    /// </param>
    public void WriteWarning(string message, bool printTimestamp = true)
    {
      Console.BackgroundColor = WarningBackgroundColor;
      Console.ForegroundColor = WarningTextColor;
      if (printTimestamp)
        Console.Error.Write($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] ");
      Console.Error.WriteLine(message);
      Console.ResetColor();
    }

    /// <summary>
    ///   Writes the formatted error message to the console standard error stream.
    /// </summary>
    /// <param name="message">
    ///   The message to be written
    /// </param>
    /// <param name="printTimestamp">
    ///   Selects if a timestamp should be printed before the message.
    /// </param>
    public void WriteError(string message, bool printTimestamp = true)
    {
      Console.BackgroundColor = ErrorBackgroundColor;
      Console.ForegroundColor = ErrorTextColor;
      if (printTimestamp)
        Console.Error.Write($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] ");
      Console.Error.WriteLine(message);
      Console.ResetColor();
    }
  }
}
