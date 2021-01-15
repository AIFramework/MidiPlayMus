using AI;
using Midi;
using Midi.Data;
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
using NoteSeqFramework;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //var a =  new Midi2Wav("2020-12-29_213219_3.mid", Setting.Fd);
            //a.Play();

            NoteSeq noteS = NoteSeq.MidiFileToNoteSequence("2020-12-29_213219_3.mid");
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
