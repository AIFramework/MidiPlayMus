using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                var indLeft = Array.IndexOf(notesAll, left.Name);
                var indRight = Array.IndexOf(notesAll, right.Name);
                if (indLeft == indRight)
                    return 0;

                if (indLeft > indRight)
                    return 1;
                
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
