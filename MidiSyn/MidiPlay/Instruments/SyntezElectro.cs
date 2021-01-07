using AI;
using AI.DSP;
using AI.DSP.MusicUtils;
using AI.DSPCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiPlay.Instruments
{
    public class SyntezElectro : IInstrument
    {

        /// <summary>
        /// Частота дискретизации
        /// </summary>
        public int Fd { get; protected set; }

        protected double alpha = 0.07;


        /// <summary>
        /// Создание пианино
        /// </summary>
        /// <param name="fd"></param>
        public virtual void Create(int fd = 16000)
        {
            Fd = fd;
        }

        /// <summary>
        /// Генерация ноты
        /// </summary>
        /// <param name="name"></param>
        /// <param name="octave"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public virtual Note GetNoteSignal(string name, int octave, double time)
        {

            var nameNote = $"{name}_{octave}";
            
            double freq = BaseFreqsNote.GetFreqNote(name, octave);


            int steps = (int)(time * Fd);
            Vector window = PhaseCorrectingWindow.Trapezoid(steps, 0.02);
            Vector sig = Generate(steps, freq, window);

            return new Note(nameNote, time, sig);
        }

        // Генерация звуковой волны
        private Vector Generate(int steps, double freq, Vector window)
        {
            Vector Signal = new Vector(steps);

            double kS1 = 0;
            double kS2 = 0;
            double freq2 = freq * 2.1;


            for (int i = 0; i < steps; i++)
            {
                double timeStep = (double)i / Fd;
                double s = kS1 *Math.Sin(6.282 * timeStep * freq);
                s += kS2 * Math.Sin(6.282 * timeStep * freq2);

                kS1 = Math.Cos(6.282 * timeStep * 9*Math.Abs( Math.Cos(6.282 * timeStep * freq)));
                kS1 = Math.Abs(kS1);
                kS2 = 1.0 - kS1;

                Signal[i] = s;
            }

            Signal = Filters.FilterKontur(Signal, 2, freq, Fd);
            Signal *= window;

            return Signal/ Signal.Max();
        }

        

        // Огибающая
        double GetEnvSempl(double step, double per60)
        {
            return Math.Exp(-alpha * (step / per60));
        }

    }
}
