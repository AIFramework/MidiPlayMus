namespace Midi.Data
{
    /// <summary>
    /// Массив элементов шрифта
    /// </summary>
    public static class ArrayETF
    {

        public const double MAX = 100000;

        public static double MinFreq(this ElementTableFont[] elementTableFonts)
        {
            double min = elementTableFonts[0].MainFreq;

            for (int i = 1; i < elementTableFonts.Length; i++)
            {
                if (min > elementTableFonts[i].MainFreq)
                {
                    min = elementTableFonts[i].MainFreq;
                }
            }

            return min;
        }

        public static double MaxFreq(this ElementTableFont[] elementTableFonts)
        {
            double min = elementTableFonts[0].MainFreq;

            for (int i = 1; i < elementTableFonts.Length; i++)
            {
                if (min > elementTableFonts[i].MainFreq)
                {
                    min = elementTableFonts[i].MainFreq;
                }
            }

            return min;
        }

        public static ElementTableFont ClosesFtromBottom(this ElementTableFont[] elementTableFonts, double freq)
        {
            for (int i = 0; i < elementTableFonts.Length; i++)
            {
                double dif = freq - elementTableFonts[i].MainFreq;

                if (dif < 0)
                {
                    elementTableFonts[i].Dist = MAX;
                }
                else
                {
                    elementTableFonts[i].Dist = dif;
                }
            }

            ElementTableFont ret = elementTableFonts[0];

            for (int i = 1; i < elementTableFonts.Length; i++)
            {
                if (ret.Dist > elementTableFonts[i].Dist)
                {
                    ret = elementTableFonts[i];
                }
            }

            return ret;
        }
    }
}
