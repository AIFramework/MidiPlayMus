using AI;
using NAudio.Midi;
using System;

namespace Midi.Data
{
    public class LoadMidi
    {
        private MidiFile midiFile { get; set; }


        public LoadMidi(string path) 
        {
            midiFile = new MidiFile(path, false);
        }

        public Matrix ToMatrix() 
        {
            throw new Exception();
        }


    }
}
