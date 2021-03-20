using AI;
using Midi.NoteGenerator.HMM;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Vector[] melody = DatasetGen.VectorsFromAudio("2020-12-27_181857_3.mid");
            HMMNeuroGenerator gen = new HMMNeuroGenerator(melody[0].Count, 35);
            gen.Train(new Vector[][] { melody });
            melody = gen.Generate(melody[0]);

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
