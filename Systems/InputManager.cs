using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameClusterFuck.Settings;

namespace MonoGameClusterFuck.Systems
{
    public class InputManager
    {
        public KeyboardManager KManager = new KeyboardManager();
        public GamePadState GamePadState, LastGamePadState;
        public MouseState MouseState, LastMouseState;

        private void UpdateStates()
        {
            LastGamePadState = GamePadState;
            LastMouseState = MouseState;
            GamePadState = GamePad.GetState(PlayerIndex.One);
            MouseState = Mouse.GetState();
        }

        public void Update()
        {
            KManager.Update();
            UpdateStates();

            if (KManager.KeyPressed(Keys.Escape))
            {
                GlobalState.GraphicsSettings.Save();
                GlobalState.Game.Exit();
            }
            if (KManager.KeyPressed(Keys.H))
                GlobalState.DrawTileSet = !GlobalState.DrawTileSet;
        }
    }
}