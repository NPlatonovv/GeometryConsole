using GeometryConsole.Models;

namespace GeometryConsole.Services;

public static class FileService
{
    public static List<object> ReadFigures(string filePath)
    {
        var figures = new List<object>();

        string[] lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            object? figure = FigureParser.Parse(line);

            if (figure is not null)
            {
                figures.Add(figure);
            }
            else
            {
                Console.WriteLine(
                    $"Предупреждение: строка {i + 1} пропущена: {line}");
            }
        }

        return figures;
    }
}