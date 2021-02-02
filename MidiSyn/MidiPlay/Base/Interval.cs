using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midi.Base
{
    public class Interval : IInterval
    {
        public string Note { get; set; }

        public float Tone { get; }

        public IntervalType IntervalType { get; }

        public Interval(string note, float tone, IntervalType intervalType)
        {
            Note = note;
            Tone = tone;
            IntervalType = intervalType;
        }

        public bool HasInterval(IEnumerable<string> notes)
        {
            var notesSrc = GetNotes();
            var a = notesSrc.OrderBy(x => x);
            var b = notes.OrderBy(x => x);

            return Enumerable.SequenceEqual(a, b);
        }

        public IEnumerable<string> GetNotes()
        {
            yield return Note;
            yield return AddTone(Note, Tone);
        }

        public static int GetBaseNote(IEnumerable<string> notes)
        {
            int ind = -1;
            string min = null;

            int i = 0;
            foreach (var note in notes)
            {
                if(ind == -1 || string.Compare(note, min) < 0)
                {
                    min = note;
                    ind = i;
                }
                i++;
            }

            return ind;
        }

        public static string AddTone(string note, float tone)
        {
            var lenF = tone * 2;
            var len = (int)lenF;
            var ind = Array.IndexOf(Constants._notesAll, note);
            if (lenF != len)
                throw new Exception("tone must be 0.5x, where x is integer");
            if (ind == -1)
                throw new Exception($"{note} is not note");

            ind += len;
            if (ind < 0)
            {
                ind += 5 * (Math.Abs(ind) / 5 + 1);
            }
            ind %= Constants._notesAll.Length;

            return Constants._notesAll[ind];
        }
    }
}
