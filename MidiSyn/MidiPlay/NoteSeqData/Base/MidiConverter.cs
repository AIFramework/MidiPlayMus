﻿using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using Midi.Extensions;

namespace Midi.NoteSeqData.Base
{
    public class MidiConverter
    {
        /// <summary>
        /// Натягивает ноты на новую сетку с фиксированным шагом по времени
        /// </summary>
        /// <param name="notes">Музыкальные ноты</param>
        /// <param name="time">фиксированный шаг по времени(например, 50)</param>
        /// <returns></returns>
        public static List<NoteEvent> ToNewGrid(List<NoteEvent> notes, int time)
        {
            List<NoteEvent> copy = notes.Copy();
            long timeMax = notes.Max(x => x.AbsoluteTime);
            int[] times = Enumerable.Range(0, (int)timeMax / time + 2).Select(x => x * time).ToArray();
            for (int i = 0; i < copy.Count; i++)
            {
                List<long> arr = times.Select(x => Math.Abs(x - copy[i].AbsoluteTime)).ToList();
                long min = arr.Min();
                int block = arr.IndexOf(min);
                copy[i].AbsoluteTime = times[block];
            }

            return copy;
        }

        /// <summary>
        /// Преобразование абсолютного времени нот в миллисекунды 
        /// </summary>
        /// <param name="midiEvents"></param>
        /// <param name="deltaTicksPerQuarterNote"></param>
        /// <param name="currentMicroSecondsPerTick">параметр midi-файла</param>
        /// <returns></returns>
        public static List<Note> ToRealTime(List<MidiEvent> midiEvents, int deltaTicksPerQuarterNote, ref decimal currentMicroSecondsPerTick)
        {
            List<Note> result = new List<Note>();
            List<decimal> eventsTimesArr = new List<decimal>();
            decimal lastRealTime = 0m;
            decimal lastAbsoluteTime = 0m;

            for (int i = 0; i < midiEvents.Count; i++)
            {
                MidiEvent midiEvent = midiEvents[i];
                TempoEvent tempoEvent = midiEvent as TempoEvent;

                if (midiEvent.AbsoluteTime > lastAbsoluteTime)
                {
                    lastRealTime += (midiEvent.AbsoluteTime - lastAbsoluteTime) * currentMicroSecondsPerTick;
                }

                if (midiEvent is NoteOnEvent)
                {
                    NoteOnEvent noteon = midiEvent as NoteOnEvent;
                    if (noteon.Velocity != 0)
                    {
                        float startTime = (float)(lastRealTime / (decimal)1000000.0);
                        float endTime = startTime + (float)(noteon.NoteLength * currentMicroSecondsPerTick / 1000000.0m);
                        result.Add(new Note(noteon.NoteNumber, noteon.Velocity, startTime, endTime, Instrument.Default, noteon.NoteName));
                    }
                }

                lastAbsoluteTime = midiEvent.AbsoluteTime;

                if (tempoEvent != null)
                {
                    currentMicroSecondsPerTick = tempoEvent.MicrosecondsPerQuarterNote / (decimal)deltaTicksPerQuarterNote;
                    midiEvents.RemoveAt(i--);
                    continue;
                }

                eventsTimesArr.Add(lastRealTime);
            }

            //for (int i = 0; i < midiEvents.Count; i++)
            //{
            //    midiEvents[i].AbsoluteTime = (long)(eventsTimesArr[i] / 1000);
            //}

            //ToDo: test syntez
            //for (int i = 0; i < result.Count; i++)
            //{
            //    result[i].EndTime = result[i].StartTime + 0.4f;
            //}

            return result;
        }

        /// <summary>
        /// Оставляет только базовые ноты
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        public static List<NoteEvent> ToBaseNotes(List<NoteEvent> notes)
        {
            return notes.Where(x => IsDigit(x.NoteName.Last())).ToList();
        }

        /// <summary>
        /// Преобразует в матрицу (width = len, height = 28)
        /// </summary>
        /// <param name="notes"></param>
        /// <param name="time">фиксированный шаг по времени</param>
        /// <param name="start">начальная позиция во времени</param>
        /// <param name="len">время интервала (ширина матрицы = len / time)</param>
        /// <returns></returns>
        //public static Matrix ToMatrix(List<NoteEvent> notes, int time, int start, int len)
        //{
        //    var array = notes.OrderBy(x => x.AbsoluteTime).ToList();

        //    int h = Constants._notesDict.Count;
        //    int w = len / time;
        //    int padding = start / time;
        //    Matrix matrix = new Matrix(h, w);
        //    for (int i = 0; i < array.Count; i++)
        //    {
        //        var note = array[i];
        //        int absoluteTime = (int)note.AbsoluteTime;
        //        int posX = (absoluteTime - start) / time;
        //        int posY = note.GetIndex();

        //        if (posX > w - 1)
        //            break;

        //        if (posX > -1 && posY != -1)
        //            matrix[posY, posX] = 1;
        //    }

        //    return matrix;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="time"></param>
        /// <param name="shiftToLeft">задать абсолютное время с 0</param>
        /// <returns></returns>
        //public static List<NoteEvent> ToNoteEvents(Matrix matrix, int time, bool shiftToLeft = true)
        //{
        //    List<NoteEvent> notes = new List<NoteEvent>();
        //    for (int i = 0; i < matrix.H; i++)
        //    {
        //        for (int j = 0; j < matrix.W; j++)
        //        {
        //            if (matrix[i, j] > 0.5)
        //            {
        //                int absoluteTime = j * time;
        //                var noteNumber = Constants._notesDict.ElementAt(i).Value;
        //                notes.Add(new NoteEvent(absoluteTime, 1, MidiCommandCode.NoteOn, noteNumber, 100));
        //            }
        //        }
        //    }

        //    if (shiftToLeft)
        //    {
        //        var min = notes.Min(x => x.AbsoluteTime);
        //        for (int i = 0; i < notes.Count; i++)
        //        {
        //            notes[i].AbsoluteTime -= min;
        //        }
        //    }

        //    return notes;
        //}

        public static List<NoteEvent> ToMono()
        {
            return null;
        }

        private static bool IsDigit(string str)
        {
            return int.TryParse(str, out int result);
        }

        private static bool IsDigit(char sym)
        {
            return IsDigit(sym + "");
        }
    }
}
