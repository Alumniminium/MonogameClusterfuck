namespace monogame
{
    public static class GlobalState
    {
        public static bool TileMapDrawing { get; set; }
        public static bool DisplayHelp { get; set; } = true;
        public static int Frames { get; internal set; }
    }
}