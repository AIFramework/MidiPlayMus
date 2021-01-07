using AI;
using MidiPlay.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiPlay.Instruments.TabelNotesGenerator
{
    public class Generator
    {
        /// <summary>
        /// Ноты для генерации
        /// </summary>
        public static string[] Notes { get; set; } = { "c", "c#", "d", "d#", "e", "f", "f#", "g", "g#", "a", "a#", "b" };

        public static int StartOct { get; set; } = 0;
        public static int EndOct { get; set; } = 9;


        public static TabelFonts GenFonts(SimpleSoundFont soundFont)
        {
            ElementTableFont[] elementTableFonts =  InitFont(soundFont);
            Dictionary<double, Vector> font = new Dictionary<double, Vector>();
            double min = elementTableFonts.MinFreq();

            for (int i = StartOct; i < EndOct; i++)
            {
                for (int j = 0; j < Notes.Length; j++)
                {
                    var freq = BaseFreqsNote.GetFreqNote(Notes[j], i);
                    var signal = GenSig(Notes[j], i, freq, min, elementTableFonts);
                    font.Add(freq, signal);
                }
            }

            return new TabelFonts(font);
        }

        private static ElementTableFont[] InitFont(SimpleSoundFont soundFont)
        {
            ElementTableFont[] elementTableFonts = new ElementTableFont[soundFont.Semples.Length];

            for (int i = 0; i < elementTableFonts.Length; i++)
            {
                string name = soundFont.Semples[i].NameNote;

                elementTableFonts[i] = new ElementTableFont()
                {
                    MainFreq = BaseFreqsNote.GetFreqNote(name.Split('_')[0], int.Parse(name.Split('_')[1])),
                    Signal = soundFont.Semples[i].Signal
                };
            }

            return elementTableFonts;
        }

        private static Vector GenSig(string name, int oct, double freq, double minFreq, ElementTableFont[] elementTableFonts)
        {
            if (freq < minFreq)
              return  BaseFreqsNote.Generator.GetNoteSignal(name, oct, 4);

            ElementTableFont elementTableFont = elementTableFonts.ClosesFtromBottom(freq);

            if (elementTableFont.Dist < 1)
                return elementTableFont.Signal;

            return BaseFreqsNote.TransferNote(elementTableFont.MainFreq, name, oct, elementTableFont.Signal);

        }
    }
}
