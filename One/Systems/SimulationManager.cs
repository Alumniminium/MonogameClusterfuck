using System.Collections.Generic;
using Microsoft.Xna.Framework;
using One.Primitives.WorldGen;

namespace One.Systems
{
    public static class SimulationManager
    {
        public static Dictionary<Vector2, Chunk> LoadedChunks;

        static SimulationManager()
        {
            LoadedChunks = new Dictionary<Vector2, Chunk>();
        }

        public static void LoadArea(Rectangle area)
        {
            Vector2 index;
            for (var y = area.Y; y < area.Bottom; y += Chunk.ChunkSize)
            {
                for (var x = area.X; x < area.Right; x += Chunk.ChunkSize)
                {
                    index = Chunk.Coord2Chunk(x, y);
                    LoadChunk(index);
                }
            }
        }

        public static void LoadChunk(Vector2 index)
        {
            if (!LoadedChunks.ContainsKey(index))
            {
                var chunk = new Chunk(index);
                LoadedChunks[index] = chunk;
                chunk.Initialize();
            }
        }

        public static void Step()
        {
            foreach (var chunk in LoadedChunks)
            {
                chunk.Value.Step();
            }
        }
    }
}