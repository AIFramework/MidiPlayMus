using AI;

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
