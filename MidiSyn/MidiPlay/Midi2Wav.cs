using AI;
using MidiPlay.Data;
using MidiPlay.Instruments;
using NAudio.Midi;
using System.Collections.Generic;
using System.Linq;

namespace MidiPlay
{
    public class Midi2Wav
    {
        private readonly Notes notes = new Notes();
        private readonly double ticsPerSeconds = 1.0/1000;
        private readonly int _fd;

        public Midi2Wav(string path, int fd) 
        {
            _fd = notes.Fd = fd;
            MidiFile midi = new MidiFile(path);
            decimal currentMicroSecondsPerTick = 0m;

            // Создание пианино (Гармонический синтез)
            IInstrument gPiano = new GSyntPiano();
            gPiano.Create(fd);

            // Создание басса (Гармонический синтез)
            IInstrument gBass = new GSBass();
            gBass.Create(fd);

            // Проход по каналам
            foreach (IList<MidiEvent> eventsIList in midi.Events)
            {
                List<MidiEvent> events = ToRealTime(eventsIList.ToList(), midi.DeltaTicksPerQuarterNote, ref currentMicroSecondsPerTick);

                // Проход по событиям
                for (int i = 0; i < events.Count; i++)
                {
                    
                    //Ноты
                    if (events[i] is NoteOnEvent)
                    {
                        NoteOnEvent note = events[i] as NoteOnEvent;

                        if (note.Velocity != 0)
                        {
                            //Имя ноты
                            string name = new string(
                                note.NoteName.ToCharArray().
                                Take(note.NoteName.Length - 1).
                                ToArray());
                            //Октава
                            int octave = int.Parse(new string(
                                note.NoteName.ToCharArray().
                                Skip(name.Length).
                                ToArray()));

                            // Данные ноты
                            NoteWithTime noteWithTime = new NoteWithTime()
                            {
                                StartTime = note.AbsoluteTime * ticsPerSeconds,
                                EndTime = (note.NoteLength + note.AbsoluteTime) * ticsPerSeconds,
                                Note = gPiano.GetNoteSignal(name, octave, note.NoteLength * ticsPerSeconds),
                                Volume = note.Velocity * 0.01
                            };

                            // Данные ноты (Дублирование на октаву ниже)
                            NoteWithTime noteWithTimeD = new NoteWithTime()
                            {
                                StartTime = note.AbsoluteTime * ticsPerSeconds,
                                EndTime = (note.NoteLength + note.AbsoluteTime) * ticsPerSeconds,
                                Note = gBass.GetNoteSignal(name, octave-2, note.NoteLength * ticsPerSeconds),
                                Volume = 0.5*note.Velocity * 0.01
                            };


                            notes.Add(noteWithTime); // Добавлнение ноты в список
                            notes.Add(noteWithTimeD); // Добавлнение ноты в список
                        }
                    }
                }
            }
        }

       


        public void Play()
        {
            Vector sig = notes.Generate();
            WavMp3.Play(sig, _fd);
        }


        public void Save(string path)
        {
            Vector sig = notes.Generate();
            WavMp3.Save(sig, _fd, path);
        }

        public Vector ToVector() 
        {
          return notes.Generate();
        }





        // Приведение к реальному времени
        private static List<MidiEvent> ToRealTime(List<MidiEvent> midiEvents, int deltaTicksPerQuarterNote, ref decimal currentMicroSecondsPerTick)
        {

            List<decimal> eventsTimesArr = new List<decimal>();
            decimal lastRealTime = 0m;
            decimal lastAbsoluteTime = 0m;

            for (int i = 0; i < midiEvents.Count; i++)
            {
                MidiEvent midiEvent = midiEvents[i];
                TempoEvent tempoEvent = midiEvent as TempoEvent;

                if (midiEvent.AbsoluteTime > lastAbsoluteTime)
                {
                    lastRealTime += (midiEvent.AbsoluteTime - lastAbsoluteTime) * currentMicroSecondsPerTick;
                }

                lastAbsoluteTime = midiEvent.AbsoluteTime;

                if (tempoEvent != null)
                {
                    currentMicroSecondsPerTick = tempoEvent.MicrosecondsPerQuarterNote / (decimal)deltaTicksPerQuarterNote;
                    midiEvents.RemoveAt(i--);
                    continue;
                }

                eventsTimesArr.Add(lastRealTime);
            }

            for (int i = 0; i < midiEvents.Count; i++)
            {
                midiEvents[i].AbsoluteTime = (long)(eventsTimesArr[i] / 1000);
            }

            return midiEvents;
        }

    }
}
