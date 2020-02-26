using Microsoft.Xna.Framework;
using NoiseGen;
using System;

namespace One.Primitives.WorldGen
{
    public class Chunk
    {
        public static FastNoise Noise = new FastNoise();
        public const int ChunkSize = 4;
        public Vector2 Index;
        public Cell[,] Cells;

        public Chunk(Vector2 index)
        {
            Index = index;
            Cells = new Cell[ChunkSize, ChunkSize];
        }

        public void Initialize()
        {
            GenerateCells();
        }

        void GenerateCells()
        {
            Vector2 coord;
            Cell cell;

            for (int i = 0; i < ChunkSize; i++)
            {
                for (int j = 0; j < ChunkSize; j++)
                {
                    coord = LocalCoord(i, j);
                    var tile = Noise.GetPerlin(i, j);
                    var sprite = 52;

                    if (tile > 0.3)
                        sprite = 35; //sand
                    if (tile > 0.4)
                        sprite = 41; //grass
                    if (tile > 0.70)
                        sprite = 227; //dirt
                    if (tile > 0.89)
                        sprite = 323; //rock
                    if (tile == 1)
                        sprite = 593; //snow
                    cell = new Cell(sprite);
                    Cells[(int)coord.X, (int)coord.Y] = cell;
                }
            }
        }
        public Cell this[Vector2 index]
        {
            get
            {
                return Cells[(int)index.X, (int)index.Y];
            }
        }

        public Rectangle Rect
        {
            get
            {
                var corner = Index * ChunkSize;
                return new Rectangle((int)corner.X, (int)corner.Y, ChunkSize, ChunkSize);
            }
        }

        public void Step()
        {
        }

        public Vector2 GlobalCoord(int x, int y) => GlobalCoord(new Vector2(x, y));
        public Vector2 GlobalCoord(Vector2 loc) => (Index * ChunkSize) + loc;

        public static Vector2 LocalCoord(int x, int y) => LocalCoord(new Vector2(x, y));
        public static Vector2 LocalCoord(Vector2 coord) => new Vector2(coord.X % ChunkSize, coord.Y % ChunkSize);

        public static Vector2 Coord2Chunk(int x, int y) => Coord2Chunk(new Vector2(x, y));
        public static Vector2 Coord2Chunk(Vector2 coord) => coord / ChunkSize;
    }
}