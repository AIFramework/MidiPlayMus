using System;
using System.Collections.Generic;
using System.Text;

namespace Midi.Base
{
    public interface IInterval
    {
        /// <summary>
        /// Базовая нота
        /// </summary>
        string Note { get; set; }

        /// <summary>
        /// Тон (0.5, 1, 1.5, 2, ...)
        /// </summary>
        float Tone { get; }

        /// <summary>
        /// Музыкальный интервал
        /// </summary>
        IntervalType IntervalType { get; }

        IEnumerable<string> GetNotes();

        bool HasInterval(IEnumerable<string> notes);
    }
}
