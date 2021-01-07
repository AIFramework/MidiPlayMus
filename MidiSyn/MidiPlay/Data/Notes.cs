using AI;
using AI.DSP.MusicUtils;
using System.Collections.Generic;

namespace MidiPlay.Data
{
    public class Notes : List<NoteWithTime>
    {
        int end = 0;
        double time = 0;
        public int Fd;

        public Vector Generate() 
        {
            CalcMeta(); // Расчет длительностей
            Vector data = new Vector(end); // Создание вектора волны


            // Добавление нот
            for (int i = 0; i < Count; i++)
            {
                AddNote(data, this[i], Fd);
            }

           // EchoReverb echo = new EchoReverb(Fd);
            //echo.Echo(data, 0.16); // Эхо
            //echo.EchoInvers(data, 0.05); // Эхо

            return data/data.Max(); // Нормализация
        }

        // Добавление ноты
        static void AddNote(Vector dat, NoteWithTime note, int fd) 
        {
            int margin = (int)(note.StartTime * fd);
            Vector sig = note.Note.Signal;

            for (int i = 0; i < sig.Count; i++)
            {
                if (margin + i < dat.Count)
                {
                    dat[margin + i] += note.Volume * sig[i];
                }
                else 
                {
                    break;
                }
            }
        }


        void CalcMeta()
        {
            time = 0;

            for (int i = 0; i < Count; i++)
            {
                if (time < this[i].EndTime) 
                {
                    time = this[i].EndTime;
                }
            }

            end = (int)(time * Fd);
        }


    }

    public class NoteWithTime 
    {
        public Note Note { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public double Volume { get; set; }
    }
}
