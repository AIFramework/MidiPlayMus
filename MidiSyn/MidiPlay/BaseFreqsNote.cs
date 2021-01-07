using AI;
using AI.DSP;
using AI.DSP.Modulation;
using Midi.Instruments.TabelNotesGenerator;
using System;
using System.Collections.Generic;

namespace Midi
{
    /// <summary>
    /// Базовые частоты
    /// </summary>
    public class BaseFreqsNote
    {
        public static ToneGenerator Generator = new ToneGenerator();

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


        public static Vector TransferFreq(string nameBaseNot, int octaveBase, string nameTargetNote, int octaveTarget, Vector signalBase, int fd = 44100)
        {
            double stFreq = GetFreqNote(nameBaseNot, octaveBase);
            double target = GetFreqNote(nameTargetNote, octaveTarget);
            double dif = target - stFreq;

            SSB ssb = new SSB(fd, dif, SSBType.Up);
            return ssb.Modulate(new Channel(signalBase, fd)).ChData;
        }

        public static Vector TransferNote(double baseFreq, string nameTargetNote, int octaveTarget, Vector signalBase)
        {
            double target = GetFreqNote(nameTargetNote, octaveTarget);
            double k = target / baseFreq;

            Vector s = SpectrumStretching.SpectrumStretch(signalBase, k).CutAndZero(signalBase.Count);
          //  s += 0.3 * Generator.GetNoteSignal(nameTargetNote, octaveTarget, signalBase.Count / 44100).CutAndZero(signalBase.Count);
            return s;
        }




    }
}
