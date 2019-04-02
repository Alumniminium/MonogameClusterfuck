using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck.Layers
{
    public class Layer
    {
        LayerType Type;
        public List<Sprite> Sprites;
        public Layer(LayerType type)
        {
            Type=type;
            Sprites=new List<Sprite>();
        }

        public void Draw()
        {
            foreach(var sprite in Sprites)
            {
                sprite.Draw(Type);
            }
        }

        internal void Add(Sprite cursor)
        {
            Sprites.Add(cursor);
        }
    }
}