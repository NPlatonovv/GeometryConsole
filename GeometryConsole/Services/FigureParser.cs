using System.Globalization;
using System.Text.RegularExpressions;
using GeometryConsole.Models;

namespace GeometryConsole.Services;

public static class FigureParser
{
    private const string Number =
        @"[-+]?(?:\d+(?:[.,]\d+)?|[.,]\d+)";


    private const string PositiveNumber =
        @"(?:\d+(?:[.,]\d+)?|[.,]\d+)";

    private static readonly Regex PointRegex = new(
        $@"^Point\(\s*({Number})\s*,\s*({Number})\s*\)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex LineRegex = new(
        $@"^Line\(\s*" +
        $@"Point\(\s*({Number})\s*,\s*({Number})\s*\)\s*,\s*" +
        $@"Point\(\s*({Number})\s*,\s*({Number})\s*\)\s*" +
        $@"\)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex CircleRegex = new(
        $@"^Circle\(\s*" +
        $@"Point\(\s*({Number})\s*,\s*({Number})\s*\)\s*,\s*" +
        $@"({PositiveNumber})\s*" +
        $@"\)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static object? Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        input = input.Trim();

        Match match = LineRegex.Match(input);

        if (match.Success)
        {
            return new Line(
                new Point(
                    ParseNumber(match.Groups[1].Value),
                    ParseNumber(match.Groups[2].Value)),

                new Point(
                    ParseNumber(match.Groups[3].Value),
                    ParseNumber(match.Groups[4].Value)));
        }

        match = CircleRegex.Match(input);

        if (match.Success)
        {
            return new Circle(
                new Point(
                    ParseNumber(match.Groups[1].Value),
                    ParseNumber(match.Groups[2].Value)),

                ParseNumber(match.Groups[3].Value));
        }

        match = PointRegex.Match(input);

        if (match.Success)
        {
            return new Point(
                ParseNumber(match.Groups[1].Value),
                ParseNumber(match.Groups[2].Value));
        }

        return null;
    }

    private static double ParseNumber(string value)
    {
        value = value.Replace(',', '.');

        return double.Parse(
            value,
            CultureInfo.InvariantCulture);
    }
}