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
using Midi.NoteSeqData;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            NoteSeq noteS = NoteSeq.MidiFileToNoteSequence("2020-12-29_213219_3.mid");
            Vector[] data = noteS.ToVectors();
            var melody = new Midi2Wav(noteS, Setting.Fd);
            melody.Play();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
