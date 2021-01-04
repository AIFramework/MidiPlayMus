using AI;
using AI.DataStructs;

namespace MidiPlay.Data
{
    public sealed class SempleFont : IBinSerial
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
