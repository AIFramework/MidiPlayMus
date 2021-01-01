using AI;
using NAudio.Midi;
using System;
using System.Collections.Generic;

namespace MidiPlay.Data
{
    public class LoadMidi
    {



        public int Fd { get; set; } = -1; // Semple rate
        private MidiFile midiFile { get; set; }





        public LoadMidi(string path) 
        {
            midiFile = new MidiFile(path, false);
        }



        public Matrix ToMatrix() 
        {
            throw new Exception();
        }



        public Vector ToVector(string path)
        {
            List<double> semplList = new List<double>();


           

            foreach (IList<MidiEvent> item in midiFile.Events)
            {

            }
            

            return semplList.ToArray();
        }


        public void Play() 
        {
            MidiOut midiOut = new MidiOut(0);
            
            foreach (IList<MidiEvent> item in midiFile.Events)
            {
                foreach (MidiEvent item1 in item)
                {
                    midiOut.Send(item1.GetAsShortMessage());
                }
            }



        }

        //public Wave
    }
}
