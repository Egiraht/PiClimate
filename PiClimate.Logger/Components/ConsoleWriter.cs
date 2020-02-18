using System;

namespace PiClimate.Logger.Components
{
  internal class ConsoleWriter
  {
    public ConsoleColor NoticeTextColor { get; set; } = ConsoleColor.White;

    public ConsoleColor NoticeBackgroundColor { get; set; } = ConsoleColor.Black;

    public ConsoleColor DataTextColor { get; set; } = ConsoleColor.DarkCyan;

    public ConsoleColor DataBackgroundColor { get; set; } = ConsoleColor.Black;

    public ConsoleColor WarningTextColor { get; set; } = ConsoleColor.DarkYellow;

    public ConsoleColor WarningBackgroundColor { get; set; } = ConsoleColor.Black;

    public ConsoleColor ErrorTextColor { get; set; } = ConsoleColor.DarkRed;

    public ConsoleColor ErrorBackgroundColor { get; set; } = ConsoleColor.Black;

    public void WriteNotice(string message)
    {
      Console.BackgroundColor = NoticeBackgroundColor;
      Console.ForegroundColor = NoticeTextColor;
      Console.Out.WriteLine(message);
      Console.ResetColor();
    }

    public void WriteData(string message)
    {
      Console.BackgroundColor = DataBackgroundColor;
      Console.ForegroundColor = DataTextColor;
      Console.Out.WriteLine(message);
      Console.ResetColor();
    }

    public void WriteWarning(string message)
    {
      Console.BackgroundColor = WarningBackgroundColor;
      Console.ForegroundColor = WarningTextColor;
      Console.Out.WriteLine(message);
      Console.ResetColor();
    }

    public void WriteError(string message)
    {
      Console.BackgroundColor = ErrorBackgroundColor;
      Console.ForegroundColor = ErrorTextColor;
      Console.Error.WriteLine(message);
      Console.ResetColor();
    }
  }
}
