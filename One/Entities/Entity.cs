using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Animations;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck.Entities
{
    public class Entity : Sprite
    {
        public WalkAnimations WalkAnimations;
        public Animation CurrentAnimation;
        public Vector2 Destination;
        public float Speed = 200;
        public uint UniqueId { get; set; }
        public DateTime DestinationReachedTimeStamp;

        public Entity(int size, float layerDepth) : base(size, layerDepth)
        {
        }

        internal static Entity Spawn(uint uniqueId, Vector2 position)
        {
            var entity = new Entity(32, 0.01f)
            {
                UniqueId = uniqueId,
                Position = position,
                Destination = position
            };
            entity.Initialize();
            entity.LoadContent();
            entity.Position = position;
            entity.Destination = position;
            Collections.Entities.TryAdd(entity.UniqueId, entity);

            ThreadedConsole.WriteLine("[Entity] Spawning new Entity#" + entity.UniqueId);
            return entity;
        }

        public override void LoadContent()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>("player_f");
            base.LoadContent();
        }

        public override void Initialize()
        {
            WalkAnimations = new WalkAnimations();
            CurrentAnimation = WalkAnimations.IdleDown;
            base.Initialize();
        }
        public override void Update(GameTime deltaTime)
        {
            if (State != SpriteState.Ready)
                return;
            UpdateMove(deltaTime);
            if(DestinationReachedTimeStamp.AddMilliseconds(250)<DateTime.UtcNow)
            CurrentAnimation=WalkAnimations.GetIdleAnimationFrom(CurrentAnimation);
            Source = CurrentAnimation.CurrentRectangle;
            CurrentAnimation.Update(deltaTime);
        }

        private void UpdateMove(GameTime deltaTime)
        {
            if (Position != Destination)
            {
                //ThreadedConsole.WriteLine("[ENTITY][UpdateMove] Pos: "+Position +" Dest: "+Destination);
                var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
                var distance = Vector2.Distance(Position, Destination);
                var direction = Vector2.Normalize(Destination - Position);
                var velocity = direction * Speed * delta;

                Position += velocity;
                if (Vector2.Distance(Position, Destination) >= distance)
                {
                    Position = Destination;
                    DestinationReachedTimeStamp = DateTime.UtcNow;
                }
                else
                    CurrentAnimation = WalkAnimations.GetWalkingAnimationFrom(direction);
            }
        }

        public void MoveTo(Vector2 location, bool teleport = false)
        {
            if (teleport)
                Position = Destination = location;
            else
                Destination = location;
        }
        public override void Draw()
        {
            if (State != SpriteState.Ready)
                return;

            base.Draw();
        }

        public void Destroy()
        {
            ThreadedConsole.WriteLine("[Entity] Destructor called. Killing Entity#" + UniqueId);
            State= SpriteState.Disposing;
            Collections.Entities.TryRemove(UniqueId, out _);
        }
    }
}
