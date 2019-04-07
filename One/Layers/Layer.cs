using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
                sprite.Draw(_type);
        }

        internal void Add(Sprite sprite)
        {
            Sprites.Add(sprite);
            sprite.Initialize();
            sprite.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var sprite in Sprites)
                sprite.Update(gameTime);
        }

        public void LoadContent()
        {
            foreach (var sprite in Sprites)
            {
                sprite.LoadContent();
            }
        }
    }
}