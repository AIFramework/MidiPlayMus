using AI;
using NAudio.Midi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midi.Data
{
    public class LoadMidi
    {

        MidiFile midiFile { get; set; }


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
