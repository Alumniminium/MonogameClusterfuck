using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClusterFuck.Primitives
{
    public abstract class DrawableComponent
    {
        public Texture2D Texture;
        public Vector2 Position { get; set; }

        public Point Size => Texture.Bounds.Size;
        public float Rotation;
        public Vector2 RotationOrigin;
        public Rectangle Source;
        
        public virtual void Initialize()
        {
            Source = new Rectangle(0, 0, Size.X, Size.Y);
            
            Position = new Vector2(Size.X/2f, Size.Y/2f);
            Rotation = 0;
            RotationOrigin = new Vector2(Size.X / 2f, Size.Y / 2f);
        }

        public abstract void LoadContent();

        public abstract void Update(GameTime deltaTime);

        public virtual void Draw(Layers.LayerType layer)
        {
            Engine.SpriteBatch.Draw(Texture, Position, Source, Color.White, Rotation, RotationOrigin, Vector2.One, SpriteEffects.None, (float)layer/ 100f);
        }
    }
}