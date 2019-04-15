﻿using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Layers;
using MonoGameClusterFuck.Networking.Packets;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck.Networking
{
    public static class PacketHandler
    {
        public static void Handle(NetworkClient socket, byte[] buffer)
        {
            var packetId = BitConverter.ToUInt16(buffer, 4);
            switch (packetId)
            {
                case 1000:
                {
                    var msgLogin = (MsgLogin)buffer;
                    socket.Player.UniqueId = msgLogin.UniqueId;

                    Collections.Entities.TryAdd(socket.Player.UniqueId, socket.Player);
                    break;
                }
                case 1001:
                    {
                        var msgWalk = (MsgWalk)buffer;
                        if (msgWalk.UniqueId == socket.Player.UniqueId)
                            return;

                        if (Collections.Entities.TryGetValue(msgWalk.UniqueId, out var entity))
                        {
                            var heading = msgWalk.Location - entity.Position;
                            var distance = heading.Length();
                            var direction = heading / distance;
                            
                            if (Math.Round(direction.X,0) == 1)
                            {
                                entity.CurrentAnimation = entity.WalkAnimations.IdleRight;
                            }
                            if (Math.Round(direction.X, 0) == -1)
                            {
                                entity.CurrentAnimation = entity.WalkAnimations.IdleLeft;
                            }
                            if (Math.Round(direction.Y, 0) == -1)
                            {
                                entity.CurrentAnimation = entity.WalkAnimations.IdleUp;
                            }
                            if (Math.Round(direction.Y, 0) == 1)
                            {
                                entity.CurrentAnimation = entity.WalkAnimations.IdleDown;
                            }

                            entity.Move(msgWalk.Location);
                        }
                        else
                        {
                                entity = new Entity(32);
                                entity.UniqueId = msgWalk.UniqueId;
                                entity.Initialize();
                                entity.LoadContent();
                                GameMap.Layers[LayerType.Entity].Add(entity);
                                Collections.Entities.TryAdd(entity.UniqueId, entity);
                        }

                        break;
                    }
                case 1002:
                {
                    var msgPing = (MsgPing)buffer;
                    var currentTicks = DateTime.UtcNow.Ticks;
                    var deltaTicks = currentTicks - msgPing.TickCount;
                    var ms = (int)deltaTicks / 1000;
                    FpsCounter.Ping = ms;
                        break;
                }
            }
        }
    }
}
