using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Settings;

namespace MonoGameClusterFuck.Systems
{
    public static class InputManager
    {
        public static KeyboardManager Keyboard = new KeyboardManager();
        public static GamePadState GamePadState, LastGamePadState;
        public static MouseState MouseState, LastMouseState;

        private static void UpdateStates()
        {
            LastGamePadState = GamePadState;
            LastMouseState = MouseState;
            GamePadState = GamePad.GetState(PlayerIndex.One);
            MouseState = Mouse.GetState();
        }

        public static void Update()
        {
            Keyboard.Update();
            UpdateStates();

            if (Keyboard.KeyPressed(Keys.Escape))
            {
                GraphicsSettings.Instance.Save();
                Environment.Exit(0);
            }
            if (Keyboard.KeyPressed(Keys.H))
                InputState.DrawTileSet = !InputState.DrawTileSet;
                if(Keyboard.KeyPressed(Keys.C))
                {
                    var cursorPos = InputManager.MouseState.Position;
                    Entity.Spawn(0,cursorPos.ToVector2());
                }
        }
    }
}