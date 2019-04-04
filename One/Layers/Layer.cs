using System.Collections.Generic;
using MonoGameClusterFuck.Primitives;

namespace MonoGameClusterFuck.Layers
{
    public class Layer
    {
        private readonly LayerType _type;
        public List<DrawableComponent> Sprites;
        public Layer(LayerType type)
        {
            _type=type;
            Sprites=new List<DrawableComponent>();
        }

        public void Draw()
        {
            foreach(var sprite in Sprites)
            {
                sprite.Draw(_type);
            }
        }

        internal void Add(DrawableComponent cursor)
        {
            Sprites.Add(cursor);
        }
    }
}