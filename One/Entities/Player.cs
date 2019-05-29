using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Animations;
using MonoGameClusterFuck.Networking;
using MonoGameClusterFuck.Networking.Packets;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Systems;
using MonoGameClusterFuck.UI.Controls;

namespace MonoGameClusterFuck.Entities
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
        public Vector2 Destination;
        public WalkAnimations WalkAnimations;
        public Animation CurrentAnimation;
        public TextBlock TextBlock;
        public Vector2 LastDirection;

        public Player(int size, float layerDepth) : base(size, layerDepth)
        {
            ThreadedConsole.WriteLine("[Player] Constructor called!");
            TextBlock = new TextBlock();
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
            Position = new Vector2(16 + (32 * 10000), 32 * 10000);
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
            var velocity = InputManager.Keyboard.GetInputAxisConstrained();

            if ((velocity.X != 0 || velocity.Y != 0) && Position == Destination)
                Destination += (velocity * 32);

            TextBlock.Position.X = Position.X - TextBlock.Width / 2;
            TextBlock.Position.Y = Position.Y - 32;
            TextBlock.Update(deltaTime);
            UpdateMove(deltaTime);
            Source = CurrentAnimation.CurrentRectangle;
            CurrentAnimation.Update(deltaTime);

            Camera.Update(deltaTime);
        }

        private void UpdateMove(GameTime deltaTime)
        {
                var keyboardAxis = InputManager.Keyboard.GetInputAxisConstrained();

            if (Position != Destination || keyboardAxis != Vector2.Zero)
            {
                ThreadedConsole.WriteLine("[ENTITY][UpdateMove] Pos: "+Position +" Dest: "+Destination);
                var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
                var distance = Vector2.Distance(Position, Destination);
                var direction = Vector2.Normalize(Destination - Position);
                var velocity = direction * Speed * delta;
                LastDirection= direction;
                Position += velocity;
                if (Vector2.Distance(Position, Destination) > distance)
                {
                    Position = Destination;

                    if ((keyboardAxis != Vector2.Zero) && Position == Destination)
                    {
                        Destination += (keyboardAxis * 32);
                        direction = Vector2.Normalize(Destination - Position);
                        velocity = direction * Speed * delta;
                        Position += velocity;
                        
                    }
                    else
                    {
                        CurrentAnimation= WalkAnimations.GetIdleAnimationFrom(CurrentAnimation);
                    }
                }
                else
                {
                    CurrentAnimation = WalkAnimations.GetWalkingAnimationFrom(direction);
                }
            }
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