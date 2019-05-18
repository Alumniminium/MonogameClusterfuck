using System.Threading.Tasks.Dataflow;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Systems;
using NoiseGen;
using One.Primitives;
using System;

namespace MonoGameClusterFuck.SceneManagement.Scenes
{
    public class InfiniteWorld : Scene
    {
        public TileSet TileSet;
        public static FastNoise NoiseGen = new FastNoise();


        ConvexHull[] objects;
        LightSource[] lights;
        RenderTarget2D lightMap;
        Texture2D alphaClearTexture;

        public InfiniteWorld()
        {
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Constructor called!");
        }

        public override void Initialize()
        {
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Initializing components...");
            TileSet = new TileSet(32);
            TileSet.Slice();
            Entities.Add(new Player(32, 0.01f));
            Entities.Add(new Cursor(32));
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Initialization handed over to base class.");
            base.Initialize();
        }

        public override void LoadContent()
        {
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Loading content...");
                        BuildObjectList();
             Texture2D lightTexture = Engine.Instance.Content.Load<Texture2D>("light");
            lights = new LightSource[5];
            lights[0] = new LightSource(lightTexture, Color.Crimson, 250, new Vector2(40, 400));
            lights[1] = new LightSource(lightTexture, Color.CornflowerBlue, 250, new Vector2(40, 200));
            lights[2] = new LightSource(lightTexture, Color.Gold, 200, Vector2.Zero);
            lights[3] = new LightSource(lightTexture, Color.Red, 150, new Vector2(510, 30));
            lights[4] = new LightSource(lightTexture, Color.ForestGreen, 150, new Vector2(50, 540));


            PresentationParameters pp = Engine.Instance.GraphicsDevice.PresentationParameters;
            lightMap = new RenderTarget2D(Engine.Instance.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, true, SurfaceFormat.Color,DepthFormat.None);
            alphaClearTexture = Engine.Instance.Content.Load<Texture2D>("AlphaOne");
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Loading handed over to base class.");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Loaded)
                return;

            
            double time = gameTime.TotalGameTime.TotalSeconds / 4.0f;
            lights[3].Position = new Vector2(700,300 + (float)Math.Sin(time) * 200);

            base.Update(gameTime);
        }

