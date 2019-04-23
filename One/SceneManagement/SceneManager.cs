using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameClusterFuck.SceneManagement.Scenes;

namespace MonoGameClusterFuck.SceneManagement
{
    public static class SceneManager
    {
        public static Dictionary<SceneEnum, Scene> Scenes = new Dictionary<SceneEnum, Scene>
        {
            [SceneEnum.Splash] = new Splash(),
            [SceneEnum.MainMenu] = new MainMenu(),
            [SceneEnum.InfiniteWorld] = new InfiniteWorld()
        };

        public static Scene CurrentScene;

        public static void Initialize()
        {
            CurrentScene.Initialize();
        }
        public static void LoadContent()
        {
            CurrentScene.LoadContent();
        }
        public static void Update(GameTime gameTime)
        {
            CurrentScene.Update(gameTime);
        }
        public static void DrawGame()
        {
            CurrentScene.DrawGame();
        }
        public static void DrawUI()
        {
            CurrentScene.DrawUI();
        }

        public static void SetState(SceneEnum scene)
        {
            Console.WriteLine("[SceneManager][SetState] -> " + scene);
            CurrentScene = Scenes[scene];
            Initialize();
            LoadContent();
        }
    }
}