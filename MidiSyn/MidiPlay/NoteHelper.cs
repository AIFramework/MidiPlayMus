using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midi.Base;

namespace Midi
{
    public class NoteHelper
    {
        public static IInterval ParseInverval(IEnumerable<string> notes)
        {
            var noteBase = Interval.GetBaseNote(notes);

            var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IInterval).IsAssignableFrom(p) && p.IsClass);

            var list = types.ToList();//.Where(x => x is IInterval).Select(x=>(IInterval)x).ToList();

            foreach (var type in types)
            {
                var interval = Activator.CreateInstance(type, noteBase) as IInterval;
                if (interval.HasInterval(notes))
                    return interval;
            }
            return null;
        }
    }
}
