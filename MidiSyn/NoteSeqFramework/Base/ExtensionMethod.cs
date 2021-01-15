using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSeqFramework.Base
{
    public static class ExtensionMethod
    {
        public static void AddByStartTime(this List<Note> notes, Note note)
        {
            notes.Add(note);
            notes.Quicksort((left, right) => right.StartTime > left.StartTime);
        }

        public static List<T> Copy<T>(this IEnumerable<T> source)
        {
            var s = source.ToList();
            var t = new T[s.Count];
            s.CopyTo(t);
            return t.ToList();
        }

        public static int IndexOf<T>(this IEnumerable<T> array, T value)
        {
            return array.ToList().IndexOf(value);
        }

        //public static int GetIndex(this NoteEvent note)
        //{
        //    return Constants._notesDict.IndexOf(new KeyValuePair<string, int>(note.NoteName, note.NoteNumber));
        //}

        private static bool IsDigit(string str)
        {
            return int.TryParse(str, out int result);
        }
    }
}
