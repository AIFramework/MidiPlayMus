using AI;
using Midi.Extensions;
using System;
using NoteC = Midi.NoteSeqData.Base.Note;

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
        /// 
        /// </summary>
        /// <returns>Length: tones_count * notes_count</returns>
        public Vector ToVector()
        {
            int posTone = (int)(Tone * 2);
            if (NoteC.TryParseNote(Note, out string note) && NoteC.TryParseOctave(Note, out int octave))
            {
                int posNote = MConstants._notesAll.IndexOf(note);
                Vector v = new Vector(MConstants._bowLen);
                v[octave] = 1;
                v[MConstants._octavesCount + posNote] = 1;
                v[MConstants._octavesCount + MConstants._notesAll.Length + posTone] = 1;

                return v;
            }

            throw new Exception("Incorrect note");
        }

        //public static float VectorToTone(Vector v)
        //{
        //    return v.IndexOf(1) / 2f;
        //}

        /// <summary>
        /// Recognize tone of interval (noteGoal must be more or equal than noteBase)
        /// </summary>
        /// <param name="noteBase">(examle: D#1)</param>
        /// <param name="noteTarget">(examle: F#5)</param>
        /// <returns></returns>
        public static float RecognizeTone(string noteBase, string noteTarget)
        {
            int l = MConstants._notesAll.Length;
            NoteC.TryParseOctave(noteBase, out int octaveBase);
            NoteC.TryParseOctave(noteTarget, out int octaveGoal);

            noteBase = noteBase.Substring(0, noteBase.Length - $"{octaveBase}".Length);
            noteTarget = noteTarget.Substring(0, noteTarget.Length - $"{octaveGoal}".Length);

            var indBase = MConstants._notesAll.IndexOf(noteBase);
            var indGoal = MConstants._notesAll.IndexOf(noteTarget);

            indBase = (octaveBase - 1) * l + indBase;
            indGoal = (octaveGoal - 1) * l + indGoal;

            return (indGoal - indBase) / 2f;
        }
    }
}
