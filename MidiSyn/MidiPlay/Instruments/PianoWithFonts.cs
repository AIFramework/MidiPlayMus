using AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiPlay.Instruments
{
    public class PianoWithFonts : IInstrument
    {
        public Dictionary<string, Vector> font = new Dictionary<string, Vector>();
        Dictionary<int, Vector> C_notes = new Dictionary<int, Vector>();

        public PianoWithFonts()
        {
        
        }


        public void Create(int fd)
        {
            if (fd != 44100)
                throw new Exception("Font fd is 44.1 kHz");


        }

        public Note GetNoteSignal(string name, int octave, double time)
        {
            throw new NotImplementedException();
        }
    }
}
