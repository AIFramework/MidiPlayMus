using AI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace Midi.Data
{
    public class TabelFonts
    {
        private readonly Dictionary<double, Vector> font = new Dictionary<double, Vector>();

        public TabelFonts(Dictionary<double, Vector> freqSig)
        {
            font = freqSig;
        }


        public Vector GetSignal(double freq)
        {
            return font[freq];
        }

        public void Add(string nameNote, int octave, Vector signal)
        {
            double freq = BaseFreqsNote.GetFreqNote(nameNote, octave);

            if (font.ContainsKey(freq))
            {
                font.Add(freq, signal);
            }
        }

        /// <summary>
        /// Сохранение нот
        /// </summary>
        /// <param name="path">Путь до нот</param>
        public void Save(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter bformatter = new BinaryFormatter
                {
                    AssemblyFormat = FormatterAssemblyStyle.Simple
                };

                bformatter.Serialize(stream, font);
            }
        }

        /// <summary>
        /// Загрузка нот
        /// </summary>
        /// <param name="path">Путь до нот</param>
        public static TabelFonts Load(string path)
        {
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter
                    {
                        AssemblyFormat = FormatterAssemblyStyle.Simple
                    };
                    if (fs.Length == 0)
                    {
                        throw new Exception("File empty");// Файл пуст
                    }

                    object data = formatter.Deserialize(fs);

                    if (data is Dictionary<double, Vector>)
                    {
                        return new TabelFonts(data as Dictionary<double, Vector>);
                    }
                    else
                    {
                        throw new Exception("Not fonts"); // не является шрифтом
                    }
                }
            }
            else
            {
                throw new FileNotFoundException(path);
            }
        }
    }
}
