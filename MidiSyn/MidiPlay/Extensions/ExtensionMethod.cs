using System;
using System.Collections.Generic;
using System.Linq;

namespace Midi.Extensions
{
    public static class ExtensionMethod
    {
        public static void AddByStartTime(this List<NoteSeqData.Base.Note> notes, NoteSeqData.Base.Note note)
        {
            notes.Add(note);
            notes.Sort((left, right) => {
                var dif = right.StartTime - left.StartTime;
                if (dif > 0)
                    return 1;
                else if (dif < 0)
                    return -1;
                else return 0;
            });
        }

        public static List<T> Copy<T>(this IEnumerable<T> source)
        {
            List<T> s = source.ToList();
            T[] t = new T[s.Count];
            s.CopyTo(t);
            return t.ToList();
        }

        public static int IndexOf<T>(this IEnumerable<T> array, T value) where T: IEquatable<T>
        {
            int i = 0;
            foreach (var item in array)
            {
                if (item.Equals(value))
                    return i;
                i++;
            }

            return -1;
        }
    }
}
