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
using Midi.NoteBase;
using Midi.NoteSeqData.Base;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var noteSeq = NoteSeq.LoadMidiAsNoteSequence("Cadillac.mid");

            var accord = new string[] { "C1", "E2", "B2" };

            var tone1 = Interval.RecognizeTone(accord[0], accord[1]);
            var tone2 = Interval.RecognizeTone(accord[1], accord[2]);


            Note note = new Note();
            note.Name = "C1";

            var v1 = ToneConverter.ToneToVector(tone1);
            var v2 = ToneConverter.ToneToVector(tone2);

            var v = v1 + v2;
            v.Add(note.ToVector());
            //NoteSeq noteS = NoteSeq.MidiFileToNoteSequence("2020-12-29_213219_3.mid");
            //Vector[] data = noteS.ToVectors();
            //var melody = new Midi2Wav(noteS, Setting.Fd);
            //melody.Play();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