        public override void DrawUI()
        {
            if (!Loaded)
                return;
            FpsCounter.Draw();
            base.DrawUI();
        }
        enum TileType
        {
            Ground,
            Wall
        }
        struct TileInfo
        {
            public TileType Type;
            public TileInfo((float, float) location)
            {
                var value = NoiseGen.GetCubic(location.Item1, location.Item2);
                if (value > 0.10f)
                    Type = TileType.Wall;
                else
                    Type = TileType.Ground;
            }
        }
        public override void DrawGame()
        {
            if (!Loaded)
                return;

            base.DrawGame();
                       
            DrawLightmap();

            var destRect = new Rectangle(Point.Zero, new Point(TileSet.TileSize));
            var viewbounds = Camera.VisibleArea;
            var left = ((viewbounds.Left / TileSet.TileSize) * TileSet.TileSize) - TileSet.TileSize;
            var top = ((viewbounds.Top / TileSet.TileSize) * TileSet.TileSize) - TileSet.TileSize;

            if (InputState.DrawTileSet)
            {
                int count = 0;
                int yoffset = 0;
                int xoffset = 0;
                destRect.Width = 64;
                destRect.Height = 64;
                foreach (var tile in TileSet.Tiles)
                {
                    if (TileSet.Tiles.Count == count)
                        count = 0;
                    destRect.Location = new Point(xoffset * 64, yoffset * 128);
                    var stringDest = new Vector2(destRect.Location.X, destRect.Y - 32);
                    Engine.SpriteBatch.DrawString(Fonts.ProFont, count.ToString(), stringDest, Color.Black, 0, Vector2.One, 1, SpriteEffects.None, 1.0f);
                    Engine.SpriteBatch.Draw(TileSet.Atlas, destRect, TileSet.Tiles[count].Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1.0f);
                    count++;
                    xoffset++;
                    if (xoffset == 12)
                    {
                        xoffset = 0;
                        yoffset++;
                    }
                }
                base.DrawGame();
                return;
            }

            var floorTile = TileSet.Tiles[69];
            var wallTile = TileSet.Tiles[144];
            var upperWallTile = TileSet.Tiles[143];
            var wallTileLeft = TileSet.Tiles[176];
            var wallTileRight = TileSet.Tiles[112];
            var upperWallTileRight = TileSet.Tiles[110];
            var upperWallTileLeft = TileSet.Tiles[175];
            
                    Sprite sprite = floorTile;
            for (var x = left; x <= viewbounds.Right; x += TileSet.TileSize)
            {
                for (var y = top; y <= viewbounds.Bottom; y += TileSet.TileSize)
                {
                        destRect.Location = new Point(x, y);
                         SpriteBatch.Draw(sprite.Texture, destRect, sprite.Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
                }
            }

            for (var x = left; x <= viewbounds.Right; x += TileSet.TileSize)
            {
                for (var y = top; y <= viewbounds.Bottom; y += TileSet.TileSize)
                {
                    var value = new TileInfo((x, y));
                    var a = new TileInfo((x, y - 32));
                    var b = new TileInfo((x, y + 32));

                    if(value.Type==TileType.Wall)
                    {
                        sprite=wallTile;
                        if (b.Type == TileType.Ground)
                            sprite = wallTile;
                        if(b.Type== TileType.Wall)
                            sprite=upperWallTile;

                        destRect.Location = new Point(x, y);
                        SpriteBatch.Draw(sprite.Texture, destRect, sprite.Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.99f);
                    }
                }
            }

            foreach (ConvexHull hull in objects)
            {
                hull.Draw();
            }

            //multiply scene with lightmap
            //Engine.SpriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend);
            Engine.SpriteBatch.Draw(lightMap, Vector2.Zero, Color.White);
            //Engine.SpriteBatch.End();
        }
        
        private void ClearAlphaToOne()
        {
            Engine.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Engine.SpriteBatch.Draw(alphaClearTexture, new Rectangle(0, 0, Engine.Instance.GraphicsDevice.Viewport.Width, Engine.Instance.GraphicsDevice.Viewport.Height), Color.White);
            Engine.SpriteBatch.End();
        }
         private void DrawLightmap()
        {
            Engine.Instance.GraphicsDevice.SetRenderTarget(lightMap);

            //clear to some small ambient light
            Engine.Instance.GraphicsDevice.Clear(new Color(35,35,35));


            foreach (LightSource light in lights)
            {
                //draw all shadows
                //write only to the alpha channel, which sets alpha to 0
                
                foreach (ConvexHull ch in objects)
                {
                    //draw shadow
                    ch.DrawShadows(light);
                }
                
                
                //draw the light shape
                //where Alpha is 0, nothing will be written
                //Engine.SpriteBatch.Begin(SpriteSortMode.Immediate);
                light.Draw(Engine.SpriteBatch);
                //Engine.SpriteBatch.End();
                
            }
            //clear alpha, to avoid messing stuff up later
            //ClearAlphaToOne();
            Engine.Instance.GraphicsDevice.SetRenderTarget(null);
        }
        private void BuildObjectList()
        {

            ConvexHull.InitializeStaticMembers(Engine.Instance.GraphicsDevice);
            objects = new ConvexHull[37];
            Color wallColor = Color.Black;

            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 20);
            points[2] = new Vector2(800, 20);
            points[3] = new Vector2(800, 0);


            objects[0] = new ConvexHull(points, wallColor, Vector2.Zero);
            objects[1] = new ConvexHull(points, wallColor,new Vector2(0,580));
            
            points[0] = new Vector2(0, 20);
            points[1] = new Vector2(0, 580);
            points[2] = new Vector2(20, 580);
            points[3] = new Vector2(20, 20);


            objects[2] = new ConvexHull(points, wallColor,Vector2.Zero);
            objects[3] = new ConvexHull(points, wallColor,new Vector2(780, 0));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 20);
            points[2] = new Vector2(560, 20);
            points[3] = new Vector2(560, 0);


