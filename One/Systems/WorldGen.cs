using System;
using NoiseGen;
using MonoGame;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace One.Systems
{
    public static class WorldGen
    {
        public const int CHUNK_WIDTH = 8;
        public const int CHUNK_HEIGHT = 8;
        public static FastNoise NoiseGen = new FastNoise(1337);
        public const string SEED = "Autism";
        public static Random PRNG;

        // >0
        public const float SCALE = 10000f;
        // >=1
        public const int OCTAVES = 7;
        public const float PERSISTENCE = 0.4f;
        // >1
        public const float LACUNARITY = 2f;
        static Vector2[] octaveOffsets = new Vector2[OCTAVES];

        static float maxNoiseHeight = float.MinValue;
        static float minNoiseHeight = float.MaxValue;
        static WorldGen()
        {
            int seed = 0;
            for (int i = 0; i < SEED.Length; i++)
            {
                seed += (sbyte)SEED[i];
            }
            PRNG = new Random(seed);

            for (int i = 0; i < OCTAVES; i++)
            {
                var offsetX = PRNG.Next(-100000, 100000);
                var offsetY = PRNG.Next(-100000, 100000);
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }
        }
        public static float[,] GenerateChunk(float x, float y)
        {
            float[,] noiseMap = new float[CHUNK_WIDTH, CHUNK_HEIGHT];

            for (int cx = 0; cx < CHUNK_WIDTH; cx++)
            {
                for (int cy = 0; cy < CHUNK_HEIGHT; cy++)
                {
                    var noiseHeight = Generate(x+cx,y+cy);

                    if (noiseHeight > maxNoiseHeight)
                        maxNoiseHeight = noiseHeight;
                    else if (noiseHeight < minNoiseHeight)
                        minNoiseHeight = noiseHeight;

                    noiseMap[cx, cy] = noiseHeight;
                }
            }
            for (int cx = 0; cx < CHUNK_WIDTH; cx++)
            {
                for (int cy = 0; cy < CHUNK_HEIGHT; cy++)
                {
                    noiseMap[cx, cy] = MathEx.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[cx, cy]);
                }
            }
            return noiseMap;
        }
        public static float Generate(float x, float y)
        {
            float amplitude = 1;
            float frequency = 1;
            float total = 0;
            float totalAmp=0;
            for (int o = 0; o < OCTAVES; o++)
            {
                float sampleX = x + octaveOffsets[o].X / SCALE * frequency;
                float sampleY = y + octaveOffsets[o].Y / SCALE * frequency;
                float simplexVal = NoiseGen.GetSimplex(sampleX, sampleY) * 2 - 1;
                total += simplexVal * amplitude;
                amplitude *= PERSISTENCE;
                frequency *= LACUNARITY;
            }

            return total/totalAmp;
        }
    }
}