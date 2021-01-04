using AI;
using AI.DSP;
using AI.DSP.Modulation;
using MidiPlay;
using MidiPlay.Data;
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

        SimpleSoundFont ssf;

        private void button1_Click(object sender, EventArgs e)
        {

            Vector signal = BaseFreqsNote.TransferNote("A", 3, comboBox1.Text, 6, ssf.Semples[2].Signal);
            WavMp3.Play(signal, 44100);
        }
    }
}
