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
    public class State
    {
        public List<Entity> Entities = new List<Entity>();
        public List<UIElement> UIElements = new List<UIElement>();

        
    }
    public class StateManager
    {
        State currentState;
        public void Initialize()
        {
            foreach(var component in currentState.Entities)
            {
                component.Initialize();
            }
        }
        public void LoadContent()
        {
            foreach(var component in currentState.Entities)
            {
                component.LoadContent();
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach(var component in currentState.Entities)
            {
                component.Update(gameTime);
            }
        }
        public void Draw()
        {
            foreach(var component in currentState.Entities)
            {
                component.Draw(LayerType.Entity);
            }
        }

        public void Switch()
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
        }

        protected override void Initialize()
        {
            FpsCounter=new FpsCounter();
            GameMap.Layers[LayerType.Cursor].Add(new Cursor(32));
            GameMap.Layers[LayerType.Entity].Add(new Player(32));
            TileSet = new TileSet(32);
            TileSet.Slice();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Fonts.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            foreach (var layer in GameMap.Layers)
                layer.Value.Update(gameTime);
            
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
            foreach (var layer in GameMap.Layers.Where(k => k.Key == LayerType.UI))
                layer.Value.Draw();
            FpsCounter.Draw();
            SpriteBatch.End();
        }
        private void DrawGame()
        {
            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Camera.Transform);

            foreach (var layer in GameMap.Layers.Where(k => k.Key != LayerType.UI))
                layer.Value.Draw();

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
