using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSeqFramework.Base
{
    public static class QSortExtension
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="compare">if T2 value more than T1 value return is true</param>
        public static void Quicksort<T>(this List<T> array, Func<T, T, bool> compare)
        {

            Quicksort(array, 0, array.Count - 1, compare);
        }

        private static int Partition<T>(List<T> array, int start, int end, Func<T, T, bool> compare)
        {
            T temp;//swap helper
            int marker = start;//divides left and right subarrays
            for (int i = start; i < end; i++)
            {
                if (compare(array[i], array[end])) //array[end] is pivot
                {
                    temp = array[marker]; // swap
                    array[marker] = array[i];
                    array[i] = temp;
                    marker += 1;
                }
            }
            //put pivot(array[end]) between left and right subarrays
            temp = array[marker];
            array[marker] = array[end];
            array[end] = temp;
            return marker;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="compare">equal (T2>T1)</param>
        private static void Quicksort<T>(List<T> array, int start, int end, Func<T, T, bool> compare)
        {
            if (start >= end)
            {
                return;
            }
            int pivot = Partition(array, start, end, compare);
            Quicksort(array, start, pivot - 1, compare);
            Quicksort(array, pivot + 1, end, compare);
            
        }
    }
}
