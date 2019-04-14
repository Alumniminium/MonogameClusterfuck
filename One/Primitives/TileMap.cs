using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClusterFuck.Primitives
{
    public class TileSet
    {
        public readonly int TileSize;
        public Texture2D Atlas;
        public List<Tile> Tiles;

        public TileSet(int tilesize)
        {
            TileSize = tilesize;
            Tiles = new List<Tile>();
        }

        public void Slice()
        {
            Atlas = Engine.Instance.Content.Load<Texture2D>("terrain");

            for (int x = 0; x < Atlas.Width; x += TileSize)
            {
                for (int y = 0; y < Atlas.Height; y += TileSize)
                {
                    var tile = new Tile(TileSize);
                    tile.Texture = Atlas;
                    tile.Source = new Rectangle(x, y, TileSize, TileSize);
                    Tiles.Add(tile);
                }
            }
        }
    }
}