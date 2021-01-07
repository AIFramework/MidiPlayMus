using AI;
using AI.DSP.MusicUtils;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midi
{
    public class Note
    {
        /// <summary>
        /// Имя ноты
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Время
        /// </summary>
        public double Time { get; set; }


        public Vector Signal { get; private set; }


        public Note(string name, double time, Vector signal)
        {
            Time = time;
            Name = name;
            Signal = signal;
        }



    }
}
