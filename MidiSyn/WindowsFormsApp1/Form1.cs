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
