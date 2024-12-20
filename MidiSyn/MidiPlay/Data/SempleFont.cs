﻿using AI;
using AI.DataStructs;

namespace Midi.Data
{
    public sealed class SampleFont : IBinSerial
    {
        public string NameNote { get; set; }
        public Vector Signal { get; set; }

        public void FromBts(byte[] bts)
        {
            BinarySerializaterDataInMemory binary = new BinarySerializaterDataInMemory(bts);
            NameNote = binary.ReadString();
            Signal = binary.ReadVector();
        }

        public byte[] GetBts()
        {
            BinarySerializaterDataInMemory binary = new BinarySerializaterDataInMemory();
            binary.Add(NameNote);
            binary.Add(Signal);
            return binary.ToByteArray();
        }

    }
}
