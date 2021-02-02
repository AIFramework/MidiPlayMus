﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Midi.Base.Intervals
{
    public class DoubleEnlargedPrima : Interval
    {
        public DoubleEnlargedPrima(string note) : base(note, 1f, IntervalType.Prima) { }
    }
}
