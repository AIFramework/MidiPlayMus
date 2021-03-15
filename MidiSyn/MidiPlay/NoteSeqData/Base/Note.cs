using AI;
using Midi.Extensions;
using Midi.NoteBase;
using System;

namespace Midi.NoteSeqData.Base
{
    [Serializable]
    public class Note
    {
        public int Pitch { get; set; }
        public int Velocity { get; set; }
        public float StartTime { get; set; }
        public float EndTime { get; set; }
        public Instrument Instrument { get; set; }
        public string Name { get; set; }

        public Note()
        {

        }

        public Note(int pitch, int velocity, float startTime, float endTime, Instrument instrument, string name)
        {
            Pitch = pitch;
            Velocity = velocity;
            StartTime = startTime;
            EndTime = endTime;
            Instrument = instrument;
            Name = name;
        }

        /// <summary>
        /// Преобразование ноты в вектор по правилу 
        /// { pitch/127, velocity/100.0, (endTime - startTime)/2.0 }
        /// </summary>
        public Vector ToVector()
        {
            double pitch = Pitch / 127.0;
            double velocity = Velocity / 100.0;
            double len = (EndTime - StartTime) / 2.0;

            pitch = pitch > 1.0 ? 1.0 : pitch;
            velocity = velocity > 1.0 ? 1.0 : velocity;
            len = len > 1.0 ? 1.0 : len;

            return new Vector(pitch, velocity, len);
        }

        /// <summary>
        /// Вектор в ноту
        /// </summary>
        public static Note NoteFromVector(float startTime, Vector noteV)
        {
            return new Note((int)(noteV[0] * 127), (int)(noteV[1] * 100), startTime, startTime + (float)noteV[2] * 2, Instrument.Default, "");
        }

        public static int GetPitch(string noteName)
        {
            if (TryParseOctave(noteName, out int octave) && TryParseNote(noteName, out string note))
            {
                var notes = MConstants._notesAll;
                return octave * notes.Length + notes.IndexOf(note);
            }

            throw new Exception("Incorrect note name");
        }

        public static bool TryParseOctave(string noteName, out int octave)
        {
            octave = int.MinValue;
            var pos = noteName.Length;
            for (int i = noteName.Length - 1; i > -1; i--)
            {
                if (!int.TryParse(noteName[i] + "", out _))
                {
                    pos = i + 1;
                    break;
                }
            }

            if (pos == noteName.Length)
                return false;

            noteName = noteName.Substring(pos);
            octave = int.Parse(noteName);

            return true;
        }

        public static bool TryParseNote(string noteName, out string note)
        {
            note = null;
            var pos = noteName.Length;
            for (int i = noteName.Length - 1; i > -1; i--)
            {
                if (!int.TryParse(noteName[i] + "", out _))
                {
                    pos = i + 1;
                    break;
                }
            }

            if (pos == noteName.Length)
                return false;

            note = noteName.Substring(0, pos);

            return true;
        }
    }
}
