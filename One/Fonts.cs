using Microsoft.Xna.Framework.Graphics;

namespace One
{
    public static class Fonts
    {
        public static SpriteFont Generic = null;
        public static SpriteFont ProFont = null;

        public static void LoadContent()
        {
            Generic = Engine.Instance.Content.Load<SpriteFont>("Font");
            ProFont= Engine.Instance.Content.Load<SpriteFont>("ProFontWindows");
        }

    }
}