using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midi.Base
{
    public static class Constants
    {
        public static readonly string[] _notesBase = { "C", "D", "E", "F", "G", "A", "B" };
        public static readonly string[] _notesAll = { "C", "C#",  "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        public static readonly int _tonesCount = 108; // 0, 0.5, 1, 1.5, ..., 53.5
    }
}
