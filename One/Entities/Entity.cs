﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Animations;
using MonoGameClusterFuck.Layers;
using MonoGameClusterFuck.Primitives;

namespace MonoGameClusterFuck.Entities
{
    public class Entity : Sprite
    {
        public WalkAnimations WalkAnimations;
        public Animation CurrentAnimation;
        public Vector2 Destination;
        public float Speed = 200;
        public uint UniqueId { get; set; }


        public Entity(int size) : base(size)
        {
        }
        
        internal static Entity Spawn(uint uniqueId, Vector2 position)
        {
            var entity = new Entity(32);
            entity.UniqueId = uniqueId;
            entity.Position = position;
            entity.Initialize();
            entity.LoadContent();
            GameMap.Layers[LayerType.Entity].Add(entity);
            Collections.Entities.TryAdd(entity.UniqueId, entity);
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
            UpdateMove(deltaTime);
            CurrentAnimation.Update(deltaTime);
            Source = CurrentAnimation.CurrentRectangle;
        }
        private void UpdateMove(GameTime deltaTime)
        {
            if (Position != Destination)
            {
                var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
                var distance = Vector2.Distance(Position, Destination);
                var direction = Vector2.Normalize(Destination - Position);
                var velocity = direction * Speed * delta;

                Position += velocity;

                if (Vector2.Distance(Position, Destination) >= distance)
                {
                    CurrentAnimation = WalkAnimations.GetIdleAnimationFrom(CurrentAnimation);
                    Position = Destination;
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
        public override void Draw(Layers.LayerType type)
        {
            base.Draw(type);
        }

        public void Destroy()
        {
            GameMap.Layers[LayerType.Entity].Sprites.Remove(this);
            Collections.Entities.TryRemove(UniqueId, out _);
        }
    }
}
