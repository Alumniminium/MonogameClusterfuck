using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck.SceneManagement.Scenes
{
    public class Scene
    {
        public DateTime SceneActivatedTime;
        public Engine Instance;
        public SpriteBatch SpriteBatch;
        public GraphicsDeviceManager Graphics;

        public static FpsCounter FpsCounter;

        public List<Sprite> Entities = new List<Sprite>();
        public List<UIElement> UIElements = new List<UIElement>();

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
        }

        public virtual void Update(GameTime gameTime)
        {
            InputManager.Update();

            foreach (var entity in Entities)
                entity.Update(gameTime);
            foreach (var element in UIElements)
                element.Update(gameTime);
        }
        public void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Camera.Transform);
            DrawGame();
            SpriteBatch.End();
            SpriteBatch.Begin();
            DrawUI();
            SpriteBatch.End();
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