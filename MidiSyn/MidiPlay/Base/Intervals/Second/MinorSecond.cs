using System;
using System.Collections.Generic;
using System.Text;

namespace Midi.Base.Intervals.Second
{
    public class MinorSecond : Interval
    {
        public MinorSecond(string note) : base(note, 0.5f, IntervalType.Second) { }
    }
}
