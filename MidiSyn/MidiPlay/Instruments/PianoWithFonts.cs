﻿using AI;
using AI.DSP;
using Midi.Data;
using System;

namespace Midi.Instruments
{
    public class PianoWithFonts : IInstrument
    {
        private readonly TabelFonts tFont;
        private int _fd;

        public PianoWithFonts(TabelFonts tabelFonts)
        {
            tFont = tabelFonts;
        }


        public void Create(int fd)
        {
            if (fd != Setting.Fd)
            {
                throw new Exception($"Font fd is {Setting.Fd} kHz");
            }

            _fd = fd;
        }

        public NoteVec GetNoteSignal(string name, int octave, double time)
        {
            string nameNote = $"{name}_{octave}";
            double freq = BaseFreqsNote.GetFreqNote(name, octave);
            int len = (int)(time * _fd);
            Vector window = PhaseCorrectingWindow.Trapezoid(len, 0.07);
            Vector signal = tFont.GetSignal(freq).CutAndZero(len) * window;
            return new NoteVec(nameNote, time, signal);
        }
    }
}
