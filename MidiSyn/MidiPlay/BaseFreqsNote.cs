using AI;
using AI.DSP;
using AI.DSP.Modulation;
using System;
using System.Collections.Generic;

namespace MidiPlay
{
    /// <summary>
    /// Базовые частоты
    /// </summary>
    public class BaseFreqsNote
    {
        static Dictionary<string, double> freqNotes = new Dictionary<string, double>
        {
            { "c",  16.35 },
            { "c#", 17.32},
            { "d",  18.35},
            { "d#", 19.45},
            { "e",  20.60},
            { "f",  21.83},
            { "f#", 23.12},
            { "g",  24.50 },
            { "g#", 25.96},
            { "a", 27.50},
            { "a#", 29.14},
            { "b", 30.85}
        };


        /// <summary>
        /// Массив частот
        /// </summary>
        /// <param name="noteName">Имя ноты</param>
        public static Vector GetFreq(string noteName, int count)
        {
            string name = noteName.ToLower();

            if (!freqNotes.ContainsKey(name))
                throw new Exception("noteName incorrect");

            double fGarm = freqNotes[name];
            double[] fr = new double[count];

            for (int i = 0; i < count; i++)
            {
                fr[i] = (i+1) * fGarm;
            }

            return fr;
        }

        /// <summary>
        /// Основной тон ноты
        /// </summary>
        /// <param name="note"></param>
        /// <param name="octave"></param>
        /// <returns></returns>
        public static double GetFreqNote(string note, int octave) 
        {
            return (1 << octave) * freqNotes[note.ToLower()];
        }


        public static Vector TransferNote(string nameBaseNot, int octaveBase, string nameTargetNote, int octaveTarget, Vector signalBase, int fd = 44100)
        {
            double stFreq = GetFreqNote(nameBaseNot, octaveBase);
            double target = GetFreqNote(nameTargetNote, octaveTarget);
            double dif = target - stFreq;

            SSB ssb = new SSB(fd, dif, SSBType.Up);
            return ssb.Modulate(new Channel(signalBase, fd)).ChData;
        }


    }
}
