using AI;
using AI.DSP.MusicUtils;
using System;

namespace Midi.Instruments.TabelNotesGenerator
{
    public class ToneGenerator
    {
        /// <summary>
        /// Массив апмлитуд
        /// </summary>
        public Vector Magns { get; protected set; }

        /// <summary>
        /// Частота дискретизации
        /// </summary>
        public int Fd { get; protected set; }

        protected double alpha = 0.04;


        public ToneGenerator() 
        {
            Fd = Setting.Fd;
            Magns = new double[] { 1};
        }


        /// <summary>
        /// Генерация ноты
        /// </summary>
        /// <param name="name"></param>
        /// <param name="octave"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public virtual Vector GetNoteSignal(string name, int octave, double time)
        {
            Vector freqs = (1 << octave) * BaseFreqsNote.GetFreq(name, Magns.Count);

            int steps = (int)(time * Fd);
            Vector sig = Generate(steps, freqs);

            return sig;
        }

        // Генерация звуковой волны
        private Vector Generate(int steps, Vector freqs)
        {
            Vector Signal = new Vector(steps);
            double period = 1.0 / freqs[0];
            double per60 = 10 * period;

            for (int i = 0; i < steps; i++)
            {
                double timeStep = (double)i / Fd;
                double s = 0;

                for (int j = 0; j < Magns.Count; j++)
                {
                    s += GetSempl(j, timeStep, freqs);
                }

                Signal[i] = s * GetEnvSempl(timeStep, per60);
            }


           
            return Signal / Magns.Sum();
        }

        double GetSempl(int ind, double step, Vector freqs)
        {
            return Magns[ind] * Math.Sin(6.282 * step * freqs[ind]);
        }

        // Огибающая
        double GetEnvSempl(double step, double per60)
        {
            return Math.Exp(-alpha * (step / per60));
        }

    }
}
