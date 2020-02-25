using System;

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
    public void WriteNotice(string message)
    {
      Console.BackgroundColor = NoticeBackgroundColor;
      Console.ForegroundColor = NoticeTextColor;
      Console.Out.WriteLine(message);
      Console.ResetColor();
    }

    /// <summary>
    ///   Writes the formatted data message to the console standard output stream.
    /// </summary>
    /// <param name="message">
    ///   The message to be written
    /// </param>
    public void WriteData(string message)
    {
      Console.BackgroundColor = DataBackgroundColor;
      Console.ForegroundColor = DataTextColor;
      Console.Out.WriteLine(message);
      Console.ResetColor();
    }

    /// <summary>
    ///   Writes the formatted warning message to the console standard output stream.
    /// </summary>
    /// <param name="message">
    ///   The message to be written
    /// </param>
    public void WriteWarning(string message)
    {
      Console.BackgroundColor = WarningBackgroundColor;
      Console.ForegroundColor = WarningTextColor;
      Console.Out.WriteLine(message);
      Console.ResetColor();
    }

    /// <summary>
    ///   Writes the formatted error message to the console standard error stream.
    /// </summary>
    /// <param name="message">
    ///   The message to be written
    /// </param>
    public void WriteError(string message)
    {
      Console.BackgroundColor = ErrorBackgroundColor;
      Console.ForegroundColor = ErrorTextColor;
      Console.Error.WriteLine(message);
      Console.ResetColor();
    }
  }
}
