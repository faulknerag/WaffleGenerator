namespace WaffleGenerator
{
    public enum POS
    {
        Top,
        Left,
        Vertical,
        Horizontal,
        Right,
        Bottom
    }

    public enum STATE
    {
        Green,
        Yellow,
        Gray
    }

    public struct Coord
    {
        public int row;
        public int col;

        public Coord()
        {
            row = -1;
            col = -1;
        }

        public Coord(int _row, int _col)
        {
            row = _row;
            col = _col;
        }
    };
}