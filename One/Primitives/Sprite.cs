using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClusterFuck.Primitives
{
    public class Sprite
    {
        private Vector2 _position;
        public Texture2D Texture;
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                Destination.Location = value.ToPoint();
            }
        }
        public Point Size;
        public float Rotation;
        public Vector2 RotationOrigin;
        public Rectangle Source, Destination;

        public Sprite(int size)
        {
            Size = new Point(size);
            
        }
        
        public virtual void LoadContent()
        {
            Console.WriteLine("Loading Content");
        }

        public virtual void Initialize()
        {
            Destination = new Rectangle(0, 0, Size.X, Size.Y);
            Source = new Rectangle(0, 0, Size.X, Size.Y);
            
            Position = new Vector2(Size.X/2f, Size.Y/2f);
            Rotation = 0;
            RotationOrigin = new Vector2(Size.X / 2f, Size.Y / 2f);
        }       
        public virtual void Update(GameTime deltaTime)
        {

        }
        public virtual void Draw(Layers.LayerType layer)
        {
            Engine.SpriteBatch.Draw(Texture, Destination, Source, Color.White, Rotation, RotationOrigin, SpriteEffects.None, (float)layer/ 100f);
        }
    }
}