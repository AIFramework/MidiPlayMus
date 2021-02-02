﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Midi.Base.Intervals
{
    public class EnlargedPrima : Interval
    {
        public EnlargedPrima(string note) : base(note, 0.5f, IntervalType.Prima) { }
    }
}