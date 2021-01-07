using AI.DataStructs;

namespace Midi.Data
{
    public class SimpleSoundFont : IBinSerial
    {
        public string Instrument { get; set; }
        public SampleFont[] Semples { get; set; }

        public void FromBts(byte[] bts)
        {
            BinarySerializaterDataInMemory binary = new BinarySerializaterDataInMemory(bts, isZip: true);
            binary.UnZipped();

            Instrument = binary.ReadString();
            int len = binary.ReadInt();
            Semples = new SampleFont[len];

            for (int i = 0; i < len; i++)
            {
                Semples[i] = new SampleFont();
                Semples[i].FromBts(binary.ReadBytes());
            }

        }

        public byte[] GetBts()
        {
            BinarySerializaterDataInMemory binary = new BinarySerializaterDataInMemory();
            binary.Add(Instrument);
            binary.Add(Semples);
            binary.Zipped();
            return binary.ToByteArray();
        }


        public void Save(string path)
        {
            BinarySerializaterDataInMemory binary = new BinarySerializaterDataInMemory();
            binary.Add(GetBts());
            binary.Save(path);
        }

        public static SimpleSoundFont Load(string path)
        {
            BinarySerializaterDataInMemory binary = new BinarySerializaterDataInMemory(path);
            SimpleSoundFont simpleSoundFont = new SimpleSoundFont();
            simpleSoundFont.FromBts(binary.ReadBytes());
            return simpleSoundFont;
        }
    }
}
