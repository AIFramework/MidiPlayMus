using AI;
using AI.DSP;
using AI.DSP.Modulation;
using Midi;
using Midi.Data;
using Midi.Instruments;
using Midi.Instruments.TabelNotesGenerator;
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
            comboBox1.Items.Clear();
            comboBox1.Text = "0";

            for (int i = 0; i < ssf.Semples.Length; i++)
            {
                comboBox1.Items.Add(""+i);
            }

        }

        int oct = 5;
        SimpleSoundFont ssf;

        private void button1_Click(object sender, EventArgs e)
        {

            Vector s = ssf.Semples[int.Parse(comboBox1.Text)].Signal;

            WavMp3.Play(s, Setting.Fd);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Vector signal = ssf.Semples[7].Signal;
            //signal += k*syntPiano.GetNoteSignal(comboBox1.Text, oct, signal.Count / 44100).CutAndZero(signal.Count);
            //signal /= 1+k;
            WavMp3.Play(signal, Setting.Fd);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Generator.GenFonts(ssf).Save("tabel.tsf");
        }
    }
}
