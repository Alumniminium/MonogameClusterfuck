using System.Collections.Specialized;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using One.Animations;
using One.Networking;
using One.Networking.Packets;
using One.Primitives;
using One.Systems;
using One.UI.Controls;
using One.SceneManagement.Scenes;

namespace One.Entities
{
    public class Player : Sprite
    {
        public uint UniqueId;
        public NetworkClient Socket;
        public Camera Camera;
        public float Speed = 200;
        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                Camera.Position = base.Position = value;
                ThreadedConsole.WriteLine($"[Player][Position] X={value.X} Y={value.Y}");

                if (Socket == null || value == Socket.ServerPosition)
                    return;

                if (Socket.LastUpdateTick + 50 < Environment.TickCount && value != Socket.ServerPosition)
                {
                    Socket.Send(MsgWalk.Create(UniqueId, Position));
                    Socket.LastUpdateTick = Environment.TickCount;
                    Socket.ServerPosition = Position;
                }
            }
        }

        public Vector2 Direction;
        public Vector2 Destination;
        public WalkAnimations WalkAnimations;
        public Animation CurrentAnimation;
        public TextBlock TextBlock;

        public Player(int size, float layerDepth) : base(size, layerDepth)
        {
            ThreadedConsole.WriteLine("[Player] Constructor called!");
            TextBlock = new TextBlock();
            Initialize();
            LoadContent();
        }
        public override void Initialize()
        {
            ThreadedConsole.WriteLine("[Player] Initializing components...");
            WalkAnimations = new WalkAnimations();
            Camera = new Camera();
            TextBlock.Initialize();
            Socket = new NetworkClient(this);
            ThreadedConsole.WriteLine("[Player] Initialization handed over to base class...");
            base.Initialize();
        }

        public override void LoadContent()
        {
            ThreadedConsole.WriteLine("[Player] Loading Content...");
            Texture = Engine.Instance.Content.Load<Texture2D>("player_f");
            ThreadedConsole.WriteLine("[Player] Loading handed over to base class...");
            base.LoadContent();
        }
        public override void Start()
        {
            ThreadedConsole.WriteLine("[Player] Startup Sequence activated...");
            Socket.ConnectAsync("127.0.0.1", 13338);
            Socket.Send(MsgLogin.Create("Test", "123"));
            Position = new Vector2(16 + (32 * 200), 32 * 200 + 32);
            Destination = Position;
            Camera.Position = Position;
            CurrentAnimation = WalkAnimations.IdleDown;
            base.Start();
        }

        public override void Update(GameTime deltaTime)
        {
            if (State != SpriteState.Ready)
                return;

            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            var keyboardAxis = InputManager.Keyboard.GetInputAxisConstrained();

            if (keyboardAxis != Vector2.Zero && Position == Destination)
            {
                var destinationTest = Destination + (keyboardAxis * 32);
                //if (IsWalkable(destinationTest))
                //{
                Destination = destinationTest;
                Direction = Vector2.Normalize(destinationTest - Position);
                //}
            }

            if (Position != Destination)
            {
                UpdateMove(delta);
                CurrentAnimation = WalkAnimations.GetWalkingAnimationFrom(Direction);
            }
            else
                CurrentAnimation = WalkAnimations.GetIdleAnimationFrom(Direction);

            //TextBlock.Position.X = Position.X - TextBlock.Width / 2f;
            //TextBlock.Position.Y = Position.Y - 32;
            //TextBlock.Update(deltaTime);

            Source = CurrentAnimation.CurrentRectangle;
            CurrentAnimation.Update(deltaTime);

            Camera.Update(deltaTime);
        }


        private void UpdateMove(float delta)
        {
            var velocity = Direction * Speed * delta;
            var distance = Vector2.Distance(Position, Destination);
            var distanceAfterMove = Vector2.Distance(Position + velocity, Destination);

            if (distanceAfterMove > distance)
            {
                Position = Destination;

                var destinationTest = Destination + velocity;
                if (velocity != Vector2.Zero && IsWalkable(destinationTest))
                {
                    Destination = destinationTest;
                    Direction = Vector2.Normalize(Destination - Position);
                }
            }
            if (Position != Destination)
                Position += velocity;
        }
        private static bool IsWalkable(Vector2 destinationTest)
        {
            return InfiniteWorld.NoiseGen.GetCubic(destinationTest.X - 16, destinationTest.Y) < 0.10f;
        }

        public override void Draw()
        {
            if (State != SpriteState.Ready)
                return;
            TextBlock.Draw();
            base.Draw();
        }
    }
}