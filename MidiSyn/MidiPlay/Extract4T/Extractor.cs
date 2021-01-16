using NAudio.Midi;
using System.Collections.Generic;
using System.Linq;

namespace Midi.Extract4T
{
    public class Extractor
    {
        

        public Extractor(string path)
        {
            decimal currentMicroSecondsPerTick = 0m;

            MidiFile midi = new MidiFile(path);

            // Проход по каналам
            foreach (IList<MidiEvent> eventsIList in midi.Events)
            {
                List<MidiEvent> events = Midi2Wav.ToRealTime(eventsIList.ToList(), midi.DeltaTicksPerQuarterNote, ref currentMicroSecondsPerTick);

                // Проход по событиям
                for (int i = 0; i < events.Count; i++)
                {

                    //Ноты
                    if (events[i] is NoteOnEvent)
                    {
                        NoteOnEvent note = events[i] as NoteOnEvent;

                    }

                }
            }

        }




    }
}
