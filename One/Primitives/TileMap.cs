using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClusterFuck.Primitives
{
    public class TileSet
    {
        public readonly int TileSize;
        public Texture2D Atlas;
        public List<Sprite> Tiles;

        public TileSet(int tilesize)
        {
            TileSize = tilesize;
            Tiles = new List<Sprite>();
        }

        public void Slice()
        {
            Atlas = Engine.Instance.Content.Load<Texture2D>("terrain");

            for (var x = 0; x < Atlas.Width; x += TileSize)
            {
                for (var y = 0; y < Atlas.Height; y += TileSize)
                {
                    var tile = new Sprite(TileSize,0);
                    tile.Texture = Atlas;
                    tile.Source = new Rectangle(x, y, TileSize, TileSize);
                    Tiles.Add(tile);
                }
            }
        }
    }
}