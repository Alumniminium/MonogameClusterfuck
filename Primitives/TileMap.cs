using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monogame.Primitives
{
    public class TileSet
    {
        public int TileSize = 32;
        public Texture2D FullTileSet;
        public Texture2D[] Tiles;

        public TileSet(int tileSize)
        {
            TileSize = tileSize;
        }
        internal void LoadContent(ContentManager content)
        {
            FullTileSet = content.Load<Texture2D>("terrain");
            Split();
        }

        internal void DrawTileSet(SpriteBatch spriteBatch)
        {
            var pos = new Vector2(0, 0);
            int count = 0;
            foreach (var tile in Tiles)
            {
                spriteBatch.Draw(tile, pos);
                spriteBatch.DrawString(Fonts.Generic, count.ToString(), pos, Color.White);
                pos.X += TileSize;
                if (pos.X > FullTileSet.Width)
                {
                    pos.Y += TileSize;
                    pos.X = 0;
                }
                count++;
            }
        }

        internal void DrawBgTile(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Game.Instance.Width; x += TileSize)
            {
                for (int y = 0; y < Game.Instance.Height; y += TileSize)
                {
                    spriteBatch.Draw(Tiles[109], new Vector2(x, y));
                }
            }
        }


        private void Split()
        {
            var yCount = FullTileSet.Height / TileSize + (TileSize % FullTileSet.Height == 0 ? 0 : 1);//The number of textures in each horizontal row
            var xCount = FullTileSet.Height / TileSize + (TileSize % FullTileSet.Height == 0 ? 0 : 1);//The number of textures in each vertical column
            Tiles = new Texture2D[xCount * yCount];//Number of parts = (area of original) / (area of each part).
            int resolution = TileSize * TileSize;//Number of pixels in each of the split parts

            //Get the pixel data from the original texture:
            Color[] originalData = new Color[FullTileSet.Width * FullTileSet.Height];
            FullTileSet.GetData<Color>(originalData);

            int index = 0;
            for (int y = 0; y < yCount * TileSize; y += TileSize)
            {
                for (int x = 0; x < xCount * TileSize; x += TileSize)
                {
                    //The texture at coordinate {x, y} from the top-left of the original texture
                    Texture2D chunk = new Texture2D(FullTileSet.GraphicsDevice, TileSize, TileSize);
                    //The data for part
                    Color[] chunkData = new Color[resolution];

                    //Fill the part data with colors from the original texture
                    for (int height = 0; height < TileSize; height++)
                    {
                        for (int width = 0; width < TileSize; width++)
                        {
                            int partIndex = width + height * TileSize;
                            //If a part goes outside of the source texture, then fill the overlapping part with Color.Transparent
                            if (y + height >= FullTileSet.Height || x + width >= FullTileSet.Width)
                                chunkData[partIndex] = Color.Transparent;
                            else
                                chunkData[partIndex] = originalData[(x + width) + (y + height) * FullTileSet.Width];
                        }
                    }
                    //Fill the part with the extracted data
                    chunk.SetData<Color>(chunkData);
                    //Stick the part in the return array:                    
                    Tiles[index++] = chunk;
                }
            }
        }
    }
}