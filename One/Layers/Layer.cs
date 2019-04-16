using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameClusterFuck.Primitives;

namespace MonoGameClusterFuck.Layers
{
    public class Layer
    {
        private readonly object _sync = new object();
        private readonly LayerType _type;
        public List<Sprite> Sprites;

        public Layer(LayerType type)
        {
            _type = type;
            Sprites = new List<Sprite>();
        }

        public void Draw()
        {
            lock (_sync)
            {
                foreach (var sprite in Sprites)
                    sprite.Draw(_type);
            }
        }

        internal void Add(Sprite sprite)
        {
            lock (_sync)
                Sprites.Add(sprite);
            sprite.Initialize();
            sprite.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            lock (_sync)
            {
                foreach (var sprite in Sprites)
                    sprite.Update(gameTime);
            }
        }

        public void LoadContent()
        {
            lock (_sync)
            {
                foreach (var sprite in Sprites)
                {
                    sprite.LoadContent();
                }
            }
        }
    }
}