﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace MonoGameClusterFuck.Networking.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgPing
    {
        public int Length;
        public ushort Id;
        public int TickCount;
        public uint UniqueId;

        public static MsgPing Create(uint uniqueId)
        {
            var msg = stackalloc MsgPing[1];
            msg->Length = sizeof(MsgPing);
            msg->Id = 1002;
            msg->TickCount = Environment.TickCount;
            msg->UniqueId = uniqueId;
            return *msg;
        }
        public static implicit operator byte[] (MsgPing msg)
        {
            var buffer = new byte[sizeof(MsgPing)];
            fixed (byte* p = buffer)
                *(MsgPing*)p = *&msg;
            return buffer;
        }
        public static implicit operator MsgPing(byte[] msg)
        {
            fixed (byte* p = msg)
                return *(MsgPing*)p;
        }
    }
}