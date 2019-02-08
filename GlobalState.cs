using MonoGameClusterFuck.Primitives;
using System.Collections.Generic;

namespace MonoGameClusterFuck
{
    public static class GlobalState
    {
        public static bool DrawTileSet { get; set; }
        public static bool DisplayHelp { get; set; } = true;
        public static int Frames { get; internal set; }
    }
    public static class Assets
    {
        public static Dictionary<int, Sprite> Tiles = new Dictionary<int, Sprite>
        {
            [0] = new Sprite(32),
        };
    }
}