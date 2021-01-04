using AI;
using NAudio.Midi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiPlay.Data
{
    public class LoadMidi
    {



        public int Fd { get; set; } = -1; // Semple rate
        MidiFile midiFile { get; set; }





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


           

            foreach (var item in midiFile.Events)
            {

            }
            

            return semplList.ToArray();
        }


        public void Play() 
        {
            MidiOut midiOut = new MidiOut(0);
            
            foreach (var item in midiFile.Events)
            {
                foreach (var item1 in item)
                {
                    midiOut.Send(item1.GetAsShortMessage());
                }
            }



        }

        //public Wave
    }
}
