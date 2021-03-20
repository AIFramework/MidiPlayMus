using AI;
using Midi.Extensions;
using Midi.NoteSeqData.Base;
using System;
using System.Collections.Generic;

namespace Midi.NoteBase
{
    public class Accord
    {
        private const int _intervalCount = 5;
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

            int l = _intervalCount * MConstants._bowLen;
            var vec = new Vector(0);

            for (int i = 1; i < timestep.Length && i < _intervalCount + 1; i++)
            {
                vec = ConcatinateTimestep(vec, timestep[i - 1].Name, timestep[i].Name);
            }

            if (timestep.Length == 1)
            {
                vec = ConcatinateTimestep(vec, timestep[0].Name, timestep[0].Name);
            }

            if (vec.Count != l)
                vec = Vector.Concatinate(new Vector[] { vec, new Vector(l - vec.Count) });

            return vec;
        }


        public static List<Note> ToAccord(Vector bow, int startTime, int endTime)
        {
            int l = GetIntervalsCount(bow);
            var notes = new List<Note>();

            int g = 0;
            for (int pos = 0; pos < bow.Count; pos += MConstants._bowLen)
            {
                int octave = -1;
                for (int i = 0; i < MConstants._octavesCount; i++)
                {
                    if (bow[pos + i] == 1)
                    {
                        octave = i;
                        break;
                    }
                }

                int noteInd1 = 0;
                for (int i = 0; i < MConstants._notesAll.Length; i++)
                {
                    if (bow[MConstants._octavesCount + pos + i] == 1)
                    {
                        noteInd1 = i;
                        break;
                    }
                }

                float tone = 0;
                for (int i = 0; i < MConstants._tonesCount; i++)
                {
                    if (bow[MConstants._octavesCount + MConstants._notesAll.Length + pos + i] == 1)
                    {
                        tone = i;
                        break;
                    }
                }
                tone /= 2f;

                if (octave != -1)
                {
                    var noteName1 = MConstants._notesAll[noteInd1];
                    noteName1 += octave;
                    var pitch1 = Note.GetPitch(noteName1);
                    var note1 = new Note(pitch1, 100, startTime, endTime, Instrument.Default, noteName1);

                    var noteName2 = AddTone(noteName1, tone);
                    var pitch2 = Note.GetPitch(noteName2);
                    var note2 = new Note(pitch2, 100, startTime, endTime, Instrument.Default, noteName2);

                    notes.Add(note1);
                    if (note1 != note2)
                        notes.Add(note2);
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

        private static Vector ConcatinateTimestep(Vector vec, string noteBase, string noteTarget)
        {
            var tone = Interval.RecognizeTone(noteBase, noteTarget);
            var interval = new Interval(noteBase, tone);
            return Vector.Concatinate(new Vector[] { vec, interval.ToVector() });
        }

        private static int GetIntervalsCount(Vector bow)
        {
            int count = 0;
            for (int i = 0; i < bow.Count; i += MConstants._bowLen)
            {
                for (int j = 0; j < MConstants._octavesCount; j++)
                {
                    if (bow[i + j] == 1)
                    {
                        count++;
                        break;
                    }
                }
            }

            return count;
        }
    }
}
