using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClusterFuck.Primitives
{
    public class Sprite
    {
        public float LayerDepth =0f;
        public Texture2D Texture;
        public Point Position;
        public Point Size;
        public float Rotation;
        public Vector2 RotationOrigin;
        public Rectangle Source, Destination;

        public Sprite(int size)
        {
            Size = new Point(size);
            Position = new Point(0, 0);
            Destination = new Rectangle(Position, Size);
            Source = new Rectangle(0, 0, size, size);
            Rotation = 0;
            RotationOrigin = new Vector2(size/2, size/2);
        }

        public virtual void LoadContent()
        {

        }
        public virtual void Update(GameTime deltaTime)
        {

        }
        public virtual void Draw()
        {
            Game.Instance.SpriteBatch.Draw(Texture, Destination, Source, Color.White, Rotation, RotationOrigin, SpriteEffects.None, LayerDepth);
        }
    }
}