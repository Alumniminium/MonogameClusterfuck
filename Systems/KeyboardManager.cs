using Microsoft.Xna.Framework.Input;

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
        public bool KeyPressed(Keys key)
        {
            return KeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key);
        } 
        public bool KeyDown(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        } 
        public bool KeyUp(Keys key)
        {
            return KeyboardState.IsKeyUp(key);
        } 

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
    }
}