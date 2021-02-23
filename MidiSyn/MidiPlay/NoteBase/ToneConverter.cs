using AI;

namespace Midi.NoteBase
{
    public static class ToneConverter
    {
        public static Vector ToneToVector(float tone)
        {
            int pos = (int)(tone * 2);
            Vector v = new Vector(Constants._tonesCount);
            v[pos] = 1;
            return v;
        }

        public static float VectorToTone(Vector v)
        {
            return v.IndexOf(1) / 2f;
        }
    }
}
