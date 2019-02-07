using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameClusterFuck.Systems
{
    public class InputManager
    {
        public KeyboardState KeyboardState, LastKeyboardState;
        public GamePadState GamePadState, LastGamePadState;
        public MouseState MouseState, LastMouseState;

        private void UpdateStates()
        {
            LastKeyboardState = KeyboardState;
            LastGamePadState = GamePadState;
            LastMouseState = MouseState;
            KeyboardState = Keyboard.GetState();
            GamePadState = GamePad.GetState(PlayerIndex.One);
            MouseState = Mouse.GetState();
        }

        public void Update()
        {
            UpdateStates();

            if (KeyPressed(Keys.Escape))
                Game.Instance.Exit();

            if (KeyPressed(Keys.H))
                GlobalState.DrawTileSet = !GlobalState.DrawTileSet;
        }

        public bool KeyPressed(Keys key)
        {
            return KeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key);
        }        
    }
}