using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClusterFuck
{
    public static class Fonts
    {
        public static SpriteFont Generic = null;

        public static void LoadContent()
        {
            Generic = Engine.Instance.Content.Load<SpriteFont>("Font");
        }

    }
}