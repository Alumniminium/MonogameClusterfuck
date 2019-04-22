using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameClusterFuck.Scenes
{
    public static class SceneManager
    {
        public static Dictionary<int, Scene> Scenes = new Dictionary<int, Scene>
        {
            [0] = new Splash(),
            [1] = new MainMenu(),
            [2] = new InfiniteWorld()
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

        public static void SetState(int index)
        {
            if (Scenes.ContainsKey(index))
            {
                CurrentScene = Scenes[index];
                Initialize();
                LoadContent();
            }
            else
                throw new IndexOutOfRangeException($"State does not exist ({index})");
        }
        public static void Switch(Scene newScene)
        {
            if (CurrentScene != null)
            {
                //foreach (var currentStateEntity in _currentScene.Entities)
                //    currentStateEntity.Destroy();
                foreach (var element in CurrentScene.UIElements)
                    element.Destroy();
            }

            CurrentScene = newScene;
            Initialize();
            LoadContent();
        }

    }
}