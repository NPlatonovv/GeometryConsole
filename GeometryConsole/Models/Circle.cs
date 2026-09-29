namespace GeometryConsole.Models
{
    public class Circle
    {
        public Point Center { get; }
        public double Radius { get; }

        public Circle(Point center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        public override string ToString()
        {
            return $"Circle({Center}, {Radius})";
        }
    }
}