            objects[4] = new ConvexHull(points, wallColor,new Vector2(20,140));
            objects[5] = new ConvexHull(points, wallColor,new Vector2(20,440));

            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 140);
            points[2] = new Vector2(20, 140);
            points[3] = new Vector2(20, 0);


            objects[6] = new ConvexHull(points, wallColor, new Vector2(580, 140));
            objects[7] = new ConvexHull(points, wallColor, new Vector2(580, 320));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 20);
            points[2] = new Vector2(20, 20);
            points[3] = new Vector2(20, 0);


            objects[8] = new ConvexHull(points, wallColor, new Vector2(640, 140));
            objects[9] = new ConvexHull(points, wallColor, new Vector2(640, 240));
            objects[10] = new ConvexHull(points, wallColor, new Vector2(640, 340));
            objects[11] = new ConvexHull(points, wallColor, new Vector2(640, 440));

            objects[12] = new ConvexHull(points, wallColor, new Vector2(720, 140));
            objects[13] = new ConvexHull(points, wallColor, new Vector2(720, 240));
            objects[14] = new ConvexHull(points, wallColor, new Vector2(720, 340));
            objects[15] = new ConvexHull(points, wallColor, new Vector2(720, 440));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 80);
            points[2] = new Vector2(20, 80);
            points[3] = new Vector2(20, 0);


            objects[16] = new ConvexHull(points, wallColor, new Vector2(100, 500));
            
            objects[17] = new ConvexHull(points, wallColor, new Vector2(200, 460));

            objects[18] = new ConvexHull(points, wallColor, new Vector2(300, 500));
            objects[19] = new ConvexHull(points, wallColor, new Vector2(400, 460));

            objects[20] = new ConvexHull(points, wallColor, new Vector2(500, 500));
            objects[21] = new ConvexHull(points, wallColor, new Vector2(580, 460));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 40);
            points[2] = new Vector2(60, 40);
            points[3] = new Vector2(60, 0);


            objects[22] = new ConvexHull(points, wallColor, new Vector2(160, 280));
            objects[23] = new ConvexHull(points, wallColor, new Vector2(360, 280));

            points[0] = new Vector2(0, -20);
            points[1] = new Vector2(-20, 0);
            points[2] = new Vector2(0, 20);
            points[3] = new Vector2(20, 0);

            objects[24] = new ConvexHull(points, wallColor, new Vector2(460, 80));
            objects[25] = new ConvexHull(points, wallColor, new Vector2(560, 80));
            objects[26] = new ConvexHull(points, wallColor, new Vector2(160, 80));

            points = new Vector2[8];
            float angleSlice = MathHelper.TwoPi / 8.0f;

            for (int i = 0; i < 8; i++)
            {
                points[i] = new Vector2((float)Math.Sin(angleSlice * i), (float)Math.Cos(angleSlice * i)) * 20;
            }

            objects[27] = new ConvexHull(points, wallColor, new Vector2(140, 220));
            objects[28] = new ConvexHull(points, wallColor, new Vector2(240, 220));
            objects[29] = new ConvexHull(points, wallColor, new Vector2(340, 220));
            objects[30] = new ConvexHull(points, wallColor, new Vector2(440, 220));

            objects[31] = new ConvexHull(points, wallColor, new Vector2(140, 380));
            objects[32] = new ConvexHull(points, wallColor, new Vector2(240, 380));
            objects[33] = new ConvexHull(points, wallColor, new Vector2(340, 380));
            objects[34] = new ConvexHull(points, wallColor, new Vector2(440, 380));

            objects[35] = new ConvexHull(points, wallColor, new Vector2(80, 300));
            objects[36] = new ConvexHull(points, wallColor, new Vector2(500, 300));
        }
    }
}