using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClusterFuck.UI.Controls
{
    public class UserControl
    {
        public UserControl Parent;
        public float LayerDepth = 0.01f;
        public Vector2 Position = Vector2.Zero;
        public Vector2 RotationOrigin = Vector2.Zero;
        public Vector2 Scale = Vector2.One;
        public Rectangle SourceRect;
        public Texture2D Texture;
        public string TextureName;
        public float Rotation;
        public List<UserControl> Children;

        public UserControl(string textureName)
        {
            TextureName = textureName;
            Children = new List<UserControl>();
        }

        public void AddChild(UserControl child)
        {
            child.Parent=this;
            Children.Add(child);
        }

        public virtual void Initialize()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>(TextureName);
            RotationOrigin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            SourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Scale = Vector2.One;

            if(Parent != null)
            {
                Position = Parent.Position;
                LayerDepth = Parent.LayerDepth+0.01f;
            }
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw() 
        {    
            Engine.SpriteBatch.Draw(Texture,Position,SourceRect,Color.White,Rotation,RotationOrigin,Scale,SpriteEffects.None,LayerDepth);
            foreach(var child in Children)
                child.Draw();
        } 
    }

    public class TextBlock : UserControl
    {
        public string Text = "Change me";
        private Vector2 stringSize;
        private UserControl backgroundd;

        public TextBlock() : base("selectionrect")
        {
            Construct();
        }

        public int Width { get; set; }

        public void Construct()
        {
            LayerDepth = LayerDepth - 0.01f;
            backgroundd = new UserControl("selectionrect");
            backgroundd.Initialize();
            AddChild(backgroundd);
            stringSize = Fonts.ProFont.MeasureString(Text);
        }

        public override void Update(GameTime gameTime)
        {
            backgroundd.Position.Y -= 48;
            backgroundd.Position.X -= stringSize.X/2;
            Width = (int)stringSize.X;
            backgroundd.Update(gameTime);
        }

        public override void Draw()
        {
            Engine.SpriteBatch.DrawString(Fonts.ProFont, Text, Position, Color.White, Rotation, RotationOrigin, Scale, SpriteEffects.None, LayerDepth);
            base.Draw();
        }
    }
}
