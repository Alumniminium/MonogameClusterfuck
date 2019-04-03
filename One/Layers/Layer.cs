using System.Collections.Generic;
using MonoGameClusterFuck.Primitives;

namespace MonoGameClusterFuck.Layers
{
    public class Layer
    {
        private readonly LayerType _type;
        public List<Sprite> Sprites;
        public Layer(LayerType type)
        {
            _type=type;
            Sprites=new List<Sprite>();
        }

        public void Draw()
        {
            foreach(var sprite in Sprites)
            {
                sprite.Draw(_type);
            }
        }

        internal void Add(Sprite cursor)
        {
            Sprites.Add(cursor);
        }
    }
}