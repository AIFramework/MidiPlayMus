using AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiPlay.Data
{
    public class TabelFonts
    {
        Dictionary<double, Vector> font = new Dictionary<double, Vector>();


        public void Add(string nameNote, int octave, Vector signal)
        {
            double freq = BaseFreqsNote.GetFreqNote(nameNote, octave);

            if (font.ContainsKey(freq))
                font.Add(freq, signal);
        }
    }
}
