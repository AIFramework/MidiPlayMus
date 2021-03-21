using AI;
using Midi.Extensions;
using Midi.NoteSeqData.Base;
using System;
using System.Collections.Generic;

namespace Midi.NoteBase
{
    /*
     *   Октава: всего 10 октав|отсчетов
     *   Нота: всего 12 нот|отсчетов
     *   Тон: всего 108 отсчетов
     *   Длительность1: длительность 1 ноты; 50мс * 30 отчетов = 1.5с (макс)
     *   Длительность2: длительность 2 ноты; 50мс * 30 отчетов = 1.5с (макс)
     */
    public class Accord
    {
        private const int _intervalsCount = 5;
        public static Vector ToBOW(Note[] timestep)
        {
            var notesAll = MConstants._notesAll;
            Array.Sort(timestep, (left, right) =>
            {
                var cmp = left.Pitch - right.Pitch;
                if (cmp > 0)
                    return 1;
                if (cmp == 0)
                    return 0;

                return -1;
            });

            int l = _intervalsCount * MConstants._bowLen;
            var vec = new Vector(0);

            for (int i = 1; i < timestep.Length && i < _intervalsCount + 1; i++)
            {
                vec = ConcatinateTimestep(vec, timestep[i - 1], timestep[i]);
            }

            if (timestep.Length == 1)
            {
                vec = ConcatinateTimestep(vec, timestep[0], timestep[0]);
            }

            if (vec.Count != l)
                vec = Vector.Concatinate(new Vector[] { vec, new Vector(l - vec.Count) });

            return vec;
        }


        public static List<Note> ToAccord(Vector prob, float startTime)
        {
            var notes = new List<Note>();

            for (int pos = 0; pos < prob.Count; pos += MConstants._bowLen)
            {
                int shift = pos;
                FindIndexMax(prob, ref shift, MConstants._octavesCount, out int octave);
                FindIndexMax(prob, ref shift, MConstants._notesAll.Length, out int noteInd1);
                FindIndexMax(prob, ref shift, MConstants._tonesCount, out int toneInd);
                FindIndexMax(prob, ref shift, MConstants._durationCount, out int durationInd1);
                FindIndexMax(prob, ref shift, MConstants._durationCount, out int durationInd2);

                if (shift - pos != MConstants._bowLen)
                    throw new Exception("Error shift");

                float tone = toneInd / 2f;
                float duration1 = durationInd1 * MConstants._duration / 1000f;
                float duration2 = durationInd2 * MConstants._duration / 1000f;

                if (octave != -1)
                {
                    var noteName1 = string.Concat(MConstants._notesAll[noteInd1], octave);
                    var pitch1 = Note.GetPitch(noteName1);
                    var note1 = new Note(pitch1, 100, startTime, startTime + duration1, Instrument.Default, noteName1);
                    notes.Add(note1);

                    //try
                    //{
                    //    var noteName2 = AddTone(noteName1, tone);
                    //    var pitch2 = Note.GetPitch(noteName2);
                    //    var note2 = new Note(pitch2, 100, startTime, endTime, Instrument.Default, noteName2);
                    //    if (note1 != note2)
                    //        notes.Add(note2);
                    //}
                    //catch
                    //{

                    //}
                }
            }

            return notes;
        }

        public static string AddTone(string noteName, float tone)
        {
            if (Note.TryParseNote(noteName, out string note) && Note.TryParseOctave(noteName, out int octave))
            {
                var di = (int)(tone * 2f);
                var doct = di / MConstants._notesAll.Length;
                var dnote = di % MConstants._notesAll.Length;
                var ind = MConstants._notesAll.IndexOf(note);
                octave += doct;
                ind += dnote;

                return MConstants._notesAll[ind] + octave;
            }

            throw new Exception("Incorrect note");
        }

        private static Vector ConcatinateTimestep(Vector vec, Note noteBase, Note noteTarget)
        {
            var dpos1 = 1000.0f * (noteBase.EndTime - noteBase.StartTime) / MConstants._duration;
            var dpos2 = 1000.0f * (noteTarget.EndTime - noteTarget.StartTime) / MConstants._duration;
            dpos1 = Math.Min(dpos1, MConstants._durationMax / MConstants._duration); /* max 1.5s */
            dpos2 = Math.Min(dpos2, MConstants._durationMax / MConstants._duration);

            var tone = Interval.RecognizeTone(noteBase.Name, noteTarget.Name);
            var interval = new Interval(noteBase.Name, tone);
            vec = Vector.Concatinate(new Vector[] {
                vec,
                interval.ToVector(),
                Vector.OneHotPol((int)dpos1, MConstants._durationCount - 1),
                Vector.OneHotPol((int)dpos2, MConstants._durationCount - 1)
            });

            if (vec.Count % MConstants._bowLen != 0)
                throw new Exception($"vec.Count = {vec.Count} not equal " + MConstants._bowLen);

            return vec;
        }

        private static void FindIndexMax(Vector x, ref int shift, int len, out int indMax)
        {
            indMax = 0;
            var max = double.MinValue;
            for (int i = 0; i < len; i++)
            {
                if (max < x[shift + i])
                {
                    indMax = i;
                }
            }

            shift += len;
        }
    }
}
