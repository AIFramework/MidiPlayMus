using System;
using System.Collections.Generic;
using System.Text;

namespace Midi.Base.Intervals.Third
{
    public class MinorThird : Interval
    {
        public MinorThird(string note) : base(note, 1.5f, IntervalType.Third) { }
    }
}
