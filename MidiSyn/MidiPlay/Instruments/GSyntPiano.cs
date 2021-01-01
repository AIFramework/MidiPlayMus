using AI;
using AI.DSP.MusicUtils;
using System;

namespace MidiPlay.Instruments
{
    public class GSyntPiano : IInstrument
    {
        
        /// Массив апмлитуд
        /// </summary>
        public Vector Magns { get; protected set; }

        /// <summary>
        /// Частота дискретизации
        /// </summary>
        public int Fd { get; protected set; }

        protected double alpha = 0.3;


        /// <summary>
        /// Создание пианино
        /// </summary>
        /// <param name="fd"></param>
        public virtual void Create(int fd = 16000)
        {
            Fd = fd;
            Magns = new double[] { 0.99, 1, 0.518, 0.7265, 0.168, 0.08, 0.1, 0.09, 0.04, 0.11 };
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

            string nameNote = $"{name}_{octave}";
            Vector freqs = (1 << octave) * BaseFreqsNote.GetFreq(name, Magns.Count);

            int steps = (int)(time * Fd);
            Vector window = PhaseCorrectingWindow.Trapezoid(steps, 0.07);
            Vector sig = Generate(steps, freqs, window);

            return new Note(nameNote, time, sig);
        }

        // Генерация звуковой волны
        private Vector Generate(int steps, Vector freqs, Vector window)
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


            Signal = Postprocessing(Signal, freqs, window);
            return Signal / Magns.Sum();
        }

        private double GetSempl(int ind, double step, Vector freqs)
        {
            return Magns[ind] * Math.Sin(6.282 * step * freqs[ind]);
        }

        // Огибающая
        private double GetEnvSempl(double step, double per60)
        {
            return Math.Exp(-alpha * (step / per60));
        }

        protected virtual Vector Postprocessing(Vector inp, Vector freqs, Vector window)
        {
            return inp*window;
        }
    }
}
