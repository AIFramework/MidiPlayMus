using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI;

namespace Midi
{
    public static class ToneConverter
    {
        public static Vector ToneToVector(float tone)
        {
            int pos = (int)(tone * 2);
            Vector v = new Vector(Base.Constants._tonesCount);
            v[pos] = 1;
            return v;
        }

        public static float VectorToTone(Vector v)
        {
            return v.IndexOf(1) / 2f;
        }
    }
}
