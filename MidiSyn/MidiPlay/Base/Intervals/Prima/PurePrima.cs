using System;
using System.Collections.Generic;
using System.Text;

namespace Midi.Base.Intervals
{
    public class PurePrima : Interval
    {
        public PurePrima(string note) : base(note, 0f, IntervalType.Prima) { }
    }
}
