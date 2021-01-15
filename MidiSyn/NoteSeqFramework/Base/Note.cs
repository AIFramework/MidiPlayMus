using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSeqFramework.Base
{
    [Serializable]
    public class Note
    {
        public int Pitch { get; set; }
        public int Velocity { get; set; }
        public float StartTime { get; set; }
        public float EndTime { get; set; }
        public Instrument Instrument { get; set; }
        public string Name { get; set; }

        public Note()
        {

        }

        public Note(int pitch, int velocity, float startTime, float endTime, Instrument instrument, string name)
        {
            Pitch = pitch;
            Velocity = velocity;
            StartTime = startTime;
            EndTime = endTime;
            Instrument = instrument;
            Name = name;
        }
    }
}
