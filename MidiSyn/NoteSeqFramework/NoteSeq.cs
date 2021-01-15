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
            //Base();
            var midiFile = new MidiFile(path);

            var midiEvents = GetMidiEvents(midiFile);
            var noteEvents = new List<List<NoteEvent>>();
            decimal currentMicroSecondsPerTick = 0m;
            foreach (var midiEvent in midiEvents)
            {
                var preproc = MidiConverter.ToRealTime(midiEvent, midiFile.DeltaTicksPerQuarterNote, ref currentMicroSecondsPerTick);
                var events = preproc
                        .Where(x => x is NoteEvent)
                        .Select(x => (NoteEvent)x)
                        .Where(x => x.CommandCode == MidiCommandCode.NoteOn)
                        .Select(x => new NoteEvent(x.AbsoluteTime, x.Channel, x.CommandCode, x.NoteNumber, x.Velocity))
                        .ToList();
                if (events.Count > 0)
                    noteEvents.Add(events);
            }

            NoteSeq noteSeq = new NoteSeq();
            for (int i = 0; i < noteEvents.Count; i++)
            {
                for (int j = 0; j < noteEvents[i].Count; j++)
                {
                    var noteEvent = noteEvents[i][j];
                    var startTime = (float)(noteEvent.AbsoluteTime / 1000.0);
                    var endTime = (float)((noteEvent.AbsoluteTime + noteEvent.DeltaTime) / 1000.0);
                    noteSeq.Add(new Note(noteEvent.NoteNumber, noteEvent.Velocity, startTime, endTime, Instrument.Default, noteEvent.NoteName));
                }
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
