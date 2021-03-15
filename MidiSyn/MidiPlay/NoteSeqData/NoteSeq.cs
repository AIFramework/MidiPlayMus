using AI;
using Midi.NoteSeqData.Base;
using NAudio.Midi;
using System.Collections.Generic;
using System.Linq;
using MConstants = Midi.NoteBase.MConstants;
using Source = Midi.NoteSeqData.Base.SourceInfo;

namespace Midi.NoteSeqData
{
    public class NoteSeq
    {

        public List<Note> Notes { get; private set; }
        public double TotalTime { get { return GetLastEndTime(); } }
        public Source SourceInfo { get; private set; }

        private bool IsSorted;

        public NoteSeq()
        {
            Notes = new List<Note>();
        }

        public void Add(Note note)
        {
            IsSorted = false;
            Notes.Add(note);
        }

        public void AddRange(IEnumerable<Note> notes)
        {
            IsSorted = false;
            Notes.AddRange(notes);
        }

        /// <summary>
        /// Возвращает ноты как векторы
        /// </summary>
        public Vector[] ToVectors()
        {
            Vector[] vectors = new Vector[Notes.Count];
            for (int i = 0; i < Notes.Count; i++)
            {
                vectors[i] = Notes[i].ToVector();
            }

            return vectors;
        }

        /// <summary>
        /// Load NoteSeq from file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static NoteSeq LoadMidiAsNoteSequence(string path)
        {
            NoteSeq noteSeq = new NoteSeq();
            MidiFile midiFile = new MidiFile(path);

            List<List<MidiEvent>> midiEvents = GetMidiEvents(midiFile);
            decimal currentMicroSecondsPerTick = 0m;
            foreach (List<MidiEvent> midiEvent in midiEvents)
            {
                List<Note> notes = MidiConverter.ToRealTime(midiEvent, midiFile.DeltaTicksPerQuarterNote, ref currentMicroSecondsPerTick);
                noteSeq.AddRange(notes);
            }
            noteSeq.Sort();
            return noteSeq;
        }

        public static NoteSeq ToNoteSeq(Vector[] vecs)
        {
            NoteSeq seq = new NoteSeq();
            float len = 0;
            foreach (var vec in vecs)
            {
                var bagOfWords = vec.GetInterval(0, MConstants._tonesCount);
                var noteParams = vec.GetInterval(MConstants._tonesCount, vec.Count);
                var note = Note.NoteFromVector(len, noteParams);
                seq.Add(note);

                len += note.EndTime;
            }

            return seq;
        }

        /// <summary>
        /// Группировка нот по времени (start time), для выделения аккордов
        /// </summary>
        /// <param name="noteSeq"></param>
        /// <returns></returns>
        public static Note[][] GroupByTimeStep(NoteSeq noteSeq)
        {
            var notes = noteSeq.Notes;
            var timesteps = notes.GroupBy(x => x.StartTime).ToArray();

            var result = new Note[timesteps.Length][];

            for (int i = 0; i < timesteps.Length; i++)
                result[i] = timesteps[i].ToArray();

            return result;
        }

        private static List<List<MidiEvent>> GetMidiEvents(MidiFile midiFile)
        {
            List<List<MidiEvent>> channels = new List<List<MidiEvent>>();
            foreach (IList<MidiEvent> channel in midiFile.Events)
            {
                channels.Add(channel.ToList());
            }

            return channels;
        }

        private void Sort()
        {
            if (!IsSorted)
            {
                Notes.Sort((left, right) =>
                {
                    var dif = right.StartTime - left.StartTime;
                    if (dif < 0)
                        return 1;
                    else if (dif > 0)
                        return -1;
                    else return 0;
                });
                IsSorted = true;
            }
        }

        private float GetLastEndTime()
        {
            Sort();
            return Notes.Last().EndTime;
        }
    }
}
