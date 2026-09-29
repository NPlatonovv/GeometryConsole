using System.CommandLine;
using GeometryConsole.Models;
using GeometryConsole.Services;

namespace GeometryConsole;

public class Program
{
    public static int Main(string[] args)
    {
        Option<string?> fileOption = new("--file", "-f")
        {
            Description = "Путь к файлу"
        };

        Option<string?> operationOption = new("--oper", "-o")
        {
            Description = "Операция: print или count"
        };

        Option<string?> logOption = new("--log")
        {
            Description = "Файл для записи ошибок"
        };

        RootCommand rootCommand = new(
            "Geometry Console Utility");

        rootCommand.Options.Add(fileOption);
        rootCommand.Options.Add(operationOption);
        rootCommand.Options.Add(logOption);

        rootCommand.SetAction(
            parseResult =>
            {
                string? filePath =
                    parseResult.GetValue(fileOption);

                string? operation =
                    parseResult.GetValue(operationOption);

                string? logPath =
                    parseResult.GetValue(logOption);

                RunProgram(
                    filePath,
                    operation,
                    logPath);
            });

        return rootCommand.Parse(args).Invoke();
    }

    private static void RunProgram(

        string? filePath,
        string? operation,
        string? logPath)
    {
        if (logPath != null)
        {
            SettingsManager.Save(
                new AppSettings
                {
                    LogPath = logPath
                });
        }
        else
        {
            AppSettings? settings =
                SettingsManager.Load();

            if (settings != null)
            {
                logPath = settings.LogPath;
            }
        }

        if (string.IsNullOrWhiteSpace(filePath))
        {
            Logger.Error(
                "Ошибка: необходимо указать файл через -f или --file.",
                logPath);

            return;
        }

        if (string.IsNullOrWhiteSpace(operation))
        {
            Logger.Error(
                "Ошибка: необходимо указать операцию через -o или --oper.",
                logPath);

            return;
        }

        operation = operation.ToLowerInvariant();

        if (operation != "print" &&
            operation != "count")
        {
            Logger.Error(
                "Ошибка: допустимые операции: print, count.",
                logPath);

            return;
        }

        if (!File.Exists(filePath))
        {
            Logger.Error(
                $"Ошибка: файл не найден: {filePath}",
                logPath);

            return;
        }

        try
        {
            List<object> figures =
                FileService.ReadFigures(filePath);

            switch (operation)
            {
                case "print":
                    foreach (object figure in figures)
                    {
                        Console.WriteLine(figure);
                    }

                    break;

                case "count":
                    Console.WriteLine(
                        $"Количество объектов: {figures.Count}");

                    break;
            }
        }
        catch (IOException ex)
        {
            Logger.Error(
                $"Ошибка чтения файла: {ex.Message}",
                logPath);
        }
        catch (UnauthorizedAccessException)
        {
            Logger.Error(
                "Ошибка: нет доступа к файлу.",
                logPath);
        }
        catch (Exception ex)
        {
            Logger.Error(
                $"Произошла ошибка: {ex.Message}",
                logPath);
        }
    }
}