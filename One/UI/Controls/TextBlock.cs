using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace One.UI.Controls
{
    public class TextBlock : UserControl
    {
        public string Text = "Change me";
        private Vector2 stringSize;
        private UserControl backgroundd;

        public TextBlock() : base("selectionrect")
        {
            Construct();
        }

        public int Width => (int)Fonts.ProFont.MeasureString(Text).X;

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
            backgroundd.Position.X -= stringSize.X;
            backgroundd.Update(gameTime);
        }

        public override void Draw()
        {
            Engine.SpriteBatch.DrawString(Fonts.ProFont, Text, Position, Color.White, Rotation, RotationOrigin, Scale, SpriteEffects.None, LayerDepth);
            base.Draw();
        }
    }
}
