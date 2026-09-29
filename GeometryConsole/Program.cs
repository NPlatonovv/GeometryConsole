using GeometryConsole.Services;

namespace GeometryConsole;

public class Program
{
    public static void Main(string[] args)
    {
        string? filePath = null;
        string? operation = null;

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-f":
                case "--file":
                    if (i + 1 >= args.Length)
                    {
                        Console.WriteLine(
                            "Ошибка: после -f необходимо указать путь к файлу.");

                        return;
                    }

                    filePath = args[++i];
                    break;

                case "-o":
                case "--oper":
                    if (i + 1 >= args.Length)
                    {
                        Console.WriteLine(
                            "Ошибка: после -o необходимо указать операцию.");

                        return;
                    }

                    operation = args[++i].ToLowerInvariant();
                    break;

                default:
                    Console.WriteLine(
                        $"Ошибка: неизвестный аргумент {args[i]}.");

                    return;
            }
        }

        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine(
                "Ошибка: необходимо указать файл через -f или --file.");

            return;
        }

        if (string.IsNullOrWhiteSpace(operation))
        {
            Console.WriteLine(
                "Ошибка: необходимо указать операцию через -o или --oper.");

            return;
        }

        if (operation != "print" && operation != "count")
        {
            Console.WriteLine(
                "Ошибка: допустимые операции: print, count.");

            return;
        }

        if (!File.Exists(filePath))
        {
            Console.WriteLine(
                $"Ошибка: файл не найден: {filePath}");

            return;
        }

        try
        {
            List<object> figures = FileService.ReadFigures(filePath);

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
            Console.WriteLine(
                $"Ошибка чтения файла: {ex.Message}");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine(
                "Ошибка: нет доступа к файлу.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Произошла ошибка: {ex.Message}");
        }
    }
}