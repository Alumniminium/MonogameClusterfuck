using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using One.Settings;

namespace One.Systems
{
    public class KeyboardManager
    { 
        public KeyboardState KeyboardState, LastKeyboardState;

        public void Update()
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
        }
        public bool KeyPressed(Keys key) => KeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key);
        public bool KeyDown(Keys key) => KeyboardState.IsKeyDown(key);
        public bool KeyUp(Keys key) => KeyboardState.IsKeyUp(key);

        public bool KeyPressed(params Keys[] keys)
        {
            var result = true;
            foreach(var key in keys)
            {
                if(KeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key))
                {
                    continue;
                }
                result = false;
            }
            return result;
        } 
        public bool KeyDown(params Keys[] keys)
        {
            var result = true;
            foreach(var key in keys)
            {
                if(KeyboardState.IsKeyDown(key))
                {
                    continue;
                }
                result = false;
            }
            return result;
        } 
        public bool KeyUp(params Keys[] keys)
        {
            var result = true;
            foreach(var key in keys)
            {
                if(LastKeyboardState.IsKeyUp(key))
                {
                    continue;
                }
                result = false;
            }
            return result;
        } 
        public Vector2 GetInputAxis()
        {
            var keyboard = InputManager.Keyboard;
            var velocity = Vector2.Zero;
            if (keyboard.KeyDown(PlayerControls.Up))
            {
                velocity.Y = -1;
            }
            if (keyboard.KeyDown(PlayerControls.Down))
            {
                velocity.Y = 1;
            }
            if (keyboard.KeyDown(PlayerControls.Left))
            {
                velocity.X = -1;
            }
            if (keyboard.KeyDown(PlayerControls.Right))
            {
                velocity.X = 1;
            }
            if (keyboard.KeyDown(PlayerControls.Sprint))
            {
                velocity.X *= 20;
                velocity.Y *= 20;
            }

            if (Math.Abs(Math.Abs(velocity.Y) - 1) < 1 && Math.Abs(Math.Abs(velocity.X) - 1) < 1)
                velocity /= 1.55f;

            return velocity;
        }
        public Vector2 GetInputAxisConstrained()
        {
            var keyboard = InputManager.Keyboard;
            var velocity = Vector2.Zero;
            if (keyboard.KeyDown(PlayerControls.Up))
            {
                velocity.Y = -1;
            }
            else if (keyboard.KeyDown(PlayerControls.Down))
            {
                velocity.Y = 1;
            }
            else if (keyboard.KeyDown(PlayerControls.Left))
            {
                velocity.X = -1;
            }
            else if (keyboard.KeyDown(PlayerControls.Right))
            {
                velocity.X = 1;
            }
            else if (keyboard.KeyDown(PlayerControls.Sprint))
            {
                velocity.X *= 20;
                velocity.Y *= 20;
            }

            if (Math.Abs(Math.Abs(velocity.Y) - 1) < 1 && Math.Abs(Math.Abs(velocity.X) - 1) < 1)
                velocity /= 1.55f;

            return velocity;
        }
    }
}