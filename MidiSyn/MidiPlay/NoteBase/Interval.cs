using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midi.Extensions;

namespace Midi.NoteBase
{
    public class Interval
    { 
        public string Note { get; set; }

        public float Tone { get; }

        public Interval(string note, float tone)
        {
            Note = note;
            Tone = tone;
        }

        /// <summary>
        /// Recognize tone of interval (noteGoal must be more or equal than noteBase)
        /// </summary>
        /// <param name="noteBase">(examle: D#1)</param>
        /// <param name="noteGoal">(examle: F#5)</param>
        /// <returns></returns>
        public static float RecognizeTone(string noteBase, string noteGoal)
        {
            int l = Constants._notesAll.Length;
            int octaveBase, octaveGoal;
            TryParseOctave(noteBase, out octaveBase);
            TryParseOctave(noteGoal, out octaveGoal);

            noteBase = noteBase.Substring(0, noteBase.Length - $"{octaveBase}".Length);
            noteGoal = noteGoal.Substring(0, noteGoal.Length - $"{octaveGoal}".Length);

            var indBase = Constants._notesAll.IndexOf(noteBase);
            var indGoal = Constants._notesAll.IndexOf(noteGoal);

            indBase = (octaveBase - 1) * l + indBase;
            indGoal = (octaveGoal - 1) * l + indGoal;

            return (indGoal - indBase) / 2f;
        }

        private static bool TryParseOctave(string str, out int num)
        {
            num = int.MinValue;
            var pos = str.Length;
            for (int i = str.Length - 1; i > -1; i--)
            {
                if (!int.TryParse(str[i] + "", out _))
                {
                    pos = i + 1;
                    break;
                }
            }

            if (pos == str.Length)
                return false;

            str = str.Substring(pos);
            num = int.Parse(str);

            return true;
        }
    }
}
