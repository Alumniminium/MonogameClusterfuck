using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace One.Networking.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgWalk
    {
        public int Length;
        public ushort Id;
        public int TickCount;
        public uint UniqueId;
        public Vector2 Location;
        
        public static MsgWalk Create(uint uniqueId, Vector2 location)
        {
            var msg = stackalloc MsgWalk[1];
            msg->Length = sizeof(MsgWalk);
            msg->Id = 1001;
            msg->TickCount = Environment.TickCount;
            msg->UniqueId = uniqueId;
            msg->Location = location;
            return *msg;
        }
        public static implicit operator byte[] (MsgWalk msg)
        {
            var buffer = new byte[sizeof(MsgWalk)];
            fixed (byte* p = buffer)
                *(MsgWalk*)p = *&msg;
            return buffer;
        }
        public static implicit operator MsgWalk(byte[] msg)
        {
            fixed (byte* p = msg)
                return *(MsgWalk*)p;
        }
    }
}