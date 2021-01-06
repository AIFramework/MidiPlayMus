using AI;
using AI.DSP;
using AI.DSP.Modulation;
using MidiPlay;
using MidiPlay.Data;
using MidiPlay.Instruments;
using MidiPlay.Instruments.TabelNotesGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSFTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ssf = SimpleSoundFont.Load("piano.ssf1");
        }

        int oct = 5;
        SimpleSoundFont ssf;

        private void button1_Click(object sender, EventArgs e)
        {
            double k = BaseFreqsNote.GetFreqNote("A", 4);


            Vector signal = ssf.Semples[2].Signal;

            Vector s = BaseFreqsNote.TransferNote(k, comboBox1.Text, oct, signal);

            WavMp3.Play(s, 44100);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Vector signal = ssf.Semples[3].Signal;
            //signal += k*syntPiano.GetNoteSignal(comboBox1.Text, oct, signal.Count / 44100).CutAndZero(signal.Count);
            //signal /= 1+k;
            WavMp3.Play(signal, 44100);
        }
    }
}
