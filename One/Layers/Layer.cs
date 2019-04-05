using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameClusterFuck.Primitives;

namespace MonoGameClusterFuck.Layers
{
    public class Layer
    {
        private readonly LayerType _type;
        public List<DrawableComponent> Components;

        public Layer(LayerType type)
        {
            _type=type;
            Components=new List<DrawableComponent>();
        }

        public void Draw()
        {
            foreach(var component in Components)
                component.Draw(_type);
        }

        internal void Add(DrawableComponent component)
        {
            Components.Add(component);
            component.Initialize();
            component.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var component in Components)
                component.Update(gameTime);
        }

        public void LoadContent()
        {
            foreach (var component in Components)
            {
                component.LoadContent();
            }
        }
    }
}