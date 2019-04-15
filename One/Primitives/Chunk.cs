using Microsoft.Xna.Framework;

namespace MonoGameClusterFuck.Primitives
{
    public class Chunk
    {
        /// <summary>
        /// The chunk side in world cordinates
        /// </summary>
        public const int ChunkSize = 16;

        /// <summary>
        /// Gets or sets the blocks in the Chunk
        /// </summary>
        public Tile[] Blocks
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the location of the chunk
        /// </summary>
        public Vector2 Location
        {
            get;
            set;
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="location">The location of the chunk</param>
        public Chunk(Vector2 location)
        {
            Location = location;
            Blocks = new Tile[ChunkSize * ChunkSize];
            Generate();
        }

        public void Generate()
        {
            for (int x = 0; x < ChunkSize; x++)
            {
                for (int y = 0; y < ChunkSize; y++)
                {
                    if (Engine.NoiseGen.GetCellular(x, y) > 0.4)
                    {
                        Blocks[x * ChunkSize + y] = Engine.TileSet.Tiles[357].Clone();
                    }
                    else
                    {
                        Blocks[x * ChunkSize + y] = Engine.TileSet.Tiles[5].Clone();
                    }
                    Blocks[x * ChunkSize + y].Position = new Vector2(Location.X + x*32, Location.Y + y * 32);
                }
            }
        }
    }
}
