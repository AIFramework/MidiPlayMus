using System;
using AI;
using Midi.NoteSeqData.Base;

namespace Midi.NoteBase
{
    public class Accord
    {
        public static Vector ToBOW(Note[] timestep)
        {
            var notesAll = Constants._notesAll;
            Array.Sort(timestep, (left, right) =>
            {
                var cmp = left.Pitch - right.Pitch;
                if (cmp > 0)
                    return 1;
                if (cmp == 0)
                    return 0;

                return -1;
            });

            var vec = new Vector(Constants._tonesCount);

            for (int i = 1; i < timestep.Length; i++)
            {
                var tone = Interval.RecognizeTone(timestep[i - 1].Name, timestep[i].Name);
                vec += ToneConverter.ToneToVector(tone);
            }

            return vec;
        }
    }
}
