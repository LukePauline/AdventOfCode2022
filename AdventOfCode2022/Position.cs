namespace AdventOfCode2022
{
    public record Position(int X, int Y)
    {
        public static (int dX, int dY) operator -(Position a, Position b) => (a.X - b.X, a.Y - b.Y);
        public static Position Parse(string input)
        {
            var parts = input.Split(',');
            return new(int.Parse(parts[0]), int.Parse(parts[1]));
        }
    }
}
