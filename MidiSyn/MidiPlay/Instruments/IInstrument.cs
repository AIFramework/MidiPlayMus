using AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiPlay.Instruments
{
    public interface IInstrument
    {
        void Create(int fd);
        Note GetNoteSignal(string name, int octave, double time);
    }
}
