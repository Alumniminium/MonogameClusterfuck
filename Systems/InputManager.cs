using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace monogame.Systems
{
    public class InputManager
    {
        public KeyboardState KeyboardState;
        public GamePadState GamePadState;

        public void Update()
        {
            var lastState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game.Instance.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.H) && lastState.IsKeyUp(Keys.H))
            {
                GlobalState.DrawTileSet = !GlobalState.DrawTileSet;
            }

        }
    }
}