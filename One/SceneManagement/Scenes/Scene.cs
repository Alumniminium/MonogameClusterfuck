using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck.SceneManagement.Scenes
{
    public class Scene
    {
        private bool _loaded;
        public bool Loaded
        {
            get => _loaded;
            set{
                _loaded=value;
                ThreadedConsole.WriteLine("Scene Loaded: "+value);
            }
        }
        public DateTime SceneActivatedTime;
        public static Engine Instance;
        public static SpriteBatch SpriteBatch;
        public static GraphicsDeviceManager Graphics;

        public static FpsCounter FpsCounter;

        public BlockingCollection<Sprite> Entities = new BlockingCollection<Sprite>();
        public BlockingCollection<UIElement> UIElements = new BlockingCollection<UIElement>();

        public Scene()
        {
            SceneActivatedTime = DateTime.UtcNow;
            Instance = Engine.Instance;
            SpriteBatch = Engine.SpriteBatch;
            Graphics = Engine.Graphics;
        }

        public virtual void Initialize()
        {
            FpsCounter = new FpsCounter();

            foreach (var element in UIElements)
                element.Initialize();
            foreach (var entity in Entities)
                entity.Initialize();
        }

        public virtual void LoadContent()
        {
            Fonts.LoadContent();

            foreach (var entity in Entities)
                entity.LoadContent();
            foreach (var element in UIElements)
                element.LoadContent();
                Loaded=true;
        }

        public virtual void Update(GameTime gameTime)
        {
            InputManager.Update();

            foreach (var entity in Entities)
                entity.Update(gameTime);
            foreach (var element in UIElements)
                element.Update(gameTime);
        }

        public virtual void DrawUI()
        {
            foreach (var element in UIElements)
                element.Draw();
        }
        public virtual void DrawGame()
        {
            foreach (var entity in Entities)
                entity.Draw();
        }
    }
}