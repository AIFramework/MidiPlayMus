using AI;
using System;
using System.Windows.Forms;
using Midi.NoteSeqData;
using Midi.NoteBase;
using NAudio.Midi;
using Midi.NoteSeqData.Base;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string S = "";
            for (int noteNumber = 127; noteNumber >= 0; noteNumber--)
            {
                var noteEvent = new NoteEvent(0, 1, MidiCommandCode.NoteOn, noteNumber, 100);

                var name = noteEvent.NoteName;
                var pitch = noteEvent.NoteNumber;

                var pitch1 = Note.GetPitch(name);
                if (pitch != pitch1)
                {

                }
            }

            //Example of use BOW
            var notes = new Note[2];
            notes[0] = new Note() { Name = "C0" };
            notes[1] = new Note() { Name = "D0" };
            var bow = Accord.ToBOW(notes);
            var notes1 = Accord.ToAccord(bow, 0, 1);

            


            var noteSeq = NoteSeq.LoadMidiAsNoteSequence("11.mid");
            var timesteps = NoteSeq.GroupByTimeStep(noteSeq);
            Vector[] melody = new Vector[timesteps.Length];
            
            for (int i = 0; i < melody.Length; i++)
            {
                melody[i] = Accord.ToBOW(timesteps[i]).TransformVector(x => x>0?1:0);
            }


            Matrix ritmgramm = new Matrix(melody[0].Count, melody.Length);

            for (int i = 0; i < melody[0].Count; i++)
            {
                for (int j = 0; j < melody.Length; j++)
                {
                    ritmgramm[i, j] = melody[j][i];
                }
            }

            heatMapControl1.CalculateHeatMap(ritmgramm);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
