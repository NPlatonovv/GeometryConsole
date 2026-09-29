namespace GeometryConsole.Services;

public static class Logger
{
    public static void Error(
        string message,
        string? logPath)
    {

        if (logPath == null)
        {
            Console.Error.WriteLine(message);
            return;
        }


        File.AppendAllText(
            logPath,
            $"{DateTime.Now}: {message}{Environment.NewLine}");
    }
}