using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monogame.Primitives
{
    public class TileMap
    {
        public Texture2D Tilemap;
        public Texture2D[] Tiles;

        public Texture2D[] Split(int tileWidth, int tileHeight)
        {
            var yCount = Tilemap.Height / tileHeight + (tileHeight % Tilemap.Height == 0 ? 0 : 1);//The number of textures in each horizontal row
            var xCount = Tilemap.Height / tileHeight + (tileHeight % Tilemap.Height == 0 ? 0 : 1);//The number of textures in each vertical column
            Texture2D[] r = new Texture2D[xCount * yCount];//Number of parts = (area of original) / (area of each part).
            int dataPerPart = tileWidth * tileHeight;//Number of pixels in each of the split parts

            //Get the pixel data from the original texture:
            Color[] originalData = new Color[Tilemap.Width * Tilemap.Height];
            Tilemap.GetData<Color>(originalData);

            int index = 0;
            for (int y = 0; y < yCount * tileHeight; y += tileHeight)
                for (int x = 0; x < xCount * tileWidth; x += tileWidth)
                {
                    //The texture at coordinate {x, y} from the top-left of the original texture
                    Texture2D part = new Texture2D(Tilemap.GraphicsDevice, tileWidth, tileHeight);
                    //The data for part
                    Color[] partData = new Color[dataPerPart];

                    //Fill the part data with colors from the original texture
                    for (int py = 0; py < tileHeight; py++)
                        for (int px = 0; px < tileWidth; px++)
                        {
                            int partIndex = px + py * tileWidth;
                            //If a part goes outside of the source texture, then fill the overlapping part with Color.Transparent
                            if (y + py >= Tilemap.Height || x + px >= Tilemap.Width)
                                partData[partIndex] = Color.Transparent;
                            else
                                partData[partIndex] = originalData[(x + px) + (y + py) * Tilemap.Width];
                        }

                    //Fill the part with the extracted data
                    part.SetData<Color>(partData);
                    //Stick the part in the return array:                    
                    r[index++] = part;
                }
            return r;
        }

        internal void LoadContent(ContentManager content)
        {
            Tilemap = content.Load<Texture2D>("terrain");
            Tiles = Split(32, 32);

            for (int i = 0; i < Tiles.Length; i++)
                Db.AssetDb.Textures.Add(i, Tiles[i]);
        }
    }
}