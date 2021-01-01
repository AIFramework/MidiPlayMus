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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var a =  new Midi2Wav("2020-12-29_213219_3.mid", 44100);
            //a.Save("synt.wav");
            a.Play();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
