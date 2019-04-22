using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck.Scenes
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
            //GameMap.Layers[LayerType.Cursor].Add(new Cursor(32));
        }

        public virtual void LoadContent()
        {
            Fonts.LoadContent();
        }

        public virtual void Update(GameTime gameTime)
        {
            InputManager.Update();
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
        }
        public virtual void DrawGame()
        {

        }
    }
}