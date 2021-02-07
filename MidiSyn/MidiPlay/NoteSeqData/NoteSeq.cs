﻿using AI;
using Midi.NoteSeqData.Base;
using NAudio.Midi;
using System.Collections.Generic;
using System.Linq;
using NoteForSeq = Midi.NoteSeqData.Base.Note;
using Source = Midi.NoteSeqData.Base.SourceInfo;

namespace Midi.NoteSeqData
{
    public class NoteSeq
    {

        public List<NoteForSeq> Notes { get; private set; }
        public double TotalTime { get { Sort(); return Notes.Last().EndTime; } private set => TotalTime = value; }
        public Source SourceInfo { get; private set; }
        private bool IsSorted;


        public NoteSeq()
        {
            Notes = new List<NoteForSeq>();
        }

        public void Add(NoteForSeq note)
        {
            IsSorted = false;
            Notes.Add(note);
        }

        public void AddRange(IEnumerable<NoteForSeq> notes)
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

        public static NoteSeq MidiFileToNoteSequence(string path)
        {
            NoteSeq noteSeq = new NoteSeq();
            MidiFile midiFile = new MidiFile(path);

            List<List<MidiEvent>> midiEvents = GetMidiEvents(midiFile);
            decimal currentMicroSecondsPerTick = 0m;
            foreach (List<MidiEvent> midiEvent in midiEvents)
            {
                List<NoteForSeq> notes = MidiConverter.ToRealTime(midiEvent, midiFile.DeltaTicksPerQuarterNote, ref currentMicroSecondsPerTick);
                noteSeq.AddRange(notes);
            }
            noteSeq.Sort();
            return noteSeq;
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

        private static NoteSeq ToNoteSeq(Vector[] vecs)
        {
            NoteSeq seq = new NoteSeq();
            float len = 0;
            foreach(var vec in vecs)
            {
                var bagOfWords = vec.GetInterval(0, Midi.Base.Constants._tonesCount);
                var noteParams = vec.GetInterval(Midi.Base.Constants._tonesCount, vec.Count);
                var note = NoteForSeq.NoteFromVector(len, noteParams);
                seq.Add(note);

                len += note.EndTime;
            }

            return seq;
        }

        private void Sort()
        {
            if (!IsSorted)
            {
                Notes.Sort((left, right) => {
                    var dif = right.StartTime - left.StartTime;
                    if (dif > 0)
                        return 1;
                    else if (dif < 0)
                        return -1;
                    else return 0;
                });
                IsSorted = true;
            }
        }


    }
}
