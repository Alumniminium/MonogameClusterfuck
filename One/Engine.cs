using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Layers;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Settings;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck
{
    public class Scene
    {
        public static Engine Instance;
        public static FpsCounter FpsCounter;
        public static SpriteBatch SpriteBatch;
        public static GraphicsDeviceManager Graphics;
        public static TileSet TileSet;
        public static FastNoise NoiseGen = new FastNoise();
        public Scene()
        {

        }

        protected void Initialize()
        {
            FpsCounter = new FpsCounter();
            GameMap.Layers[LayerType.Cursor].Add(new Cursor(32));
            GameMap.Layers[LayerType.Entity].Add(new Player(32));
            TileSet = new TileSet(32);
            TileSet.Slice();
            ApplicationStateManager.Initialize();
        }

        protected void LoadContent()
        {
            Fonts.LoadContent();
            ApplicationStateManager.LoadContent();
        }

        protected void Update(GameTime gameTime)
        {
            InputManager.Update();
            ApplicationStateManager.Update(gameTime);
        }
        protected void Draw(GameTime gameTime)
        {
            DrawGame();
            DrawUI();
        }

        private void DrawUI()
        {
            SpriteBatch.Begin();
            ApplicationStateManager.DrawUI();
            FpsCounter.Draw();
            SpriteBatch.End();
        }
        private void DrawGame()
        {
            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Camera.Transform);
            ApplicationStateManager.DrawGame();
            var SpriteSize = TileSet.TileSize;
            var destRect = new Rectangle(Point.Zero, new Point(SpriteSize));
            var viewbounds = Camera.VisibleArea;

            var left = (viewbounds.Left / SpriteSize * SpriteSize) - SpriteSize;
            var top = (viewbounds.Top / SpriteSize * SpriteSize) - SpriteSize;
            var FloorTile = TileSet.Tiles[357];
            var WallTile = TileSet.Tiles[5];
            for (var x = left; x <= viewbounds.Right; x += SpriteSize)
            {
                for (var y = top; y <= viewbounds.Bottom; y += SpriteSize)
                {
                    var value = NoiseGen.GetCellular(x, y);

                    if (value > 0.45f)
                    {
                        destRect.Location = new Point(x, y);
                        SpriteBatch.Draw(FloorTile.Texture, destRect, FloorTile.Source, Color.White);
                    }
                    else
                    {
                        destRect.Location = new Point(x, y);
                        SpriteBatch.Draw(WallTile.Texture, destRect, WallTile.Source, Color.White);
                    }
                }
            }
            SpriteBatch.End();
        }
    }
    public class ApplicationState
    {
        public List<Sprite> Entities = new List<Sprite>();
        public List<UIElement> UIElements = new List<UIElement>();
    }
    public static class ApplicationStateManager
    {
        public static Dictionary<int, ApplicationState> States = new Dictionary<int, ApplicationState>
        {
            [0] = new ApplicationState
            {
                Entities =
                {
                    new Cursor(32),
                    new Player(32)
                },
                UIElements =
                {

                }
            }
        };
        private static ApplicationState _currentApplicationState;
        public static void Initialize()
        {
            foreach(var component in _currentApplicationState.Entities)
            {
                component.Initialize();
            }
        }
        public static void LoadContent()
        {
            foreach(var component in _currentApplicationState.Entities)
            {
                component.LoadContent();
            }
            foreach (var element in _currentApplicationState.UIElements)
            {
                element.LoadContent();
            }
        }
        public static void Update(GameTime gameTime)
        {
            foreach (var component in _currentApplicationState.Entities)
            {
                component.Update(gameTime);
            }
            foreach (var element in _currentApplicationState.UIElements)
            {
                element.Update(gameTime);
            }
        }
        public static void DrawGame()
        {
            foreach(var component in _currentApplicationState.Entities)
            {
                component.Draw(LayerType.Entity);
            }
            foreach (var element in _currentApplicationState.UIElements)
            {
                element.Draw();
            }
        }

        public static void SetState(int index)
        {
            if(States.ContainsKey(index))
            _currentApplicationState = States[index];
            else
                throw new IndexOutOfRangeException($"State does not exist ({index})");
        }
        public static void Switch(ApplicationState newApplicationState)
        {
            if (_currentApplicationState != null)
            {
                //foreach (var currentStateEntity in _currentApplicationState.Entities)
                //    currentStateEntity.Destroy();
                foreach (var element in _currentApplicationState.UIElements)
                    element.Destroy();
            }

            Initialize();
            LoadContent();
        }

        public static void DrawUI()
        {

        }
    }
    public class Engine : Game
    {
        public static Engine Instance;
        public static FpsCounter FpsCounter;
        public static SpriteBatch SpriteBatch;
        public static GraphicsDeviceManager Graphics;
        public static TileSet TileSet;
        public static FastNoise NoiseGen = new FastNoise();
        public Engine()
        {
            IsFixedTimeStep = false;
            Graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = GraphicsSettings.Instance.VSync,
                PreferredBackBufferHeight = GraphicsSettings.Instance.Height,
                PreferredBackBufferWidth = GraphicsSettings.Instance.Width,
                IsFullScreen = GraphicsSettings.Instance.Fullscreen,
            };
            Graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
            ApplicationStateManager.SetState(0);
        }

        protected override void Initialize()
        {
            FpsCounter=new FpsCounter();
            GameMap.Layers[LayerType.Cursor].Add(new Cursor(32));
            GameMap.Layers[LayerType.Entity].Add(new Player(32));
            TileSet = new TileSet(32);
            TileSet.Slice();
            ApplicationStateManager.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Fonts.LoadContent();
            ApplicationStateManager.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            ApplicationStateManager.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawGame();
            DrawUI();

            base.Draw(gameTime);
        }

        private void DrawUI()
        {
            SpriteBatch.Begin();
            ApplicationStateManager.DrawUI();
            FpsCounter.Draw();
            SpriteBatch.End();
        }
        private void DrawGame()
        {
            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Camera.Transform);
            ApplicationStateManager.DrawGame();
            var SpriteSize = TileSet.TileSize;
            var destRect = new Rectangle(Point.Zero, new Point(SpriteSize));
            var viewbounds = Camera.VisibleArea;

            var left = (viewbounds.Left / SpriteSize * SpriteSize) - SpriteSize;
            var top = (viewbounds.Top / SpriteSize * SpriteSize) - SpriteSize;
            var FloorTile = TileSet.Tiles[357];
            var WallTile = TileSet.Tiles[5];
            for (var x = left; x <= viewbounds.Right; x += SpriteSize)
            {
                for (var y = top; y <= viewbounds.Bottom; y += SpriteSize)
                {
                    var value = NoiseGen.GetCellular(x, y);

                    if (value > 0.45f)
                    {
                        destRect.Location = new Point(x, y);
                        SpriteBatch.Draw(FloorTile.Texture, destRect, FloorTile.Source, Color.White);
                    }
                    else
                    {
                        destRect.Location = new Point(x, y);
                        SpriteBatch.Draw(WallTile.Texture, destRect, WallTile.Source, Color.White);
                    }
                }
            }
            SpriteBatch.End();
        }
    }
}
