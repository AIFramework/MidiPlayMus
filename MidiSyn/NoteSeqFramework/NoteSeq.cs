using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Midi;
using NoteSeqFramework.Base;

namespace NoteSeqFramework
{
    public class NoteSeq
    {
        public List<Note> Notes { get; private set; }
        public double TotalTime { get { Sort(); return Notes.Last().EndTime; } private set { TotalTime = value; } }
        public SourceInfo SourceInfo { get; private set; }
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

        private void Sort()
        {
            if (!IsSorted)
            {
                Notes.Quicksort((left, right) => right.StartTime > left.StartTime);
                IsSorted = true;
            }
        }

        public static NoteSeq MidiFileToNoteSequence(string path)
        {
            var noteSeq = new NoteSeq();
            var midiFile = new MidiFile(path);

            var midiEvents = GetMidiEvents(midiFile);
            var noteEvents = new List<List<NoteEvent>>();
            decimal currentMicroSecondsPerTick = 0m;
            foreach (var midiEvent in midiEvents)
            {
                var preproc = MidiConverter.ToRealTime(midiEvent, midiFile.DeltaTicksPerQuarterNote, ref currentMicroSecondsPerTick);
                var events = preproc
                        .Where(x => x is NoteOnEvent)
                        .Select(x => (NoteOnEvent)x)
                        .ToList();

                noteSeq.AddRange(events.Select(note=>new Note(
                    note.NoteNumber,
                    note.Velocity,
                    (float)(note.AbsoluteTime / 1000.0),
                    (float)((note.AbsoluteTime + note.NoteLength) / 1000.0),
                    Instrument.Default,
                    note.NoteName
                )));
            }
            noteSeq.Sort();
            return noteSeq;
        }

        private static List<List<MidiEvent>> GetMidiEvents(MidiFile midiFile)
        {
            var channels = new List<List<MidiEvent>>();
            foreach (var channel in midiFile.Events)
            {
                channels.Add(channel.ToList());
            }

            return channels;
        }

    }
}
