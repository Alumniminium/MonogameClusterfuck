using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClusterFuck
{
    public static class Fonts
    {
        public static SpriteFont Generic = null;

        public static void LoadContent(ContentManager cm)
        {
            Generic = cm.Load<SpriteFont>("Font");
        }

    }
}