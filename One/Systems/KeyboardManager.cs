using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameClusterFuck.Settings;

namespace MonoGameClusterFuck.Systems
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
        public Vector2 GetVelocity(float speed)
        {
            var keyboard = Engine.InputManager.KManager;
            var velocity = Vector2.Zero;
            if (keyboard.KeyDown(PlayerControls.Up))
            {
                velocity.Y = -speed;
            }
            if (keyboard.KeyDown(PlayerControls.Down))
            {
                velocity.Y = speed;
            }
            if (keyboard.KeyDown(PlayerControls.Left))
            {
                velocity.X = -speed;
            }
            if (keyboard.KeyDown(PlayerControls.Right))
            {
                velocity.X = speed;
            }
            if (keyboard.KeyDown(PlayerControls.Sprint))
            {
                velocity.X *= 20;
                velocity.Y *= 20;
            }

            if (Math.Abs(Math.Abs(velocity.Y) - speed) < 1 && Math.Abs(Math.Abs(velocity.X) - speed) < 1)
                velocity /= 1.55f;

            return velocity;
        }
    }
}