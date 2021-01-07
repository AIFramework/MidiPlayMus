using Midi.Data;
using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midi.Extract4T
{
    public class Extractor
    {
        

        public Extractor(string path)
        {
            decimal currentMicroSecondsPerTick = 0m;

            MidiFile midi = new MidiFile(path);

            // Проход по каналам
            foreach (var eventsIList in midi.Events)
            {
                var events = Midi2Wav.ToRealTime(eventsIList.ToList(), midi.DeltaTicksPerQuarterNote, ref currentMicroSecondsPerTick);

                // Проход по событиям
                for (int i = 0; i < events.Count; i++)
                {

                    //Ноты
                    if (events[i] is NoteOnEvent)
                    {
                        var note = events[i] as NoteOnEvent;

                    }

                }
            }

        }




    }
}
