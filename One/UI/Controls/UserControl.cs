using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClusterFuck.UI.Controls
{
    public class UserControl
    {
        public float LayerDepth;
        public Vector2 Position = Vector2.Zero;
        public Vector2 RotationOrigin = Vector2.Zero;
        public Vector2 Scale = Vector2.One;
        public Rectangle SourceRect;
        public Texture2D Texture;
        public string TextureName;
        public float Rotation;

        public UserControl(string textureName)
        {
            TextureName = textureName;
        }

        public virtual void Initialize()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>(TextureName);
            RotationOrigin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            SourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Scale = Vector2.One;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw() => Engine.SpriteBatch.Draw(Texture,Position,SourceRect,Color.White,Rotation,RotationOrigin,Scale,SpriteEffects.None,LayerDepth);
    }

    public class TextBlock
    {
        public string Text = "Change me";
        public List<UserControl> Controls = new List<UserControl>();

        public TextBlock()
        {
            
        }

        public void Construct()
        {
            var backgroundd = new UserControl("");

            Controls.Add(backgroundd);
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
