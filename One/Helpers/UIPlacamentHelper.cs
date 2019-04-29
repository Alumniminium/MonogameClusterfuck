using Microsoft.Xna.Framework;

namespace MonoGameClusterFuck.Helpers
{
    public static class UIPlacementHelper
    {
        public static int Height => Engine.Graphics.PreferredBackBufferHeight;
        public static int Width => Engine.Graphics.PreferredBackBufferWidth;
        public static int HalfHeight => Engine.Graphics.PreferredBackBufferHeight / 2;
        public static int HalfWidth => Engine.Graphics.PreferredBackBufferWidth / 2;
        public static Vector2 Position(Vector2 size, UIElementPositioEnEnum scheme)
        {
            switch (scheme)
            {
                case UIElementPositioEnEnum.BottomLeftCorner:
                    return new Vector2(0, Height - size.Y);
                case UIElementPositioEnEnum.BottomRightCorner:
                    return new Vector2(Width - size.X, 0);
                case UIElementPositioEnEnum.BottomCenter:
                    return new Vector2(CenterScreenHorizontal(size),Height- size.Y);
                case UIElementPositioEnEnum.TopLeftCorner:
                    return new Vector2(0,  size.Y);
                case UIElementPositioEnEnum.TopRightCorner:
                    return new Vector2(Width - size.X, 0); 
                case UIElementPositioEnEnum.CenterHorizontal:
                    return new Vector2(CenterScreenHorizontal(size), 0);
                case UIElementPositioEnEnum.CenterVertical:
                    return new Vector2(0, CenterScreenVertical(size));
                case UIElementPositioEnEnum.Center:
                    return CenterScreen(size);
                case UIElementPositioEnEnum.TopCenter:
                    return new Vector2(CenterScreenHorizontal(size),0);
            }
            return Vector2.Zero;
        }
        private static Vector2 CenterScreen(Vector2 size) => new Vector2(CenterScreenHorizontal(size), CenterScreenVertical(size));
        private static float CenterScreenHorizontal(Vector2 size) => HalfWidth - (size.X / 2);
        private static float CenterScreenVertical(Vector2 size) => HalfHeight - (size.Y / 2);
    }
}