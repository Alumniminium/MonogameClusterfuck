using One.SceneManagement.Scenes;

namespace One.SceneManagement.Primitives
{
    struct TileInfo
    {
        public TileType Type;
        public TileInfo((float, float) location)
        {
            var value = InfiniteWorld.NoiseGen.GetCubic(location.Item1, location.Item2);
            if (value > 0.10f)
                Type = TileType.Wall;
            else
                Type = TileType.Ground;
        }
    }
}